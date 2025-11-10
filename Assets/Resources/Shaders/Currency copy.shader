Shader "Custom/BoxPatternShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _N ("Box Count", Float) = 20.0
        _BaseLineWidth ("Base Line Width", Float) = 2.0
        _Saturation ("Saturation", Range(0,1)) = 0.3
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

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float _N;
            float _BaseLineWidth;
            float _Saturation;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Equivalent of vec3 hue(float h)
            float3 hue(float h)
            {
                float3 p = frac((float3(0.0, 1.0, 2.0) / 3.0) + h);
                return max(float3(0.0, 0.0, 0.0), 1.0 - abs(p * 3.0 - 1.0));
            }

            // Equivalent of mat2 rot(float angle)
            float2x2 rot(float angle)
            {
                float c = cos(angle);
                float s = sin(angle);
                return float2x2(c, -s, s, c);
            }

            // Equivalent of float box(vec2 uv, float d)
            float box(float2 uv, float d)
            {
                float r = 0.5 * d;
                uv = abs(uv);
                if (uv.x < r && uv.y < r)
                {
                    // Receive shine from both edges.
                    return (1.0 / distance(uv, float2(r, uv.y)) +
                            1.0 / distance(uv, float2(uv.x, r)));
                }
                else
                {
                    return 1.0 / distance(uv, min(uv, float2(r, r)));
                }
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Reconstruct fragCoord and iResolution equivalent
                float2 fragCoord = i.uv * _MainTex_TexelSize.xy;
                float2 iResolution = _MainTex_TexelSize.xy;

                // GLSL: vec2 uv = 4. * (fragCoord - iResolution.xy * 0.5) / iResolution.xx;
                float2 uv = 4.0 * (fragCoord - iResolution * 0.5) / iResolution.xx;

                float3 col = float3(0.0, 0.0, 0.0);

                float t = 0.5 - 0.5 * cos(_Time.y * 0.8);
                float n = lerp(1.0, _N, t);
                float s = 1.0 / n;
                float lineWidth = _BaseLineWidth * 0.001;

                // Main loop
                for (float iVal = 0.0; iVal < 1.0; iVal += s)
                {
                    // HLSL uses mul(uv, matrix) instead of matrix * uv
                    float2 rotatedUV = mul(uv, rot((iVal - t) * 3.14 * 0.5));
                    float b = lineWidth * box(rotatedUV, 1.0);

                    float k = iVal / s;
                    float f = min(1.0, (n - k));

                    float3 colorMix = ((1.0 - _Saturation) * 0.5 + 
                                        _Saturation * hue(_Time.y * 0.1 - 0.3 * k / _N));

                    col += f * min(float3(2.5, 2.5, 2.5), b * colorMix);
                }

                return float4(col, 1.0);
            }
            ENDCG
        }
    }
}
