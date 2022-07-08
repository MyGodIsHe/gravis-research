Shader "UIShaders/DottedPolygon" {

    Properties {
        [Header(Polygon parameters)]
        _Number("Number", Range(0, 50)) = 8.0
        _Size("Size", Range(0, 1)) = 0.4
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5

        [Header(Circles parameters)]
        _Radius ("Radius", Range(0, 10)) = 5
        _Sharpness ("Sharpness", Range(0, 400)) = 400
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)

        [Header(Rotation)]
        [Toggle] _Rotating("Rotating", Int) = 1
        [Toggle] _RotCounterClock("Counter Clock", Int) = 0
        [KeywordEnum(Linear, Sin, Elastic, Crazy, Variant)] _RotMode ("Mode", Float) = 0
        _RotSpeed ("Speed", Range(0, 2)) = .5
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
            
            //PROPERTIES
            //Polygon
            float _Number, _Size, _CenterX, _CenterY;

            //Circles
            float4 _Color;
            float _Sharpness, _Radius;

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

            float circle(float2 uv, float2 position, float radius) {
                return length(uv - position) - radius;
            }

            float4 mixShape(float4 colA, float4 colB, float dist, float aaSamples) {
                return lerp(colA, colB, smoothstep(0.0, -aaSamples/_Sharpness, dist));
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord - float2(_CenterX, _CenterY);                
                fixed4 texCol = tex2D(_MainTex, uv+float2(_CenterX, _CenterY));
                texCol.a = _TexAlpha * texCol.a;
                uv = uvRotation(uv, _RotMode, _RotSpeed*_Rotating, _RotCounterClock, _Time[1], _Modifier1, _Modifier2, _Modifier3);
                float4 _color = float4(1.0, 1.0, 1.0, 0.0);
                float faceDist = circle(uv, float2(0.0, 0.0), 1.0);
                float number = int(_Number);
                float _i = round(atan2(uv.y, uv.x) * number / TPI);
                float angle = (TPI / number) * _i;
                float cDist = circle(uv, float2(cos(angle), sin(angle)) * _Size, _Radius*0.01);
                _color = mixShape(_color, _Color, cDist, 1.0);
                return _color * texCol;    
            }
            ENDCG
        }
    }
}