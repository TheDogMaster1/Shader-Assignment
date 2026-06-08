Shader "CustomRenderTexture/Mandelbrot"
{
	Properties
	{
		_Zoom("Zoom",float) = 1
		_Translate("Translate", Vector) = (0,0,0,0)
	}

		SubShader
	{
	   Blend One Zero

	   Pass
	   {
		   Name "Mandelbrot"

		   CGPROGRAM
		   #include "UnityCustomRenderTexture.cginc"
		   #pragma vertex CustomRenderTextureVertexShader
		   #pragma fragment frag
		   #pragma target 3.0

		   float _Zoom;
		   float4 _Translate;

		   // Mandbrot iteration formula, for complex numbers z and c:
		   //   z = z^2 + c
		   float Mandelbrot(float2 cCoord) {
			   int iteration = 0;
			   float2 zCoord = float2(0, 0); 
			   int maxIt = 250;
			   while (zCoord.x * zCoord.x + zCoord.y * zCoord.y < 3 & iteration < maxIt) {
				   zCoord = float2(
					   zCoord.x * zCoord.x - zCoord.y * zCoord.y + cCoord.x,
					   2 * zCoord.x * zCoord.y + cCoord.y
				   );
				   iteration++;
			   }
			   return 1.0 * iteration / maxIt;
		   }

		   float4 frag(v2f_customrendertexture IN) : SV_Target
		   {
			   float2 uv = (IN.localTexcoord.xy - float2(0.5,0.5)) * 4;
			   // TODO: Zoom in on a target point + use a nice color gradient!
			   float4 color = float4(Mandelbrot(uv).xxx,1);
			   return color;
		   }

		   ENDCG
	   }
	}
}
