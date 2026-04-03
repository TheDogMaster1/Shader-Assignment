Shader "CustomRenderTexture/Heart"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
        _Center("HeartCenter", Vector) = (0, 0, 0, 0)
        _Size("Size", Range(0, 1)) = .5
        _Center2("HeartCenter2", Vector) = (0, 0 , 0, 0)
        _Size2("Size2", Range(0, 1)) = 0.5
        _Center3("HeartCenter2", Vector) = (0, 0, 0, 0)
        _Size3("Size3", Range(0, 1)) = 0.5
     }

     SubShader
     {
        Blend One Zero

        Pass
        {
            Name "Heart"

            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            sampler2D   _MainTex;
            float4      _Center;
            float       _Size;
            float4      _Center2;
            float       _Size2;
            float4      _Center3;
            float       _Size3;

            float4 frag(v2f_customrendertexture IN) : SV_Target
            {
                float2 uv = IN.localTexcoord.xy;
                float4 color = tex2D(_MainTex, uv) * float4(1, 1, 1, 1);

                if(abs(uv.x - _Center.x) + abs(uv.y - _Center.y) < _Size || 
                length(uv - float2(_Center.x + _Size / 2, _Center.y + _Size / 2)) < sqrt(pow(_Size, 2) + pow(_Size, 2)) / 2 || 
                length(uv - float2(_Center.x - _Size / 2, _Center.y + _Size / 2)) < sqrt(pow(_Size, 2) + pow(_Size, 2)) / 2){
                    color = _Color;
                }

                if(abs(uv.x - _Center2.x) + abs(uv.y - _Center2.y) < _Size2 || 
                length(uv - float2(_Center2.x + _Size2 / 2, _Center2.y + _Size2 / 2)) < sqrt(pow(_Size2, 2) + pow(_Size2, 2)) / 2 || 
                length(uv - float2(_Center2.x - _Size2 / 2, _Center2.y + _Size2 / 2)) < sqrt(pow(_Size2, 2) + pow(_Size2, 2)) / 2){
                    color = _Color;
                }
                   
                if(abs(uv.x - _Center3.x) + abs(uv.y - _Center3.y) < _Size3 || 
                length(uv - float2(_Center3.x + _Size3 / 2, _Center3.y + _Size3 / 2)) < sqrt(pow(_Size3, 2) + pow(_Size3, 2)) / 2 || 
                length(uv - float2(_Center3.x - _Size3 / 2, _Center3.y + _Size3 / 2)) < sqrt(pow(_Size3, 2) + pow(_Size3, 2)) / 2){
                    color = _Color;
                }

                return color;
            }
            ENDCG
        }
    }
}
