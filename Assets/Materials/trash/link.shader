Shader "Custom/PixelPerfectRope"
{
    Properties
    {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _Offset ("Texture Offset", Range(0,1)) = 0.0
        _PixelsPerUnit ("Pixels Per Unit", Float) = 32.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex1;
            sampler2D _MainTex2;
            float _Offset;
            float _PixelsPerUnit;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // �����������굽��������
                float2 texCoord = floor(i.texcoord * _PixelsPerUnit) / _PixelsPerUnit;

                // ʹ��texCoordֱ�Ӳ�������
                fixed4 color1 = tex2D(_MainTex1, texCoord);
                fixed4 color2 = tex2D(_MainTex2, texCoord - float2(_Offset, 0));

                // ȥ�����ѡ��ֱ��ʹ�õ�һ�Ż�ڶ�������
                fixed4 finalColor = color1.a > 0 ? color1 : color2;

                // Ԥ��Alpha����͸����
                finalColor.rgb *= finalColor.a;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
