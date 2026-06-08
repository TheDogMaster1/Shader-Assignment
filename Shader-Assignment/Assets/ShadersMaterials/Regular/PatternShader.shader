Shader "CustomRenderTexture/PatternShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (0,0,0,0)
        _Radius("Radius", Range(0, 1)) = 0.5
        _MainTex("InputTex", 2D) = "White" {}
        _Degrees("Degrees", Range(0, 360)) = 0 
        _Speed("Speed", float) = 1
        _Hue("Hue", Range(0, 1)) = 0
        _ShaderNums("ShaderNums", Integer) = 0
        // _OutsideColor("Outside Color", Color) = (0, 0, 0, 1)
        // _InsideColor("Inside Color", Color) = (1, 1, 0, 1)
	}

    SubShader
    {
        Blend One Zero

        Pass
        {
            Name "PatternShader"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            float4      _Color2;
            float       _Radius;
            sampler2D   _MainTex;
            float       _Degrees;
            float       _Speed;
            float       _Hue;
            int         _ShaderNums;
            // float4      _OutsideColor;
            // float4      _InsideColor;

            float myFunction(float x, float _Offset){
                float PI = 3.14159265f;
                float y = 0.5 * cos(2 * PI * x + _Offset) + 0.5;
                return y;
                }

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float PI = 3.14159265f;

                float4 _texture = tex2D(_MainTex, uv) * _Color;

                float4 color = _Color;

                switch (_ShaderNums){
                    default:
                    color = _texture;
                    break;
                // _Degrees = _Degrees + _Time.y;
                case 1:
                _Hue = _Hue + _SinTime.y;
                if(_Hue > 1){
                    _Hue = 0;
                    }

                // if(_Degrees > 360){
                //     _Degrees = 0;
                //     }

                float u1 = uv.x * cos(radians(_Degrees)) - uv.y * sin(radians(_Degrees));
                float v1 = uv.x * sin(radians(_Degrees)) + uv.y * cos(radians(_Degrees));
                
                //with a texture
                // float2 u1v1 = float2(u1, v1);
                // float4 color = tex2D(_MainTex, u1v1) * _Color;

                // checkerboard
                u1 = u1 + 256;
                v1 = v1 + 256;
                float4 colorRainbow = float4(myFunction(_Hue, 0), myFunction(_Hue, PI / 3 * 2), myFunction(_Hue, PI / 3 * 4), 1);
                float4 colorRainbowOffset = float4(myFunction(_Hue + 0.5, 0), myFunction(_Hue + 0.5, PI / 3 * 2), myFunction(_Hue + 0.5, PI / 3 * 4), 1);
                color = lerp(colorRainbow, colorRainbowOffset, fmod(floor(u1 * 10) + floor(v1 * 10), 2));

                color = color * _texture;
                break;
                
                case 2:
                float c1 = length((uv - 0.5) * 1.5);
                float dist = pow(c1, 2);
                float ring = abs(dist - 0.35);
                if (ring < .1){
                    color = _Color;
                    }
                else{
                    color = _texture;
                    }
                break;

                case 3:
                float distance = length(uv - 0.5);
                float strength = -256 * pow(distance - 0.25, 2) + 1;
                if(strength < 0){
                    strength = 0;
                    }
                if(distance < 0.5){
                    color = _Color2 * strength;
                    }
                else{
                    color = _texture;
                    }

                break;
                case -1:
                float4 texColor = tex2D(_MainTex, uv);
                color = float4(1 - texColor.r, 1 - texColor.g, 1 - texColor.b, texColor.a);
                break;

                }
				return color;
            }
            ENDCG
        }
    }
}
