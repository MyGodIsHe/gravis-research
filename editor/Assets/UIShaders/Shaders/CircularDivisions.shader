Shader "UIShaders/CircularDivisions" {

    Properties {
        [Header(Bar parameters)]
        _Radius ("Radius", Range(0, 1)) = 0.4
        _Width ("Width", Range(0, 1)) = 0.12
        _Divisions ("Disivions", Range(1.0, 360.0)) = 10.0
        _Speed ("Speed", Range(0, 30)) = 5.0
        _Color1 ("Color 1", Color) = (0, 1, 1, 1)
        _Color2 ("Color 2", Color) = (0, 0, 0, 1)
        _Color3 ("Color 3", Color) = (1, 1, 1, 1)
        _Sharpness ("Sharpness", Range(0, 400)) = 400
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5
        
        [Header(Rotation)]
        [Toggle] _Rotating("Rotating", Int) = 0
        [Toggle] _RotCounterClock("Counter Clock", Int) = 0
        [KeywordEnum(Linear, Sin, Elastic, Crazy, Variant)] _RotMode ("Mode", Float) = 0
        _RotSpeed ("Speed", Range(0, 2)) = 1
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

        Pass  {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Utils/Rotation2D.cginc"
            #include "Utils/Circles.cginc"

            //PROPERTIES
            //Bar
            float _Radius, _Width, _Sharpness, _Speed;
            float _Divisions;
            fixed4 _Color1, _Color2, _Color3;
            float _CenterX, _CenterY;
            float _DivOffset;
            
            //Rotation
            float _RotCounterClock, _Rotating, _RotSpeed, _RotMode;
            float _Modifier1, _Modifier2, _Modifier3;

            //Texturing
            sampler2D _MainTex;
            float _TexAlpha;

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

                float2 center = float2(_CenterX, _CenterY);
                float2 uv = i.texcoord - center;

                fixed4 texCol = tex2D(_MainTex, uv+center);
                texCol.a = _TexAlpha * texCol.a;

                uv = uvRotation(uv, _RotMode, _RotSpeed*_Rotating, _RotCounterClock, _Time[1], _Modifier1, _Modifier2, _Modifier3);
                  
                float angle = atan2(uv.y, uv.x);
                fixed number = floor(_Divisions) * 1.0;

                float intensity = pow(sin((angle)*number) + 1.0, 100.0);

                float _radiusStart = _Radius - _Width*.5;
                float _radiusEnd = _Radius + _Width*.5;
                                
                float zone = 0.0;
                float angleRad = (angle / (PI/180.0) + 180.0);
                float _step=(360.0/_Divisions);
                float _offset = mod(floor(_Time[1]*_Speed), number-0.001) * _step;
                _offset = mod(_offset, 360.0);
                if(angleRad > _offset + 0.001 && angleRad < _offset + _step + 0.001) zone = 2.0;
                
                _Color1 *= smoothstep(0.0, 1.0, intensity);
                _Color2 *= smoothstep(0.0, 1.0, zone);

                float4 divisions = float4(lerp(_Color1, _Color2, smoothstep(0.0, 1.0, zone)));
                divisions.a *= smoothstep(0.0, 1.0, intensity);

                float4 _circle = circle(uv, float2(0.0, 0.0), _radiusStart, _radiusEnd, _Color3, _Sharpness);
                float4 _circle2 = circle(uv, float2(0.0, 0.0), _radiusStart, _radiusEnd, divisions, _Sharpness);

                return lerp(_circle2, _circle, 1.0-smoothstep(0.0, 1.0, intensity)) * texCol;
            }
            ENDCG
        }
    }
}