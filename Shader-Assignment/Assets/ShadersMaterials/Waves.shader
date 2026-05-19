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
		_AmbientColor("Ambient Color", Color) = (1, 1, 1, 1)
		_AI("Ambient Intensity", float) = 0.1
		_Smoothness("Smoothness", float) = 1
		_SI("Specular Intensity", float) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

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
				float4 normal : NORMAL;
			};

			sampler2D _MainTex;
			sampler2D _HeightMap;
			float4	_BaseColor;
			float	_WaveMult;
			float	_TimeMult;
			float	_Height;
			float	_MonsterHeight;
			int		_ShaderNums;
			float4	_AmbientColor;
			float	_AI;
			float	_Smoothness;
			float	_SI;

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
					case -1:
					v.vertex.y += -(40 * pow((muv.x - 0.5), 2) + 40 * pow((muv.y - 0.5), 2)) + _MonsterHeight;
					if(v.vertex.y < 0){
						v.vertex.y = 0;
						}
					muv.x += _Time.y * _TimeMult;
					muv.y += _Time.y * _TimeMult;
					v.vertex.y += _Height * sin((muv.x + muv.y * .5) * _WaveMult);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv * 4;
					break;

					}
					o.normal = normalize(mul(UNITY_MATRIX_M, float4(v.normal.xyz, 0)));
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float diffuse = max(dot(i.normal, normalize(_WorldSpaceLightPos0)), 0);

				// float4 reflection = _WorldSpaceLightPos0 - 2 * (_WorldSpaceLightPos0 * i.normal) * i.normal;

				float4 reflection = reflect(normalize(_WorldSpaceLightPos0), i.normal);

				float Sf = pow(max(dot(normalize(i.normal - _WorldSpaceCameraPos), reflection), 0), _Smoothness);

				fixed4 albedo = tex2D(_MainTex, i.uv);

				float4 ambientLight = _AI * _AmbientColor;

				float4 col = (ambientLight + diffuse * _LightColor0) * albedo + Sf * _SI * _LightColor0;
				return col;
			}
			ENDCG
		}

		Pass
		{
			Tags{ "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex VSMain
			#pragma fragment PSMain

			float4 VSMain(float4 vertex:POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 PSMain(float4 vertex:SV_POSITION) : SV_TARGET
			{
				return 0;
			}

			ENDCG
		}
	}
}
