Shader "UIShaders/GlowRing" {

    Properties {
        [Header(Circle parameters)]
        _Radius ("Radius", Range(0, 1)) = .6
        _CenterX ("Center X", Range(-1, 1)) = 0
        _CenterY ("Center Y", Range(-1, 1)) = 0

        [Header(Pulsing parameters)]
        [Toggle] _Pulsing ("Pulsing", Float) = 1
        _PulsingScale ("Pulsing Scale", Range(.5, 1)) = 0.5
        _PulsingSpeed ("Pulsing Speed", Range(1, 5)) = 1

        [Header(Coloring parameters)]
        [KeywordEnum(Rotating, Horizontal, Vertical, Diagonal)] _Mode ("Mode", Float) = 0
        _Speed ("Speed", Range(0, 5)) = 1
        _Density ("Density", Range(1, 3)) = 1
        _GlowIntensity ("Glow Intensity", Range(.5, 5)) = 1.85
        _AlphaAdj("Cutoff", Range(0, 1)) = 0.05
        _FilterColor ("Filter Color (no alpha)", Color) = (.25, .25, .25, 1)
        [Toggle] _SingleColor ("Single Color", Float) = 0
        _Color ("Color", Color) = (0, 1, 1, 1)

        [Header(Texturing)]
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
            #include "Utils/Extensions.cginc"
            #include "Utils/Constants.cginc"

            //PROPERTIES
            //Circle
            float _Radius, _CenterX, _CenterY;

            //Pulsing
            float _Pulsing, _PulsingSpeed, _PulsingScale;

            //Coloring
            float4 _FilterColor, _Color;
            float _Speed, _Mode, _Density, _GlowIntensity, _AlphaAdj, _SingleColor;

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

            float normalizeSpeed(float rawSpeed) {
                return rawSpeed = (1.0/rawSpeed) * .04;
            }

            fixed4 frag (v2f i) : SV_Target {
                _PulsingSpeed = normalizeSpeed(_PulsingSpeed);
                _PulsingScale *= 0.2;
                _Radius = (1.0/_Radius) + _Pulsing * sin(_Time[0]/_PulsingSpeed) * _PulsingScale;
                float2 p = _Radius*i.texcoord - (float2((0.5 + _CenterX)*_Radius, (0.5 + _CenterY)*_Radius));
                float tau = PI * 2.0;

                fixed4 texCol = tex2D(_MainTex, i.texcoord);
                texCol.a = _TexAlpha * texCol.a;

                float b = 0;
                if(_Mode==0) {
                    b = atan2(p.x,p.y)*_Density;
                }
                else if(_Mode==1) {
                    b = sin(p.x)*(pow(_Density,2));
                }
                else if(_Mode==2) {
                    b = sin(p.y)*(pow(_Density,2));
                }
                else if(_Mode==3) {
                    b = sin(length(i.texcoord - float2(_CenterX, _CenterY)))*(pow(_Density,2));
                }

                float r = length(p);
                float2 uv = float2(b/tau, r);

                _Speed = normalizeSpeed(_Speed);
                float xCol = (uv.x - (_Time[0] / _Speed)) * 3.0;
                xCol = mod(xCol, 3.0);
                float4 horColour = _FilterColor;
                
                if(xCol < 1.0){
                    horColour.r += 1.0 - xCol;
                    horColour.g += xCol;
                }
                else if(xCol < 2.0){
                    xCol -= 1.0;
                    horColour.g += 1.0 - xCol;
                    horColour.b += xCol;
                }
                else {
                    xCol -= 2.0;
                    horColour.b += 1.0 - xCol;
                    horColour.r += xCol;
                }
                horColour = (horColour * (1.0 - _SingleColor)) + (_Color * _SingleColor);
                // draw color beam
                uv = (2.0 * uv) - 1.0;
                float beamWidth = (1.4) * abs(1.0 / (60.0 * uv.y)) * _GlowIntensity;
                float4 horBeam = float4(beamWidth, beamWidth, beamWidth, 1.0);
                float4 resultColor = horBeam * horColour;
                float colorSum = resultColor.r + resultColor.g + resultColor.b;
                resultColor.a = distance(resultColor, float4(0.0, 0.0, 0.0, 1.0)) - _AlphaAdj;

                return resultColor*texCol;
            }
            ENDCG
        }
    }
    CustomEditor "GlowRingInspector"
}
