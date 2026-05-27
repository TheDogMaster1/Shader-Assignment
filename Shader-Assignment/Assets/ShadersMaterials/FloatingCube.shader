Shader "Unlit/FloatingCube"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _Strength ("Strength", float) = 1
        _AmbientColor("Ambient Color", Color) = (1, 1, 1, 1)
		_AI("Ambient Intensity", float) = 0.1
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
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainColor;
            float _Strength;
            float4	_AmbientColor;
			float	_AI;

            v2f vert (appdata v)
            {
                v2f o;
                float angle = sin(unity_ObjectToWorld._14 + unity_ObjectToWorld._34);
                float4x4 rotZ = {
                    1, 0, 0, 0,
					0, cos(angle + _Time.y), -sin(angle + _Time.y), 0,
					0, sin(angle + _Time.y), cos(angle + _Time.y), 0,
					0, 0, 0, 1
                    };
                float4x4 rotY = {
                    cos(angle + _Time.y), 0, sin(angle + _Time.y), 0,
                    0, 1, 0, 0,
                    -sin(angle + _Time.y), 0, cos(angle + _Time.y), 0,
                    0, 0, 0, 1
                    };
                float4x4 rotYZ = mul(rotZ, rotY);
                v.vertex = mul(rotYZ, v.vertex);
                float4 world = mul(UNITY_MATRIX_M, v.vertex);
                float offset = sin(unity_ObjectToWorld._14 + unity_ObjectToWorld._34);
                world.y += sin(_Time.y + offset) * _Strength;
                o.vertex = mul(UNITY_MATRIX_VP, world);
                o.uv = v.uv;
                o.normal = normalize(mul(UNITY_MATRIX_M, float4(v.vertex.xyz, 0)));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float diffuse = max(dot(float4(i.normal.xyz, 1), normalize(_WorldSpaceLightPos0)), 0);

				fixed4 albedo = _MainColor;

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

            float _Strength;

			float4 VSMain(float4 vertex:POSITION) : SV_POSITION
			{
				float angle = fmod(sin(unity_ObjectToWorld._14 + unity_ObjectToWorld._34), 3.14);
                float4x4 rotZ = {
                    1, 0, 0, 0,
					0, cos(angle + _Time.y), -sin(angle + _Time.y), 0,
					0, sin(angle + _Time.y), cos(angle + _Time.y), 0,
					0, 0, 0, 1
                    };
                float4x4 rotY = {
                    cos(angle + _Time.y), 0, sin(angle + _Time.y), 0,
                    0, 1, 0, 0,
                    -sin(angle + _Time.y), 0, cos(angle + _Time.y), 0,
                    0, 0, 0, 1
                    };
                float4x4 rotYZ = mul(rotZ, rotY);
                vertex = mul(rotYZ, vertex);
                float4 world = mul(UNITY_MATRIX_M, vertex);
                float offset = sin(unity_ObjectToWorld._14 + unity_ObjectToWorld._34);
                world.y += sin(_Time.y + offset) * _Strength;
                vertex = mul(UNITY_MATRIX_VP, world);
                return vertex;
			}

			float4 PSMain(float4 vertex:SV_POSITION) : SV_TARGET
			{
				return 0;
			}

			ENDCG
		}
    }
}
