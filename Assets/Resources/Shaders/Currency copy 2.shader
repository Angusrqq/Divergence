Shader "Custom/SineCircle_HDRDualColor"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}

        [HDR]_RingsColor ("Rings Color (HDR)", Color) = (1, 0.6, 0.2, 1)
        [HDR]_BetweenColor ("Between Color (HDR)", Color) = (0.05, 0.05, 0.08, 1)

        _RingsIntensity ("Rings Intensity", Float) = 5.0
        _Speed ("Animation Speed", Float) = 5.0
        _Frequency ("Ring Frequency", Float) = 8.0
        _Scale ("UV Scale", Float) = 2.0
        _Radius ("Ring Radius Offset", Float) = 0.0
        _Sharpness ("Ring Sharpness", Range(0.001, 1.0)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // (1/width, 1/height, width, height)

            float4 _RingsColor;
            float4 _BetweenColor;

            float _RingsIntensity;
            float _Speed;
            float _Frequency;
            float _Scale;
            float _Radius;
            float _Sharpness;

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

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample base texture
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Compute texture-based coordinates
                float2 fragCoord = i.uv * float2(_MainTex_TexelSize.zw);
                float2 iResolution = float2(_MainTex_TexelSize.zw);

                // GLSL equivalent: uv = (fragCoord * 2.0 - iResolution.xy) / iResolution.y;
                float2 uv = (_Scale * (fragCoord * 2.0 - iResolution.xy)) / iResolution.y;

                // Base distance from center
                float d = length(uv) + _Radius;

                // Oscillating sine pattern
                float wave = sin(d * _Frequency - _Time.y * _Speed) / 8.0;
                wave = abs(wave);

                // Control ring edge softness
                float ringMask = smoothstep(0.0, _Sharpness, wave);

                // ringMask ≈ 1 in gaps, 0 on bright rings
                float3 rings = _RingsColor.rgb * _RingsIntensity * (1.0 - ringMask);
                float3 between = _BetweenColor.rgb * ringMask;

                // Mix both HDR colors
                float3 patternColor = rings + between;

                // Modulate with texture alpha
                float3 finalColor = patternColor * texColor.a;
                float finalAlpha = texColor.a * max(_RingsColor.a, _BetweenColor.a);

                return float4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }
}
