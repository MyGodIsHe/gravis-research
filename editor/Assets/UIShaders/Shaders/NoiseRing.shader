Shader "UIShaders/NoiseRing" {

    Properties {
        [Header(Main Circle)]
        _Radius("Radius", Range(0, 1)) = 0.3
        _Width("Width", Range(0, 2)) = 1.0
        _Color ("Color 1", Color) = (0.2, 0.4, 0.7, .3)

        [Header(Noise)]
        _Modifier2("Vertical", Range(0, 2)) = 0.6
        _Modifier1("Diagonal", Range(0, 2)) = 0.5
        _Modifier3("Speed", Range(0, 2)) = 0.35

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
        Cull Off
        ZWrite Off
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

            //PROPERTIES
            //Circle
            float _Radius;
            float _Width;
            float4 _Color;

            //Noise
            float _Modifier1;
            float _Modifier2;
            float _Modifier3;

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

            float hash(float p) {
                float3 p3  = frac(float3(p, p, p) * 443.8969);
                p3 += dot(p3, p3.yzx + 19.192);
                return 2.0*frac((p3.x + p3.y) * p3.z)-1.0;
            }

            float noise(float t) {
                float i = floor(t);
                float f = frac(t);
                return lerp(hash(i) * f, hash(i+1.0) * (f - 1.0), f);
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = (i.texcoord - float2(0.5, 0.5));
                float2 delta = float2(noise(_Time[1])*_Modifier1, noise(_Time[1]+(60.0*_Modifier2))) * abs(noise((_Modifier3*20.0)*_Time[1]));
                float r = dot(uv-delta, uv-delta);
                float g = dot(uv, uv);
                float b = dot(uv+delta, uv+delta);
                float3 _rgb = (float3(r, g, b)) - (_Radius*0.4);
                float _color = _Color * float3(r, g, b);
                _rgb = (_Width * 0.001)/(_rgb*_rgb);
                fixed4 texCol = tex2D(_MainTex, i.texcoord);
                texCol.a = _TexAlpha * texCol.a;
                return float4(_rgb * _Color, length(_rgb)* _Color.a * texCol.a);
            }
            ENDCG
        }
    }
}