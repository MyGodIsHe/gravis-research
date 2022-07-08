Shader "UIShaders/DottedCircle" {

    Properties {
        [Header(Circle parameters)]
        _Angle("Angle", Range(0, 360)) = 360.0
        _Density("Density", Range(0, 50)) = 8.6
        _Radius("Radius", Range(0, 1)) = 0.35
        _Height("Point Height", Range(0, 1)) = 0.005
        _Displacement("Displacement", Range(0, 1)) = 0.01

        [Header(Colors parameters)]
        _AlphaMul("Alpha multiplier", Range(1, 10)) = 4.0
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)

        [Header(Rotation)]
        [Toggle] _Rotating("Rotating", Int) = 0
        [Toggle] _RotCounterClock("Counter Clock", Int) = 0
        [KeywordEnum(Linear, Sin, Elastic, Crazy, Variant)] _RotMode ("Mode", Float) = 0
        _RotSpeed ("Speed", Range(0, 2)) = 0.1
        _Modifier1("Modifier 1", Range(0, 3)) = 1.0
        _Modifier2("Modifier 2", Range(0, 3)) = 1.0
        _Modifier3("Modifier 3", Range(0, 3)) = 1.0

        [Header(UI Mask)]
        _Stencil ("Stencil ID", Float) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader {
        Tags { "RenderType" = "Background" "Queue" = "Transparent" "PreviewType" = "Plane" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 200

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
            #include "Utils/Rotation2D.cginc"

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

            //PROPERTIES
            //Circle
            float _Angle;
            float _Density;
            float _Radius;
            float _Height;
            float _Displacement;

            //Color
            float4 _Color;
            float _AlphaMul;
            
            //Rotation
            float _RotCounterClock, _Rotating, _RotSpeed, _RotMode;
            float _Modifier1, _Modifier2, _Modifier3;

            float alpha_circle(float2 texC) {
                float dist = atan(texC.y / texC.x) * _Density;
                float init_sm = smoothstep(1.0, 0.0, frac(dist));
                float final_sm = smoothstep(0, 1.0, frac(dist));
                float circle_mul = smoothstep(_Height + _Displacement, _Height - _Displacement, distance(distance(texC.xy, float2(0.0, 0.0)), _Radius));
                float angle = atan2(texC.x, texC.y) / (2.0 * PI);
                _Angle /= 180.0;
                if (angle < 0.0) angle = 1.0 + angle;
                float ratio_fact = smoothstep(1.0 - _Angle, 1.0 - _Angle, (angle - .5) * 2.0);
                return ratio_fact * circle_mul * _AlphaMul * init_sm * final_sm;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord - float2(.5, .5);
                if(_Rotating == 1) uv = uvRotation(uv, _RotMode, _RotSpeed, _RotCounterClock, _Time[1], _Modifier1, _Modifier2, _Modifier3);
                return float4(_Color.rgb, alpha_circle(uv)*_Color.a);
            }
            ENDCG
        }
    }
}