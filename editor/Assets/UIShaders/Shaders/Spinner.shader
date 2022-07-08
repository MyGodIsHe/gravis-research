Shader "UIShaders/Spinner" {

    Properties {
        [Header(Spinner Bar)]
        _Radius ("Radius", Range(0, 1)) = 0.4
        _Width ("Width", Range(0, 1)) = 0.12
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5
        _Sharpness("Sharpness", Range(0, 400)) = 400
        _Color ("Color", Color) = (1, 1, 1, 1)

        [Header(Range)]
        _Amplitude("Amplitude", Range(0, 3)) = 2.75
        _RangeSpeed ("Speed", Range(0, 10)) = 1

        [Header(Rotation)]
        _RotSpeed ("Speed", Range(0, 10)) = 1
        [Toggle] _RotCounterClock("Counter Clock", Int) = 0
        _Modifier1("Modifier 1", Range(0, 3)) = 1.0
        _Modifier2("Modifier 2", Range(0, 3)) = 1.0
        _Modifier3("Modifier 3", Range(0, 3)) = 1.0
        
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
            #include "Utils/Rotation2D.cginc" 

            //PROPERTIES
            //Spinner bar
            float _Radius, _Width, _Sharpness;
            float _CenterX, _CenterY;
            float4 _Color;

            //Range
            float _Amplitude, _RangeSpeed;

            //Rotation
            float _Modifier1, _Modifier2, _Modifier3;
            float _RotSpeed, _RotCounterClock;
            
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
                float2 center = float2(_CenterX, _CenterY);
                fixed4 texCol = tex2D(_MainTex, uv);
                texCol.a = _TexAlpha * texCol.a;
                float2 cuv = uv-center;
                cuv = uvRotation(cuv, 4, _RotSpeed, _RotCounterClock, _Time[1], _Modifier1, _Modifier2, _Modifier3);
                float l = length(cuv);
                float f = smoothstep(l-(.1/_Sharpness), l, _Radius+(_Width*.5));
                f -= smoothstep(l, l+(.1/_Sharpness), _Radius-(_Width*.5));
                float t = mod(_Time[1]*_RangeSpeed, TPI) - PI;
                float t1 = -PI;
                float t2 = sin(t) * (PI - 3.0-_Amplitude);
                float _a = atan2(cuv.y, cuv.x);
                f = f * step(_a, t2) * (1.0-step(_a, t1));
                float3 col = float3(0.0, 0.0, 0.0);
                col = lerp(col, _Color, f);
                return float4(col, f) * texCol; 
            }
            ENDCG
        }
    }
}

