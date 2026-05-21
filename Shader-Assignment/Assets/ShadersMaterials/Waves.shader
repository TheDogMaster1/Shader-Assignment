Shader "Unlit/Waves"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_HeightMap("HeightMap", 2D) = "black" {}
		_BaseColor("Base Color", Color) = (1, 1, 1, 1)
		_WaveMult("WaveMult", float) = 1
		_TimeMult("TimeMult", float) = 1
		_Height("Height", float) = 1
		_MonsterHeight("Monster Height", float) = 1
		_ShaderNums("Shadernums", Integer) = 0
		_AmbientColor("Ambient Color", Color) = (1, 1, 1, 1)
		_AI("Ambient Intensity", float) = 0.1
		_PixelSampleDistance ("Pixel Sample Distance", float) = 1
		_difStrength ("Difference Strength", float) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{

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
			float4	_Height_ST;
			float4	 _HeightMap_TexelSize;
			float4	_BaseColor;
			float	_WaveMult;
			float	_TimeMult;
			float	_Height;
			float	_MonsterHeight;
			int		_ShaderNums;
			float4	_AmbientColor;
			float	_AI;
			float	_PixelSampleDistance;
			float	_difStrength;

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
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
			
					break;
					case -1:
					float4 offset = tex2Dlod(_HeightMap, float4(v.uv, 0, 0));
					v.vertex.y += offset.y * _MonsterHeight;
						muv.x += _Time.y * _TimeMult;
					muv.y += _Time.y * _TimeMult;
					v.vertex.y += _Height * sin((muv.x + muv.y * .5) * _WaveMult);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					break;

					}
					// o.normal = normalize(mul(UNITY_MATRIX_M, float4(v.normal.xyz, 0)));
					o.normal = v.normal;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 colUp = tex2D(_HeightMap, float2(i.uv.x, i.uv.y + _HeightMap_TexelSize.y * _PixelSampleDistance));
                float4 colDown = tex2D(_HeightMap, float2(i.uv.x, i.uv.y - _HeightMap_TexelSize.y * _PixelSampleDistance));
                float4 colRight = tex2D(_HeightMap, float2(i.uv.x  +  _HeightMap_TexelSize.x * _PixelSampleDistance, i.uv.y));
                float4 colLeft = tex2D(_HeightMap, float2(i.uv.x  - _HeightMap_TexelSize.x  * _PixelSampleDistance, i.uv.y));

                float3 horizontalIncrease = float3(1, 0, (colRight.x - colLeft.x) * _difStrength);
                float3 verticalIncrease = float3(0, 1, (colUp.x - colDown.x) * _difStrength);
                float3 normalVector = normalize(cross(horizontalIncrease, verticalIncrease));
				float3 usedNormal = float3(1, 1, 1);
				if(_ShaderNums == -1){
					usedNormal = float3(normalVector.x, -normalVector.z, normalVector.y);
					i.uv *= 4;
				}
				else{
					usedNormal = float3 (0, -1, 0);
				}

				float diffuse = max(dot(float4(-usedNormal, 1), normalize(_WorldSpaceLightPos0)), 0);

				fixed4 albedo = tex2D(_MainTex, i.uv);

				float4 ambientLight = _AI * _AmbientColor;

				float4 col = (ambientLight + diffuse * float4(1, 1, 1, 1)) * albedo;
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
