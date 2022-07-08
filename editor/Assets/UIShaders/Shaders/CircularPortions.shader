Shader "UIShaders/CircularPortions" {
    Properties {
        [Header(Bar parameters)]
        _Radius ("Radius", Range(0, 1)) = 0.4
        _Width ("Width", Range(0, 1)) = 0.12
        _Sharpness ("Sharpness", Range(0, 400)) = 400
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5

        [Header(Portion parameters)]
        _Divisions ("Divisions", Range(4, 60)) = 4
        _DivOffset ("Div. Offset", Range(0, 180)) = 0
        _Speed ("Speed", Range(0, 6)) = 2
        [Toggle] _Clockwise("Clockwise", Int) = 0

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
        _RotSpeed ("Speed", Range(1, 2)) = 0
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

            //PROPERTIES
            //Bar
            float _Radius, _Width, _Sharpness;
            float _CenterX, _CenterY;

            //Portion
            float _Divisions;
            float _Clockwise, _Speed;
            float _DivOffset;

            //Color
            float4 _Color1, _Color2, _BackColor;
            float _Inverse, _Balance, _Mode;

            //Rotation
            float _RotCounterClock, _Rotating, _RotSpeed, _RotMode;
            float _Modifier1, _Modifier2, _Modifier3;

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

            float normalizeSpeed(float rawSpeed){
                return rawSpeed = (1.0/rawSpeed) * .04;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord - float2(.5, .5);
                fixed4 texCol = tex2D(_MainTex, uv + float2(.5, .5));
                texCol.a = _TexAlpha * texCol.a;
                uv = uvRotation(uv, _RotMode, _RotSpeed*_Rotating, _RotCounterClock, _Time[1], _Modifier1, _Modifier2, _Modifier3);
            
                float radiusStart = _Radius - _Width*.5;
                float radiusEnd = _Radius + _Width*.5;

                float2 center = float2(_CenterX, _CenterY);
                float4 gradColor = gradient2(uv+float2(.5, .5), _Color1, _Color2, _Inverse, _Mode, _Balance);

                float4 _backCircle = circle(uv, float2(0.0, 0.0), radiusStart, radiusEnd, _BackColor, _Sharpness)* texCol;
                float4 _circle = circle(uv, float2(0.0, 0.0), radiusStart, radiusEnd, gradColor, _Sharpness) * texCol;
                _Clockwise = boolToSign(_Clockwise); 
                float _step = (360.0/(int)_Divisions) * _Clockwise;
                float angleStart = (fmod((int)(_Time[2]*_Speed)*_step, 360.0)-180 * _Clockwise) - _DivOffset*.5;
                float angleEnd = angleStart + _step + _DivOffset;

                float angle = ( atan2(-(uv.x), -(uv.y)) / (PI/180.0) );
                float _min = min(angleEnd, angleStart);
                float _max = max(angleEnd, angleStart);
                if (angle <= _max && angle >= _min) {
                    return _circle;
                }
                return _backCircle;
            }
            ENDCG
        }
    }
}