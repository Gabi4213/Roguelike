Shader "Custom/UIShineShader" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _ShineColor ("Shine Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _ShineWidth ("Shine Width", Range(0.01, 0.5)) = 0.1
        _ShineSpeed ("Shine Speed", Range(0.1, 5.0)) = 1.0
        _ShineDirection ("Shine Direction", Vector) = (1.0, 1.0, 0.0, 0.0)
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _ShineColor;
            float _ShineWidth;
            float _ShineSpeed;
            float3 _ShineDirection;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate shine intensity along the specified direction
                float shine = abs(sin(_Time.y * _ShineSpeed + dot(i.uv, _ShineDirection.xy))) * 2.0 - 1.0;
                shine = smoothstep(0.0, _ShineWidth, shine);

                // Apply shine color
                col.rgb = lerp(col.rgb, _ShineColor.rgb, shine);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
