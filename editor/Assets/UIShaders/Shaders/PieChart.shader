Shader "UIShaders/PieChart" {

    Properties {
        [Header(Main Circle)]
        _Sharpness("Sharpness", Range(0, 400)) = 400
        _CenterX("Center X", Range(0, 1)) = 0.5
        _CenterY("Center Y", Range(0, 1)) = 0.5
        _MaxAngle("Max angle", Range(0, 360)) = 360
        _Divider("Divider", Range(0, 50)) = 0

        [Header(Data 1)]
        _Perc1 ("Percentage", Range(0, 100)) = 100
        _Color1 ("Color", Color) = (1, 0, 0, 1)
        _Radius1("Radius", Range(0, 2)) = 0.3
        _Width1 ("Width", Range(0, 1)) = 0.12

        [Header(Data 2)]
        _Perc2 ("Percentage", Range(0, 100)) = 100
        _Color2 ("Color", Color) = (1, 1, 0, 1)
        _Radius2("Radius", Range(0, 2)) = 0.3
        _Width2 ("Width", Range(0, 1)) = 0.12

        [Header(Data 3)]
        _Perc3 ("Percentage", Range(0, 100)) = 100
        _Color3 ("Color", Color) = (0, 1, 0, 1)
        _Radius3("Radius", Range(0, 2)) = 0.3
        _Width3 ("Width", Range(0, 1)) = 0.12

        [Header(Data 4)]
        _Perc4 ("Percentage", Range(0, 100)) = 100
        _Color4 ("Color", Color) = (0, 1, 1, 1)
        _Radius4("Radius", Range(0, 2)) = 0.3
        _Width4 ("Width", Range(0, 1)) = 0.12

        [Header(Data 5)]
        _Perc5 ("Percentage", Range(0, 100)) = 100
        _Color5 ("Color", Color) = (0, 0, 1, 1)
        _Radius5("Radius", Range(0, 2)) = 0.3
        _Width5 ("Width", Range(0, 1)) = 0.12

        [Header(Data 6)]
        _Perc6 ("Percentage", Range(0, 100)) = 100
        _Color6 ("Color", Color) = (1, 0, 1, 1)
        _Radius6("Radius", Range(0, 2)) = 0.3
        _Width6 ("Width", Range(0, 1)) = 0.12

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
            #include "Utils/Circles.cginc"
            #include "Utils/Constants.cginc"

            //PROPERTIES
            //Main circle
            float _Sharpness;
            float _CenterX, _CenterY, _MaxAngle;
            float _Divider;

            //Datas
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;
            float4 _Color5;
            float4 _Color6;
            float _Perc1, _Radius1, _Width1;
            float _Perc2, _Radius2, _Width2;
            float _Perc3, _Radius3, _Width3;
            float _Perc4, _Radius4, _Width4;
            float _Perc5, _Radius5, _Width5;
            float _Perc6, _Radius6, _Width6;         

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
                float angle = ( atan2((uv.x-.5), (uv.y-.5)) / (PI/180.0) );
                float4 _circle = float4(0.0, 0.0, 0.0, 0.0);

                fixed4 texCol = tex2D(_MainTex, uv);
                texCol.a = _TexAlpha * texCol.a;
                float halfDiv = _Divider *.5;

                float totalPerc = _Perc1 + _Perc2 + _Perc3 + _Perc4 + _Perc5 + _Perc6;
                float normPerc1 = ((_Perc1 / totalPerc) * _MaxAngle);
                float normPerc2 = ((_Perc2 / totalPerc) * _MaxAngle) + normPerc1;
                float normPerc3 = ((_Perc3 / totalPerc) * _MaxAngle) + normPerc2;
                float normPerc4 = ((_Perc4 / totalPerc) * _MaxAngle) + normPerc3;
                float normPerc5 = ((_Perc5 / totalPerc) * _MaxAngle) + normPerc4;
                float normPerc6 = ((_Perc6 / totalPerc) * _MaxAngle) + normPerc5;

                if(angle<0) angle = (2.0 * 180.0) + angle;

                if (angle >= halfDiv && angle <= normPerc1 - halfDiv) {
                    _circle = circle(uv, center, _Radius1-_Width1*.5, _Radius1+_Width1*.5, _Color1, _Sharpness);
                }
                if (angle >= normPerc1 + halfDiv && angle <= normPerc2 - halfDiv) {
                    _circle = circle(uv, center, _Radius2-_Width2*.5, _Radius2+_Width2*.5, _Color2, _Sharpness);
                }
                if (angle >= normPerc2 + halfDiv && angle <= normPerc3 - halfDiv) {
                    _circle = circle(uv, center, _Radius3-_Width3*.5, _Radius3+_Width3*.5, _Color3, _Sharpness);
                }
                if (angle >= normPerc3 + halfDiv && angle <= normPerc4 - halfDiv) {
                    _circle = circle(uv, center, _Radius4-_Width4*.5, _Radius4+_Width4*.5, _Color4, _Sharpness);
                }
                if (angle >= normPerc4 + halfDiv && angle <= normPerc5 - halfDiv) {
                    _circle = circle(uv, center, _Radius5-_Width5*.5, _Radius5+_Width5*.5, _Color5, _Sharpness);
                }
                if (angle >= normPerc5 + halfDiv && angle <= normPerc6 - halfDiv) {
                    _circle = circle(uv, center, _Radius6-_Width6*.5, _Radius6+_Width6*.5, _Color6, _Sharpness);
                }
                return _circle * texCol;
            }
            ENDCG
        }
    }
}

