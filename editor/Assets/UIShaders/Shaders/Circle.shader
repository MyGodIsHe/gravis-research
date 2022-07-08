Shader "UIShaders/Circle" {

    Properties {
        [Header(Main Circle)]
        _Radius("Radius", Range(0, 2)) = 0.3
        _Sharpness("Sharpness", Range(0, 400)) = 400
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5
        _Color ("Main color", Color) = (0, 1, 1, 1)
        _Color2 ("Secondary color", Color) = (0, 1, 1, 1)
        _Fill ("Fill", Range(0.0, 1.0)) = 1.0
        

        [Header(Aux Circle)]
        [Toggle] _AuxPresent("Active", Int) = 0
        _AuxColor ("Color", Color) = (0, 0, 0, 1)
        _AuxSharpness ("Sharpness", Range(0, 400)) = 400
        _AuxRadius("Radius", Range(0, 2)) = 0.08
        _AuxCenterX("Center X", Range(-.5, .5)) = 0
        _AuxCenterY("Center Y", Range(-.5, .5)) = 0
        _AuxOffset ("Offset", Range(0, 2)) = 0.4
        _AuxOrbitColor ("Orbit Color", Color) = (1, 0, 0, 1)
        _AuxOrbitWidth ("Orbit width", Range(0, 1)) = .01

        [Header(Aux Circle Rotation)]
        [Toggle] _AuxRotating("Rotating", Int) = 0
        [Toggle] _AuxRotCounterClock("Counter Clock", Int) = 0
        [KeywordEnum(Linear, Sin, Elastic, Crazy, Variant)] _AuxRotMode ("Mode", Float) = 0
        _AuxRotSpeed ("Speed", Range(0, 2)) = 1
        _AuxRotModifier1("Modifier 1", Range(0, 3)) = 1.0
        _AuxRotModifier2("Modifier 2", Range(0, 3)) = 1.0
        _AuxRotModifier3("Modifier 3", Range(0, 3)) = 1.0

        [Header(Texturing)]
        _MainTex ("Texture", 2D) = "white" {}
        _TexAlpha ("Alpha", Range(0, 1)) = 1

        [Header(Pulsing)]
        _PulsingScale ("Pulsing Scale", Range(0.0, 1)) = 0.0
        _PulsingSpeed ("Pulsing Speed", Range(1, 5)) = 1

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
            #include "Utils/Circles.cginc"
            #include "Utils/Rotation2D.cginc"

            //PROPERTIES
            //Main circle
            float _Radius, _Sharpness, _Fill;
            float _CenterX, _CenterY;
            float4 _Color, _Color2;

            //Aux circle
            float _AuxPresent, _AuxSharpness, _AuxRadius;
            float _AuxOffset, _AuxOrbitWidth;
            float4 _AuxColor, _AuxOrbitColor;
            
            //Aux circle rotation
            float _AuxCenterX, _AuxCenterY;
            float _AuxRotMode, _AuxRotCounterClock, _AuxRotating, _AuxRotSpeed;
            float _AuxRotModifier1, _AuxRotModifier2, _AuxRotModifier3;
            
            //Texturing
            sampler2D _MainTex;
            float _TexAlpha;
            
            //Pulsing
            float _PulsingScale, _PulsingSpeed;

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
                float2 uv = i.texcoord;
                float2 center = float2(_CenterX, _CenterY);

                _PulsingSpeed = normalizeSpeed(_PulsingSpeed);
                _PulsingScale *= 0.2;
                _Radius = _Radius + sin(_Time/_PulsingSpeed) * _PulsingScale;

                float4 _circle = circle(uv, center, -1.0, _Radius, _Color, _Sharpness);
                float4 _circle2 = circle(uv, center, -1.0, _Radius, _Color2, _Sharpness);
                fixed4 texCol = tex2D(_MainTex, uv);
                texCol.a = _TexAlpha * texCol.a;

                float4 resColor = _circle;
                if(uv.y < _Fill) {
                    
                    resColor = lerp(_circle, _circle2, _circle2.a);
                }

                if(_AuxPresent) {
                    float2 center2 = rotationCoord(_AuxRotMode, 
                                                   _AuxRotSpeed* (-1.0 + (2.0*_AuxRotCounterClock)),
                                                   _AuxOffset,
                                                   _Time[1]*_AuxRotating,
                                                   _AuxRotModifier1, _AuxRotModifier2, _AuxRotModifier3);
                    center2 = center2 + float2(_AuxCenterX, _AuxCenterY);
                    float4 circle2 = circle(uv, center2, -1.0, _AuxRadius, _AuxColor, _AuxSharpness);
                    float4 orbit = circle(uv, float2(.5+_AuxCenterX, .5+_AuxCenterY), _AuxOffset - _AuxOrbitWidth, _AuxOffset+_AuxOrbitWidth, _AuxOrbitColor, _AuxSharpness);
                    circle2 = lerp(orbit, circle2, circle2.a);
                    return lerp(resColor, circle2, circle2.a) * texCol;
                }
                else 
                    return resColor * texCol; 
            }
            ENDCG
        }
    }
}