Shader "CustomRenderTexture/PatternShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (0,0,0,0)
        _Radius("Radius", Range(0, 1)) = 0.5
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
            // float4      _OutsideColor;
            // float4      _InsideColor;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;

                //whatever powstep means
                float4 color = sin(uv.x * 3.14) * _Color;

                //checkerboard
                // float4 color = lerp(_Color, _Color2, fmod(floor(uv.x * 10) + floor(uv.y * 10), 2));

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
