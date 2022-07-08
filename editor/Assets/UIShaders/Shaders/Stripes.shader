Shader "UIShaders/Stripes"
{
    Properties
    {
        [KeywordEnum(Diagonal, Horizontal, Vertical, Radial, Star)] _Mode ("Mode", Float) = 0
        _Color1 ("Color 1", Color) = (0, 1, 1, 1)
        _Color2 ("Color 2", Color) = (0, 0, 0, 1)
        _Divisions ("Divisions", float) = 8.0
        _Offset ("Offset", float) = 2.0
        _Angle ("Angle", Range(-90, 90)) = 45


        [Header(UI Mask)]
        _Stencil ("Stencil ID", Float) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "PreviewType" = "Plane" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        LOD 100

        Stencil
        {
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

            //Properties
            fixed4 _Color1, _Color2;
            float _Offset, _Divisions, _Angle;

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

             fixed4 frag (v2f i) : SV_Target {
                 _Angle = _Angle * (3.14/180.0);
                float2x2 rot = float2x2(cos(_Angle),sin(_Angle),-sin(_Angle),cos(_Angle));
                i.texcoord = mul(i.texcoord-float2(.5, .5), rot);
                i.texcoord += float2(.5, .5);
                float c = i.texcoord.x * _Divisions + _Offset;
                c = fmod(c, 1.0);
                float d = 0.001;
                c = smoothstep(0.5 - d, 0.5 + d, c);
                 return lerp(_Color1, _Color2, c);
             }
             ENDCG
        }
    }
}