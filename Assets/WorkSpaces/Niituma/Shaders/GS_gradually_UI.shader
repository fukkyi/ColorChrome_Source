Shader "Unlit/GS_gradually_UI"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _DisolveTex("DisolveTex (RGB)", 2D) = "white" {}
        [Toggle(Fade_Trigger)]
        _FadeTrigger ("FadeTrigger", Float) = 0
        _Threshold("Threshold", Range(0,1)) = 0.0
    }
    SubShader
    {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            LOD 100
            Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature Fade_Trigger

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DisolveTex;
            half _Threshold;
            fixed4 _Color;


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 m = tex2D(_DisolveTex, i.uv);
                half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                half g_col = col.r * 0.2 + col.g * 0.7 + col.b * 0.1;

            #ifdef Fade_Trigger
                if (_Threshold < g) {
                    col = fixed4(col.agb, 0);
                }
            #else
                if (g < _Threshold) {
                    col = fixed4(g_col, g_col, g_col, 1);
                }
                   
            #endif
            
                return col;
                }
            ENDCG
        }
    }
}

