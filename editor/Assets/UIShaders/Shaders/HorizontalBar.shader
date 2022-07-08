Shader "UIShaders/HorizontalBar"
{
    Properties
    {
        [Header(Bar)]
        _Progress("Progress", Range(0.0, 1.0)) = .45
        _BorderCol ("Border color", Color) = (0, 1, 1, 1)
        _BackCol ("Back color", Color) = (0, 0, 0, 1)
        _Size ("Size", Range(0.0, 1.0)) = 0.9
        _Thickness ("Thickness", Range(0.0, 1.0)) = 0.09
        _Border ("Border", Range(0.0, 1.0)) = 0.015
        _Sharpness("Sharpness", Range(0, 1000)) = 400
        _Adjustment("Adjustment", Range(0, .5)) = 0.002

        [Header(Stripes)]
        _Stripe1 ("Color 1", Color) = (1, 0, 0, 1)
        _Stripe2 ("Color 2", Color) = (1, 1, 0, 1)
        _Divisions ("Divisions", float) = 21.0
        _Offset ("Offset", float) = 0.05
        _Angle ("Angle", Range(-90, 90)) = -60

        [Header(Aspcect)]
        _Aspect ("Aspect Ratio", float) = 1.0

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
        Cull Back
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


            //Properties
            fixed4 _BorderCol, _BackCol, _Stripe1, _Stripe2;
            float _Progress, _Size, _Thickness, _Border;
            float _Offset, _Divisions, _Angle, _Aspect, _Sharpness, _Adjustment;

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

            float4 sigar(float2 uv, float size, float radius, float4 col, float _sharpness, float _fill, float _border) {
                size += _border;

                float center_1_x = (size*0.5 * _fill);
                float center_2_x = -size*0.5 + (radius*(_fill+1));
                float2 center_1 = float2(center_1_x, 0.0);
                float2 center_2 = float2(center_2_x, 0.0);                
                radius += _border;
                // Circles
                float4 circle1 = circle(uv, center_1, -.01, radius, col, _sharpness);
                float4 circle2 = circle(uv, center_2, -.01, radius, col, _sharpness);
                float4 circleColor = lerp(circle1, circle2, circle2.a);
                if (uv.x > center_2_x && uv.x < center_1_x && uv.y < radius + _Adjustment && uv.y > -radius-_Adjustment)
                    return col;
                return circleColor; 
            }

            float4 stripes(float2 uv, float _angle, float _divisions, float _offset, float4 _color1, float4 _color2) {
                _angle = _angle * (3.14/180.0);
                float2x2 rot = float2x2(cos(_angle), sin(_angle), -sin(_angle), cos(_angle));
                uv = mul(uv-float2(.5, .5), rot);
                uv += float2(.5, .5);
                float c = uv.x * _divisions + _offset;
                c = fmod(c, 1.0);
                float d = 0.001;
                c = smoothstep(0.5 - d, 0.5 + d, c);
                return lerp(_color1, _color2, c);
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord - float2(0.5-_Thickness, 0.5);
                _Progress *= 2.0;
                _Progress -= 1.0;
                uv.y *= _Aspect;
                fixed4 texCol = tex2D(_MainTex, uv);
                texCol.a = _TexAlpha * texCol.a;
                float4 _stripes = stripes(i.texcoord*_Aspect, _Angle, _Divisions, _Offset, _Stripe1, _Stripe2);
                float4 _sigar1 = sigar(uv, _Size, _Thickness, _BackCol, _Sharpness, 1.0, 0);
                float4 _sigar2 = sigar(uv, _Size, _Thickness, _stripes, _Sharpness, _Progress, 0);
                float4 _sigar3 = sigar(uv, _Size, _Thickness, _BorderCol, _Sharpness, 1.0, _Border);
                float4 col = lerp(_sigar1, _sigar2, _sigar2.a);
                col.a = _sigar1.a;
                float4 col2 = lerp(_sigar3, col, col.a);
                
                return col2 * texCol;
            }
            ENDCG
        }
    }
}