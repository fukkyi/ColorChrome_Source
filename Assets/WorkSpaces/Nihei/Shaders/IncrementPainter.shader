Shader "MappedPainter/IncPainter"
{
	Properties
	{
		_MainTex("Source", 2D) = "black" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

			sampler2D _PositionMap;
			float4x4 _ObjectToWorld;
			float4 _Pos_Rad;
			float4 _Color;
			fixed _PaintAmount;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 pos_a = tex2D(_PositionMap, i.uv);
				fixed4 inColor = tex2D(_MainTex, i.uv);

				float3 wpos = mul(_ObjectToWorld, float4(pos_a.xyz, 1)).xyz;
				float dis = distance(wpos, _Pos_Rad.xyz);

				bool dopaint = dis < _Pos_Rad.w;

				fixed4 col = dopaint ? lerp(inColor, inColor + _Color * _PaintAmount, 1.1 - (dis / _Pos_Rad.w)) : inColor;

				return col;
			}
			ENDCG
		}
	}
}
