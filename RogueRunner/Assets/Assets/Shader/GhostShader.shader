Shader "Custom/GhostShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Speed ("Fade Speed", Float) = 1.0
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
       // Cull Off

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Speed;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float alpha = abs(sin(_Time.y * _Speed));

                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;
                texColor.a *= alpha;    

                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
