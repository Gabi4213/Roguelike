Shader "Custom/HologramShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _HoloTex ("Hologram Texture", 2D) = "white" {}
        _HoloColor ("Hologram Color", Color) = (0.0, 1.0, 1.0, 1.0)
        _Speed ("Speed", Range(0.1, 10.0)) = 1.0
        _Intensity ("Intensity", Range(0.1, 5.0)) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            sampler2D _HoloTex;
            float4 _HoloColor;
            float _Speed;
            float _Intensity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 hologramUV = i.uv + float2(_Time.y * _Speed, _Time.y * _Speed);
                fixed4 holo = tex2D(_HoloTex, hologramUV);
                col.rgb += holo.rgb * _HoloColor.rgb * _Intensity;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
