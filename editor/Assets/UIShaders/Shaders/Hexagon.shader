Shader "UIShaders/Hexagon" {

    Properties {
        [Header(Hexagon)]
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5
        _HexRadius("Radius", Range(0, 1)) = 0.3
        _Width("Width", Range(0, 10)) = 0.3
        _InnerSharpness("Inner Sharpness", Range(0, 1)) = 1
        _OuterSharpness("Outer Sharpness", Range(0, 1)) = 1

        [Header(Colors)]
        [KeywordEnum(Diagonal, Horizontal, Vertical, Radial, Star)] _Mode ("Mode", Float) = 0
        _Color1 ("Color 1", Color) = (0, 1, 1, 1)
        _Color2 ("Color 2", Color) = (0, 0, 0, 1)
        [Toggle] _Inverse ("Inverse", Float) = 0
        _Balance ("Balance", Range(-5.0, 5.0)) = 1
        _Alpha("Global Alpha", Range(0, 1)) = 1.0

        [Header(Rotation)]
        [Toggle] _Rotating("Rotating", Int) = 1
        [Toggle] _RotCounterClock("Counter Clock", Int) = 0
        [KeywordEnum(Linear, Sin, Elastic, Crazy, Variant)] _RotMode ("Mode", Float) = 0
        _RotSpeed ("Speed", Range(0.1, 2)) = 0.4
        _Modifier1("Modifier 1", Range(0, 3)) = 1.0
        _Modifier2("Modifier 2", Range(0, 3)) = 1.0
        _Modifier3("Modifier 3", Range(0, 3)) = 1.0

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
        Cull Off
        LOD 100

        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Utils/Gradient.cginc"
            #include "Utils/Rotation2D.cginc"

            //PROPERTIES
            //Hexagon
            float _HexRadius, _Width, _InnerSharpness, _OuterSharpness;
            float _CenterX, _CenterY;

            //Color
            fixed _Mode;
            fixed4 _Color1, _Color2;
            float _Inverse;
            float _Balance;
            float  _Alpha;

            //Rotation
            float _RotCounterClock, _Rotating, _RotSpeed, _RotMode;
            float _Modifier1, _Modifier2, _Modifier3;

            //Texturing
            sampler2D _MainTex;
            float _TexAlpha;

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

            float smooth(float p, float r, float s) {
                return smoothstep(-s, s, p-(r));
            }

            float smin( float a, float b, float k ) {
                float h = clamp(0.5 + 0.5 * (b-a)/k, 0.0, 1.0 );
                return lerp(b, a, h) - (k * h * (1.0-h));
            }

            float smax(float a, float b, float k) {
                return (-smin(-a, -b, k));
            }

            float df(float2 pos) {
                float2 q = abs(pos);
                return smax(smax((q.x * 0.866025 + pos.y * 0.5), q.y, .05), smax((q.x * 0.866025 -pos.y*0.5), q.y, .05), .05);
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 center = float2(_CenterX, _CenterY);
                float2 uv = i.texcoord-center;
                uv = uvRotation(uv, _RotMode, _RotSpeed*_Rotating, _RotCounterClock, _Time[1], _Modifier1, _Modifier2, _Modifier3);
                fixed4 texCol = tex2D(_MainTex, uv+center);
                texCol.a = _TexAlpha * texCol.a;
                float dist = 1.0 - df((uv)*(1.0/_HexRadius));
                float shape = smooth(sin(dist * TPI), 0.5*(.1/_InnerSharpness), 0.5*(.1/_InnerSharpness));
                shape *= smooth(dist, 0.5/(8.0*_Width), 1.0*(.01/_OuterSharpness));
                float4 col = gradient2(i.texcoord, _Color1, _Color2, _Inverse, _Mode, _Balance);
                col = col * shape * distance(col, float4(0.0, 0.0, 0.0, 1.0)) * _Alpha * texCol;
                return col;
            }
            ENDCG
        }
    }
}