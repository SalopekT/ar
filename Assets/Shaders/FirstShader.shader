Shader "Custom/ColorRelativeFill"
{
    Properties
    {
        _BaseColor ("Fill Color", Color) = (0, 0, 1, 1)
        _FillPercent ("Fill Percentage", Range(0,1)) = 0.5
        _MinY ("Object Min Y", Float) = 0.0
        _MaxY ("Object Max Y", Float) = 1.0

        _RimColor ("Rim Light Color", Color) = (1, 1, 1, 1)
        _RimPower ("Rim Light Power", Float) = 2.0
        _Glossiness ("Glossiness", Float) = 0.5
        _Metallic ("Metallic", Float) = 0.5
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        //ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _BaseColor;
            float _FillPercent;
            float _MinY;
            float _MaxY;

            float4 _RimColor;
            float _RimPower;
            float _Glossiness;
            float _Metallic;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float worldY : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2; 
            };

            v2f vert(appdata v)
            {
                v2f o;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldY = worldPos.y;
                o.normal=normalize(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float fillY = lerp(_MinY, _MaxY, _FillPercent);

                if (i.worldY < fillY){
                    //return _BaseColor;

                    // Rim lighting effect: highlight edges based on view angle
                    float rim = 1.0 - saturate(dot(i.normal, i.viewDir));
                    rim = pow(rim, _RimPower); // Rim strength based on power value

                    // Combine base color with rim color
                    half4 baseColor = _BaseColor;
                    half4 rimLight = _RimColor * rim;

                    return baseColor + rimLight;
                }
                else
                    return float4(1, 1, 1, 0.2);
                    //return float4(1, 1, 1, 1);

            }
            ENDCG
        }
    }
}
