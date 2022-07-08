 Shader "Madrid/MobileAdditive" 
 {
     Properties 
     {
         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
         _Color ("Tint", Color) = (1,1,1,1)
 
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
         _ColorMask ("Color Mask", Float) = 15
 
         [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
         [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
     }
 
     Category {
         Tags {  
             "Queue"="Transparent" 
             "IgnoreProjector"="True" 
             "RenderType"="Transparent" 
             "PreviewType"="Plane" 
             "CanUseSpriteAtlas"="True"
         }
         Blend SrcAlpha One
         Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
         
         Stencil
         {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp] 
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
         }
         ColorMask [_ColorMask]
 
         SubShader {
             Pass
             {
                 CGPROGRAM
                 #pragma vertex vert
                 #pragma fragment frag
                 #pragma shader_feature ETC1_EXTERNAL_ALPHA
                 #include "UnityCG.cginc"
                 #include "UnityUI.cginc"
                 
                 struct appdata_t
                 {
                     float4 vertex   : POSITION;
                     float4 color    : COLOR;
                     float2 texcoord : TEXCOORD0;
                 };
 
                 struct v2f
                 {
                     float4 vertex   : SV_POSITION;
                     fixed4 color    : COLOR;
                     float2 texcoord  : TEXCOORD0;
                     float4 worldPosition : TEXCOORD1;
                 };
                 
                 sampler2D _MainTex;
                 
                 fixed4 _Color;
                 
                 bool _UseClipRect;
                 float4 _ClipRect;
 
                 bool _UseAlphaClip;
 
          sampler2D _AlphaTex;
 
                 float _EnableExternalAlpha;
 
                 fixed SampleSpriteTextureAlpha(float2 uv)
                 {
                     fixed alpha = tex2D (_MainTex, uv).a;
     #if ETC1_EXTERNAL_ALPHA
                     // get the color from an external texture (usecase: Alpha support for ETC1 on android)
                     fixed alphaTex = tex2D (_AlphaTex, uv).r;
 
                     alpha = lerp (alpha, alphaTex, _EnableExternalAlpha);
     #endif //ETC1_EXTERNAL_ALPHA
 
                     return alpha;
                 }
             
                 v2f vert(appdata_t IN)
                 {
                     v2f OUT;
                     OUT.vertex = UnityObjectToClipPos(IN.vertex);
                     OUT.texcoord = IN.texcoord;
                     OUT.worldPosition = IN.vertex;
                     OUT.color = IN.color * _Color;
                     return OUT;
                 }
                 
                 fixed4 frag(v2f IN) : SV_Target
                 {
                     fixed4 c;
                     c.rgb = IN.color.rgb * tex2D (_MainTex, IN.texcoord).rgb;
                     c.a = IN.color.a * SampleSpriteTextureAlpha (IN.texcoord);
                     if (_UseClipRect)
                         c *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                     if (_UseAlphaClip)
                         clip (c.a - 0.001);
                     return c;
                 }
             ENDCG
             }
         }
     }
 }