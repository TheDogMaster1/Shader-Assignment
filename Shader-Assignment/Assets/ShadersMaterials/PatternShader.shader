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
            // float4      _OutsideColor;
            // float4      _InsideColor;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;

                _Degrees = _Degrees + _Speed * _Time.y;

                if(_Degrees > 360){
                    _Degrees = 0;
                    }

                float u1 = uv.x * cos(radians(_Degrees)) - uv.y * sin(radians(_Degrees));
                float v1 = uv.x * sin(radians(_Degrees)) + uv.y * cos(radians(_Degrees));
                
                float2 u1v1 = float2(u1, v1);
                //with a texture
                // float4 color = tex2D(_MainTex, u1v1) * _Color;

                // checkerboard
                u1 = u1 + 1;
                v1 = v1 + 1;
                float4 color = lerp(_Color, _Color2, fmod(floor(u1 * 10) + floor(v1 * 10), 2));

                //lines but with strange colors
                // float4 color =lerp(_Color, _Color2, fmod(uv.x + 0.5 * uv.y, _Radius));

                // circle
                // if(length(uv - float2(0.5, 0.5)) > _Radius){
                //     color = _OutsideColor;
                //     }
                // else {
                //     color = _InsideColor;
                //     }
                // float4 color = _Radius * _Color;
				return color;
            }
            ENDCG
        }
    }
}
