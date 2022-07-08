Shader "UIShaders/CircularBar" {

    Properties {
        [Header(Bar parameters)]
        _Progress("Progress", Range(0, 1)) = .45
        _Radius ("Radius", Range(0, 1)) = 0.4
        _Width ("Width", Range(0, 1)) = 0.12
        _BackWidth ("Back Width", Range(-.5, .5)) = -0.08
        _BackOffset ("Back Offset", Range(-.5, .5)) = 0
        _Sharpness ("Sharpness", Range(0, 400)) = 400
        [Toggle] _RoundedEnds("Rounded Ends", Int) = 1

        [Header(Colors parameters)]
        [KeywordEnum(Diagonal, Horizontal, Vertical, Radial, Star)] _Mode ("Mode", Float) = 0
        _Color1 ("Color 1", Color) = (0, 0, 0, 1)
        _Color2 ("Color 2", Color) = (1, 1, 1, 1)
        [Toggle] _Inverse ("Inverse", Float) = 0
        _Balance ("Balance", Range(-5.0, 5.0)) = 2
        _BackColor ("Back color", Color) = (1, 1, 1, 1)

        [Header(Rotation)]
        [Toggle] _Rotating("Rotating", Int) = 0
        [Toggle] _RotCounterClock("Counter Clock", Int) = 0
        [KeywordEnum(Linear, Sin, Elastic, Crazy, Variant)] _RotMode ("Mode", Float) = 0
        _RotSpeed ("Speed", Range(0.1, 2)) = 0.4
        _Modifier1("Modifier 1", Range(0, 3)) = 1.0
        _Modifier2("Modifier 2", Range(0, 3)) = 1.0
        _Modifier3("Modifier 3", Range(0, 3)) = 1.0

        [Header(Jitter parameters)]
        _RadiusJitter ("Radius jitter ratio", Range(0, 1)) = 0
        _RadiusJSpeed ("Radius jitter speed", Range(1, 5)) = 1
        _WidthJitter ("Width jitter ratio", Range(0, 1)) = 0
        _WidthJSpeed ("Width jitter speed", Range(1, 5)) = 1

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
            #include "Utils/Circles.cginc"
            #include "Utils/Rotation2D.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            //PROPERTIES
            //Bar
            float _Radius, _Width, _BackWidth, _BackOffset, _Sharpness, _Progress;
            float _RoundedEnds;
            
            //Colors
            float4 _Color1, _Color2, _BackColor;
            float _Inverse, _Balance, _Mode;

            //Rotation
            float _RotCounterClock, _Rotating, _RotSpeed, _RotMode;
            float _Modifier1, _Modifier2, _Modifier3;

            //Jitter
            float _RadiusJSpeed, _RadiusJitter;
            float _WidthJSpeed, _WidthJitter;

            //Texturing
            sampler2D _MainTex;
            float _TexAlpha;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            float normalizeSpeed(float rawSpeed) {
                return rawSpeed = (1.0/rawSpeed) * .04;
            }

            fixed4 frag(v2f i) : SV_Target{
                _Progress = _Progress * 360.0;
                float _AngleStart = -180.0;
                _Progress = _Progress - 180.0;
                float2 center = float2(.5, .5);
                float2 uv = i.texcoord-center;
                float4 gradColor = gradient2(uv+center, _Color1, _Color2, _Inverse, _Mode, _Balance);
                fixed4 texCol = tex2D(_MainTex, uv+center);
                texCol.a = _TexAlpha * texCol.a;
                uv = uvRotation(uv, _RotMode, _RotSpeed*_Rotating, _RotCounterClock, _Time[1], _Modifier1, _Modifier2, _Modifier3);

                _RadiusJSpeed = normalizeSpeed(_RadiusJSpeed);
                _RadiusJitter *= 0.2;

                _WidthJSpeed = normalizeSpeed(_WidthJSpeed);
                _WidthJitter *= 0.2;

                _Radius = _Radius + sin(_Time/_RadiusJSpeed) * _RadiusJitter;
                _Width = _Width + sin(_Time/_WidthJSpeed) * _WidthJitter;

                float radiusStart = _Radius - _Width*.5;
                float radiusEnd = _Radius + _Width*.5;

                float backRadiusStart = (_Radius+_BackOffset) - (_Width + _BackWidth)*.5;
                float backRadiusEnd = (_Radius+_BackOffset) + (_Width + _BackWidth)*.5;
               
                float angle = ( atan2(-(uv.x), -(uv.y)) / (PI/180.0) );

                float4 _backCircle = circle(uv, float2(0, 0), backRadiusStart, backRadiusEnd, _BackColor, _Sharpness)* texCol;
                float3 color3 = float3(gradColor.r, gradColor.g, gradColor.b);
                float4 _circle = circle(uv, float2(0, 0), radiusStart, radiusEnd, gradColor, _Sharpness) * texCol;

                float2 rotCenter1 = rotationAngle(_Radius*1.0015, (_Progress-180.0)*(PI/180.0))-center;
                float2 rotCenter2 = rotationAngle(_Radius*1.0015, (_AngleStart-180.0)*(PI/180.0))-center;
                float4 circle3 = circle(uv, rotCenter1, -.1, _Width*.5, gradColor, 30000);
                float4 circle4 = circle(uv, rotCenter2, -.1, _Width*.5, gradColor, 30000);
                float4 circles = lerp(circle4, circle3, circle3.a);

                if (angle <= _Progress && angle >= _AngleStart) {
                    return lerp(_circle, circles, circles.a*_RoundedEnds);
                }

                return lerp(_backCircle, circles, circles.a*_RoundedEnds);
            }
            ENDCG
        }
    }
}