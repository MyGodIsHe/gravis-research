Shader "UIShaders/NeonDigits"
{
    Properties
    {
        [Header(Digits)]
        _Scale("Scale", Range(0, 2)) = 0.105
        _Spacing("Spacing", Range(1, 5)) = 1.2
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5
        _Number("Number", Range(0, 9)) = 0.0

        [Header(Coloring)]
        _Color ("Color", Color) = (0.5, 1.0, 0.1, 1)
        _Glow("Glow", Range(1, 10)) = 4.0
        _Intensity("Intensity", Range(0.01, 1.0)) = 0.3
        _AlphaAdj("Cutoff", Range(0, 1)) = 0.025

        [Header(Grid)]
        _GridStrenght("Strenght", Range(1, 50)) = 20.0
        _GridSizeX("X size", Range(1, 100)) = 50.0
        _GridSizeY("Y Size", Range(1, 100)) = 50.0

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
            #include "Utils/Extensions.cginc"
            #include "Utils/Constants.cginc"

             //PROPERTIES
            //Digits
            float _Scale, _Spacing;
            float _Number;
            float _CenterX, _CenterY;
         
            //Coloring
            float _Glow, _Intensity;
            float4 _Color;
            float _AlphaAdj;

            //Grid
            float _GridSizeX, _GridSizeY, _GridStrenght;

            //Globals
            float2 digitSpacing;
            float dist;

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
         
            //Distance to a line segment
            float dfLine(float2 start, float2 end, float2 uv) {
                start *= _Scale;
                end *= _Scale;
                
                float2 _line = end - start;
                float _frac = dot(uv - start, _line) / dot(_line, _line);
                return distance(start + _line * clamp(_frac, 0.0, 1.0), uv);
            }

            //Distance to the edge of a circle
            float dfCircle(float2 origin, float radius, float2 uv) {
                origin *= _Scale;
                radius *= _Scale;
                
                return abs(length(origin-uv) - radius);
            }

            //Distance to an arc.
            float dfArc(float2 origin, float start, float sweep, float radius, float2 uv) {
                origin *= _Scale;
                radius *= _Scale;
                
                uv -= origin;
                uv = mul(uv, float2x2(cos(start), sin(start), -sin(start), cos(start)));
                
                float offs = (sweep / 2.0 - PI);
                float ang = mod(atan2(uv.y, uv.x) - offs, TPI) + offs;
                ang = clamp(ang, min(0.0, sweep), max(0.0, sweep));
                
                return distance(radius * float2(cos(ang), sin(ang)), uv);
            }

            //Length of a number in digits
            float numberLength(float n) {
                return floor(max(log(n) / log(10.0), 0.0) + 1.0);
            }

            //Distance to the digit "d" (0-9).
            float dfDigit(float2 origin, float d, float2 uv) {
                uv -= origin;
                d = floor(d);

                if(d == 0.0) {
                    dist = min(dist, dfLine(float2(1.0, 1.0), float2(1.0, 0.5), uv));
                    dist = min(dist, dfLine(float2(0.0, 1.0), float2(0.0, 0.5), uv));
                    dist = min(dist, dfArc(float2(0.5, 1.0), 0.0, 3.142, 0.5, uv));
                    dist = min(dist, dfArc(float2(0.5, 0.5), 3.142, 3.142, 0.5, uv));
                }
                else if(d == 1.0) {
                    dist = min(dist, dfLine(float2(0.5, 1.5), float2(0.5, 0.0), uv));
                }

                else if(d == 2.0) {
                    dist = min(dist, dfLine(float2(1.0, 0.0), float2(0.0,0.0), uv));
                    dist = min(dist, dfLine(float2(0.388, 0.561), float2(0.806,0.719), uv));
                    dist = min(dist, dfArc(float2(0.5, 1.0), 0.0, 2.8, 0.5, uv));
                    dist = min(dist, dfArc(float2(0.7, 1.0), -5.074, 1.209, 0.3, uv));
                    dist = min(dist, dfArc(float2(0.6, 0.0), -1.932, 1.209, 0.6, uv));
                }
                else if(d == 3.0) {
                    dist = min(dist, dfLine(float2(0.0, 1.5), float2(1.0, 1.5), uv));
                    dist = min(dist, dfLine(float2(1.0, 1.5), float2(0.5, 1.0), uv));
                    dist = min(dist, dfArc(float2(0.5, 0.5), PI, PI, 0.5, uv));
                    dist = min(dist, dfArc(float2(0.5, 0.5), 0, PI*.5, 0.5, uv));
                }
                else if(d == 4.0) {
                    dist = min(dist, dfLine(float2(0.7, 1.5), float2(0.0, 0.5), uv));
                    dist = min(dist, dfLine(float2(0.0, 0.5), float2(1.0, 0.5), uv));
                    dist = min(dist, dfLine(float2(0.7, 1.2), float2(0.7, 0.0), uv));
                }
                else if(d == 5.0) {
                    dist = min(dist, dfLine(float2(1.0, 1.5), float2(0.3, 1.5), uv));
                    dist = min(dist, dfLine(float2(0.3, 1.5), float2(0.2, 0.9), uv));
                    dist = min(dist, dfArc(float2(0.5, 0.5), PI, 5.356, 0.5, uv));
                    dist = min(dist, dfArc(float2(0.5, 0.5), 0, PI*.7, 0.5, uv));
                }
                else if(d == 6.0)
                {
                    dist = min(dist, dfLine(float2(0.067, 0.750), float2(0.5,1.5), uv));
                    dist = min(dist, dfCircle(float2(0.5, 0.5), 0.5, uv));
                    return dist;
                }
                else if(d == 7.0) {
                    dist = min(dist, dfLine(float2(0.0, 1.5), float2(1.0,1.5), uv));
                    dist = min(dist, dfLine(float2(1.0, 1.5), float2(0.5,0.0), uv));
                }
                else if(d == 8.0) {
                    dist = min(dist, dfCircle(float2(0.5, 0.4), 0.4, uv));
                    dist = min(dist, dfCircle(float2(0.5, 1.15), 0.35, uv));
                }
                else if(d == 9.0) {
                    dist = min(dist, dfLine(float2(0.933, 0.75), float2(0.5, 0.0), uv));
                    dist = min(dist, dfCircle(float2(0.5, 1.0), 0.5, uv));
                }

                return dist;
            }
                     
            //Distance to a number
            float dfNumber(float2 origin, float num, float2 uv, float _offset) {
                uv -= origin;
                float offs = _offset;
                float len = numberLength(num);
                float2 pos = digitSpacing * float2(offs,0.0);

                for(int i = len; i > -1; i--) {
                    float d = mod(num / pow(10.0, i), 10.0);
                    float2 pos = digitSpacing * float2(offs,0.0);
                    if(num > pow(10.0, i) || i == 0.0) {
                        dist = min(dist, dfDigit(pos, d, uv));
                        offs++;
                    }
                }
                return dist;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.texcoord;
                digitSpacing = float2(_Spacing, 0) * _Scale;
                float2 pos = (-digitSpacing * float2(numberLength(_Number), 1.0)*0.5) + float2(_CenterX, _CenterY);
                dist = 1e06;
                dist = min(dist, dfNumber(pos, _Number, uv, 0));
                float3 _color = float3(0.0, 0.0, 0.0);
                float shade = 0.0;
                shade = (_Glow*0.002) / (dist);
                _color += _Color * shade;
                float grid = 0.5-max(abs(mod(uv.x*_GridSizeX, 1.0)-0.5), abs(mod(uv.y*_GridSizeY, 1.0)-0.5));
                float _step = smoothstep(0.0, _GridStrenght*0.01, grid);
                _color *= _Intensity + float3(_step, _step, _step);
                return float4( _color, distance(_color, float3(0.0, 0.0, 0.0))- _AlphaAdj);
            }
            ENDCG
        }
    }
}