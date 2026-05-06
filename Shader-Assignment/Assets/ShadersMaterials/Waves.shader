Shader "Unlit/Waves"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_HeightMap("Height Map", 2D) = "Black" {}
		_BaseColor("Base Color", Color) = (1, 1, 1, 1)
		_WaveMult("WaveMult", float) = 1
		_TimeMult("TimeMult", float) = 1
		_Height("Height", float) = 1
		_MonsterHeight("Monster Height", float) = 1
		_ShaderNums("Shadernums", Integer) = 0
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent"
		"Queue" = "Transparent"
		}
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _HeightMap;
			float4	_BaseColor;
			float	_WaveMult;
			float	_TimeMult;
			float	_Height;
			float	_MonsterHeight;
			int		_ShaderNums;

			v2f vert(appdata v)
			{
				v2f o;
				float2 muv = v.uv;

				switch (_ShaderNums){
					default:
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					break;
					case 1:
					muv.x += _Time.y * _TimeMult;
					muv.y += _Time.y * _TimeMult;
					v.vertex.y += _Height * sin((muv.x + muv.y * .5) * _WaveMult);
					float4 modVertex = v.vertex;
					o.vertex = UnityObjectToClipPos(modVertex);
					o.uv = v.uv * 4;
					break;
					case 2:
					float4 offset = tex2Dlod(_HeightMap, float4(v.uv, 0, 0));
						muv.x += _Time.y * _TimeMult;
					muv.y += _Time.y * _TimeMult;
					v.vertex.y += _Height * sin((muv.x + muv.y * .5) * _WaveMult);
					v.vertex.y += offset.y * _MonsterHeight;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv * 4;
					break;
					}
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _BaseColor;
				return col;
			}
			ENDCG
		}
	}
}
