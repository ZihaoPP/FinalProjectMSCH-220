Shader "/Shield_Ion_Effect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (0, 1, 1, 1)
        _EmissionIntensity ("Emission Intensity", Float) = 1
        _ScrollSpeed ("UV Scroll Speed", Float) = 0.2
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
            float4 _MainTex_ST;
            float4 _EmissionColor;
            float _EmissionIntensity;
            float _ScrollSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);


                float2 scrollUV = v.uv + _Time.y * _ScrollSpeed;
                o.uv = TRANSFORM_TEX(scrollUV, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 texColor = tex2D(_MainTex, i.uv);


                float breath = (sin(_Time.y * 2) + 1.0) * 0.5;

                fixed4 emission = _EmissionColor * (breath * _EmissionIntensity);

                return texColor + emission;
            }
            ENDCG
        }
    }
}
