Shader "UIShaders/RoundRectangle" {

    Properties {
        [Header(Rectangle parameters)]
        _SizeX("Size X", Range(0, 1)) = 1
        _SizeY("Size Y", Range(0, 1)) = 1
        _Radius ("Radius", Range(0, 1)) = 0.5


        [Header(Gradient parameters)]
        [KeywordEnum(Diagonal, Horizontal, Vertical, Radial)] _Mode("Mode", Float) = 0
        _Color1("Color 1", Color) = (1, 0, 0, 1)
        _Color2("Color 2", Color) = (0, 0, 1, 1)
        _BackColor("Background", Color) = (1, 1, 1, 0)
        [Toggle] _Inverse("Inverse", Float) = 0
        _Balance("Balance", Range(-5, 5)) = 1
        
        [Header(Texturing Parameters)]
        _MainTex ("Texture", 2D) = "white" {}
        _TexAlpha ("Alpha", Range(0, 1)) = 1

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
        ZWrite Off
        Cull Off
        LOD 300

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
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Utils/Gradient.cginc"

            //PROPERTIES
            float _SizeX, _SizeY, _Radius;
            float4 _Color1, _Color2, _BackColor;
            float _Inverse, _Balance, _Mode;
            
            //Texturing
            sampler2D _MainTex;
            float _TexAlpha;

            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            float udRoundRect(float2 p, float2 b, float r)
            {
                return length(max(abs(p) - b, 0.0)) - r;
            }

            
            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord;
                float2 center = float2(0.5, 0.5);
                float2 hsize = float2(_SizeX*500.0, _SizeY*500.0);
                _Radius = _Radius * 500.0;
                fixed4 texCol = tex2D(_MainTex, uv);
                texCol.a = _TexAlpha * texCol.a;

                float a = clamp(udRoundRect(mul(uv - center, 1000), hsize - _Radius, _Radius), 0.0, 1.0);
                return lerp(gradient2(uv, _Color1, _Color2, _Inverse, _Mode, _Balance), _BackColor, a) * texCol;
            }
            ENDCG
        }
    }
}