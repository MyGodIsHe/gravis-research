Shader "UIShaders/CirclesSpirals" {

    Properties {
        [Header(Main Circle)]
        _Quantity("Quantity", Range(0, 16)) = 16
        _Shape("Shape", Range(0, 16)) = 16
        _EmittingSpeed("Emitting speed", Range(0.01, 1.0)) = .2
        _Distribution("Distribution", Range(0, 20)) = 16
        _ShapeRadius("Shape radius", Range(0, 400)) = 280.0
        _ShapeSpan("Shape span", Range(0, 300)) = 170
        _ShapeRotation("Shape rotation", Range(-.5, .5)) = 0.02

        [Header(Colouring)]
        [KeywordEnum(Diagonal, Horizontal, Vertical, Radial, Star)] _Mode ("Mode", Float) = 0
        _Color1 ("Color 1", Color) = (1, 1, 1, 1)
        _Color2 ("Color 2", Color) = (1, 1, 1, 1)
        [Toggle] _Inverse ("Inverse", Float) = 0
        _Balance ("Balance", Range(-5.0, 5.0)) = 2

        [Header(Circles)]
        _CircleRadius("Circles Radius", Range(0, 80)) = 50.0
        _OutLine("Circles outline", Range(0, 1)) = 0.5
        _ShIntensity("Shadow intensity", Range(0, 1)) = 0.1
        _ShSize("Shadow size", Range(0, 80)) = 5
        _ShColor1 ("Circles Shadow Color 1", Color) = (0, 0, 0, 1)
        _ShColor2 ("Circles Shadow Color 2", Color) = (1, 1, 1, 1)

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
        ZWrite Off
        LOD 400

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
            #include "Utils/Constants.cginc"
            #include "Utils/Gradient.cginc"

            //PROPERTIES
            //Main circle
            float _Quantity, _Shape, _EmittingSpeed, _Distribution;
            float _ShapeRadius, _ShapeSpan, _ShapeRotation;

            //Colouring
            float4 _Color1, _Color2;
            float _Inverse, _Balance, _Mode;
            
            //Circles
            float _OutLine, _ShIntensity, _ShSize, _CircleRadius;
            float4 _ShColor1, _ShColor2;

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

            void rotate(inout float2 n, float b) {
                n = cos(b) * n + sin(b) * float2(n.y, -n.x);
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 p = (i.texcoord - float2(0.5, 0.5)) * 1000.0;
                float sdf = 1e6;
                float dirX = 0.0;
                for (float iC = 1.0; iC < _Quantity * 4.0 - 1.0; ++iC) {
                    float circleN = iC / (_Shape * 4.0 - 1.0);
                    float t = frac(circleN + _Time[1] * _EmittingSpeed);
                    float _offset = -_ShapeRadius - _ShapeSpan * t;
                    float angle  = frac(iC / _Distribution + _Time[1] * _ShapeRotation + circleN / 8.0);
                    float radius = lerp(_CircleRadius, 0.0, 1.0 - saturate(1.2 * (1.0 - abs(2.0 * t - 1.0))));
                    float2 p2 = p;
                    rotate(p2, -angle * 2.0 * PI);
                    p2 += float2(-_offset, 0.0);
                    float dist = length(p2) - radius;
                    if (dist < sdf) {
                        dirX = p2.x / radius;
                        sdf = dist;
                    }
                }

                fixed4 texCol = tex2D(_MainTex, i.texcoord);
                texCol.a = _TexAlpha * texCol.a;

                float4 colorA = float4(1.0-_OutLine, 1.0-_OutLine, 1.0-_OutLine, 0.0);
                float4 colorB = gradient2(i.texcoord, _Color1, _Color2, _Inverse, _Mode, _Balance);
                float4 resCol = colorB;
                resCol = lerp(resCol, _ShColor1, saturate(dirX)*_ShColor1.a);
                resCol = lerp(resCol, _ShColor2, saturate(-dirX)*_ShColor2.a);
                colorB = lerp(colorB, resCol, smoothstep(0.0, 1.0, (sdf + _ShSize) * _ShIntensity));
                float val =  1.0 - smoothstep(0.0, 1.0, sdf * 0.3);
                float4 _color = lerp(colorA, colorB, float4(val, val, val, val));
                return _color * texCol;
            }
            ENDCG
        }
    }
}