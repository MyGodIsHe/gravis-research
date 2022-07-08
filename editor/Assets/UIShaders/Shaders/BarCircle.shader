Shader "UIShaders/BarCircle" {

    Properties {
        [Header(Bar parameters)]
        _Progress ("Progress", Range(0, 1)) = .45
        _BarRadius ("Radius", Range(0, 1)) = 0.45
        _BarWidth ("Width", Range(0, 1)) = 0.09
        _BarSharpness ("Sharpness", Range(0, 400)) = 400

        [Header(Inner circle parameters)]
        _CircleRadius ("Radius", Range(0, 1)) = 0.41
        _CircleWidth ("Width", Range(0, 1)) = 0.01
        _CircleSharpness ("Circle Sharpness", Range(0, 400)) = 400

        [Header(Gradient parameters)]
        [KeywordEnum(Diagonal, Horizontal, Vertical, Radial)] _Mode ("Mode", Float) = 0
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
        [Toggle] _Inverse ("Inverse", Float) = 0
        _Balance ("Balance", Range(-5, 5)) = 1

        [Header(Rotating circle parameters)]
        [Toggle] _RotCircleActive ("Active", Float) = 0
        [Toggle] _RotCounterClock("Counter Clock", Int) = 0
        _RotColor ("Color", Color) = (0, 0, 1, 1)
        _RotRadius ("Radius", Range(0, 2)) = 0.05
        _RotOffset ("Offset", Range(0, 2)) = 0.35
        
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
            #include "Utils/Circles.cginc"
            #include "Utils/Rotation2D.cginc"

            //PROPERTIES
            //Bar
            float _BarRadius, _BarWidth, _BarSharpness;
            float _AngleStart, _Progress;

            //Inner circle
            float _CircleRadius, _CircleWidth, _CircleSharpness;

            //Gradient
            float4 _Color1, _Color2;
            float _Inverse, _Balance, _Mode;

            //Rotating circle
            float4 _RotColor;
            float _RotCircleActive, _RotCounterClock;
            float _RotRadius, _RotOffset;
            
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
            
            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord;
                float2 center = float2(0.5, 0.5);
                _AngleStart = 0.0;
                _Progress = _Progress * 360.0;
                fixed4 texCol = tex2D(_MainTex, uv);
                texCol.a = _TexAlpha * texCol.a;

                _AngleStart = _AngleStart - 180.0;
                _Progress = _Progress - 180.0;
                _RotRadius *= _RotCircleActive;

                  float _BarRadiusStart, _BarRadiusEnd;
                _BarRadiusStart = _BarRadius - _BarWidth*.5;
                _BarRadiusEnd = _BarRadius + _BarWidth*.5;
                float _CircleRadiusStart, _CircleRadiusEnd;
                _CircleRadiusStart = _CircleRadius - _CircleWidth*.5;
                _CircleRadiusEnd = _CircleRadius + _CircleWidth*.5;

                float4 gradColor = gradient2(uv, _Color1, _Color2, _Inverse, _Mode, _Balance);
                float angle = ( atan2(-(uv.x-.5), -(uv.y-.5)) / (PI/180.0) );

                float2 rotCenter = rotationCoord(0, (-1.0 + (2.0*_RotCounterClock)), _RotOffset, _Time[1], 1.0, 1.0, 1.0);
                float4 rotCircle = circle(uv, rotCenter, -0.1, _RotRadius, _RotColor, _BarSharpness);

                if (angle >= _AngleStart && angle <= _Progress) {
                    float4 circle1 = circle(uv, center, _BarRadiusStart, _BarRadiusEnd, gradColor, _BarSharpness);
                    return lerp(circle1, rotCircle, rotCircle.a) * texCol;
                }
                float4 resColor = circle(uv, center, _CircleRadiusStart, _CircleRadiusEnd, gradColor, _CircleSharpness);
                return lerp(resColor, rotCircle, rotCircle.a) * texCol;
            }
            ENDCG
        }
    }
}