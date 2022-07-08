Shader "UIShaders/Sonar"
{
    Properties
    {
        [Header(Circle 1)]
        _c1Radius("Radius", Range(0, 2)) = 0.21
        _c1Width("Width", Range(0, 2)) = 0.025
        _c1Sharpness("Sharpness", Range(0, 400)) = 200
        _c1JitterSpeed("Jitter speed", Range(0, 1)) = 0
        _c1JitterAmp("Jitter amplitude", Range(0, 1)) = 0

        [Header(Circle 2)]
        _c2Radius("Radius", Range(0, 2)) = 0.28
        _c2Width("Width", Range(0, 2)) = 0.016
        _c2Sharpness("Sharpness", Range(0, 400)) = 200
        _c2Alpha("Alpha", Range(0, 1)) = 1
        _c2JitterSpeed("Jitter speed", Range(0, 1)) = 0
        _c2JitterAmp("Jitter amplitude", Range(0, 1)) = 0

        [Header(Circle 3)]
        _c3Radius("Radius", Range(0, 2)) = 0.31
        _c3Width("Width", Range(0, 2)) = 0.013
        _c3Sharpness("Sharpness", Range(0, 400)) = 200
        _c3Alpha("Alpha", Range(0, 1)) = 1
        _c3JitterSpeed("Jitter speed", Range(0, 1)) = 0
        _c3JitterAmp("Jitter amplitude", Range(0, 1)) = 0

        [Header(Common params)]
        _Color1 ("Color 1", Color) = (0.0, 0.4, 0.9, 1)
        _Color2 ("Color 2", Color) = (0.1, 0.5, .7, 1)
        [KeywordEnum(Diagonal, Horizontal, Vertical, Radial, Star)] _Mode ("Mode", Float) = 0
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5
        [Toggle] _Inverse ("Inverse", Float) = 0
        _Balance ("Balance", Range(-5.0, 5.0)) = 1
        _Angle ("Angle", Range(0, 360.0)) = 35
        _angOffset ("Offset", Range(-1.0, 1.0)) = -.13

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
    SubShader
    {
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

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Utils/Circles.cginc"
            #include "Utils/Gradient.cginc"
            
            
            //Properties
            fixed _Mode;
            fixed4 _Color1, _Color2;
            float _CenterX, _CenterY;
            float _Inverse, _Balance;

            float _c1Radius, _c1Width, _c1Sharpness, _c1JitterSpeed, _c1JitterAmp;
            float _c2Radius, _c2Width, _c2Sharpness, _c2JitterSpeed, _c2JitterAmp;
            float _c3Radius, _c3Width, _c3Sharpness, _c3JitterSpeed, _c3JitterAmp;
            float _c2Alpha, _c3Alpha;
            float _Angle, _angOffset;

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

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord;

                float4 gradColor = gradient2(uv, _Color1, _Color2, _Inverse, _Mode, _Balance);
                float2 center = float2(_CenterX, _CenterY);
                float angle = ( atan2(-(uv.x+_angOffset), -(uv.y)) / (3.14/180.0) );
                _Angle -= 180.0;

                _c1Radius += sin(_Time[1] * _c1JitterSpeed) * _c1JitterAmp;
                _c2Radius += cos(_Time[1] * _c2JitterSpeed) * _c2JitterAmp;
                _c3Radius += sin(_Time[1] * _c3JitterSpeed) * _c3JitterAmp;

                 //circle 1
                 float _c1InnerRadius = _c1Radius - (_c1Width *.5);
                 float _c1OutherRadius = _c1Radius + (_c1Width *.5);
                float4 _circle3 = circle(uv, center, _c1InnerRadius, _c1OutherRadius, gradColor, _c1Sharpness);

                //circle 2
                float _c2InnerRadius = _c2Radius - (_c2Width *.5);
                 float _c2OutherRadius = _c2Radius + (_c2Width *.5);
                float4 _circle2 = circle(uv, center, _c2InnerRadius, _c2OutherRadius, gradColor, _c2Sharpness);
                _circle2.a *= _c2Alpha;

                //circle 3
                float _c3InnerRadius = _c3Radius - (_c3Width *.5);
                 float _c3OutherRadius = _c3Radius + (_c3Width *.5);
                float4 _circle1 = circle(uv, center, _c3InnerRadius, _c3OutherRadius, gradColor, _c3Sharpness);
                _circle1.a *= _c3Alpha;

                if(angle > _Angle && length(uv-center) < _c1Radius) {
                    return gradColor;
                }
                
                 return lerp(lerp(_circle1, _circle2, _circle2.a), _circle3, _circle3.a);
            }
            ENDCG
        }
    }
}