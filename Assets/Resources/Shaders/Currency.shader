Shader "Custom/CosineGradientCircles_Bloom"
{
    Properties
    {
        _MainTex ("Base (Alpha Controls Blend)", 2D) = "white" {}
        [HDR]_BackgroundColor ("Background Color", Color) = (0.05, 0.05, 0.08, 1)
        _CirclesBrightness ("Circles Brightness", Float) = 5.0
        _Speed ("Animation Speed", Float) = 1.0
        _CirclesCount ("Circles Count", Float) = 5.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // (1/width,1/height,width,height)

            float4 _BackgroundColor;
            float _CirclesBrightness;
            float _Speed;
            float _CirclesCount;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // The same cosine color palette you used earlier
            float3 palette(float t)
            {
                float3 a = float3(0.5, 0.5, 0.5);
                float3 b = float3(0.5, 0.5, 0.5);
                float3 c = float3(1.0, 1.0, 1.0);
                float3 d = float3(0.263, 0.416, 0.557);
                return a + b * cos(6.28318 * (c * t + d));
            }

            // Draw multiple shrinking circles tinted with the cosine gradient
            float3 circlePattern(float2 uv)
            {
                float2 center = float2(0.5, 0.5);
                float2 p = uv - center;
                float r = length(p);
                float3 col = 0;

                for (int i = 0; i < 6; i++)
                {
                    float t = (_Time.y * _Speed + i * 0.5);
                    float radius = frac(t);         // goes 0..1 repeatedly
                    //radius = 1.0 - radius;          // invert so it shrinks inward

                    float edge = smoothstep(radius, radius - 0.01, r);
                    float3 gradCol = palette(radius + i * 0.2 + _Time.y * 0.2);

                    col += gradCol * edge * (1.0 - radius); // fade smaller ones
                }

                return col * _CirclesBrightness;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                // Base background
                float3 color = _BackgroundColor.rgb;

                // Add cosine gradient circles
                float3 circles = circlePattern(i.uv);
                color += circles;

                // Blend with texture alpha
                color = lerp(texColor.rgb, color, texColor.a);

                return float4(color, texColor.a);
            }
            ENDCG
        }
    }
}
