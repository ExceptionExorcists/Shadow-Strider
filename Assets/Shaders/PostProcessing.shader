Shader "Custom/PostProcessing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Float) = 8.0
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _GritIntensity ("Grit Intensity", Float) = 0.5
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
            sampler2D _NoiseTex;
            float _PixelSize;
            float _GritIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pixelUV = i.uv;
                pixelUV *= _PixelSize; 
                pixelUV = floor(pixelUV) / _PixelSize;

                fixed4 sceneColor = tex2D(_MainTex, pixelUV);
                fixed4 noiseColor = tex2D(_NoiseTex, i.uv);
                float noiseEffect = 1.0 + noiseColor.r * _GritIntensity;

                sceneColor.rgb *= noiseEffect;

                return sceneColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
