Shader "UIShaders/SimpleGradient" {

    Properties {
        [Header(Colors)]
        [KeywordEnum(Diagonal, Horizontal, Vertical, Radial, Star)] _Mode ("Mode", Float) = 0
        _Color1 ("Color 1", Color) = (0, 1, 1, 1)
        _Color2 ("Color 2", Color) = (0, 0, 0, 1)
        [Toggle] _Inverse ("Inverse", Float) = 0
        _Balance ("Balance", Range(-5.0, 5.0)) = 1

        [Header(UI Mask)]
        _Stencil ("Stencil ID", Float) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "PreviewType" = "Plane" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        LOD 100

        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Utils/Gradient.cginc"

             //PROPERTIES
            //COLORS
             fixed _Mode;
            fixed4 _Color1, _Color2;
            float _Inverse;
            float _Balance;
            
            #pragma vertex vert
            #pragma fragment frag

            //DATA STRUCTURES
            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 vertex : SV_POSITION;
                half2 texcoord : TEXCOORD0;
            };

             v2f vert (appdata_t v) {
                 v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
             }

             fixed4 frag (v2f i) : SV_Target{
                 return gradient2(i.texcoord, _Color1, _Color2, _Inverse, _Mode, _Balance);
             }
             ENDCG
        }
    }
}
