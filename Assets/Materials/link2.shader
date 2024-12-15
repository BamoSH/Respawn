Shader "Custom/SingleTexturePixelPerfectRope"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OverlayColor ("Overlay Color", Color) = (1,1,1,1) // ��������ɫ����
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

            sampler2D _MainTex;
            float4 _OverlayColor;  // ���ڵ��ӵ���ɫ����
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
                // �����������굽��������ȷ�����Ĳ���
                float2 texCoord = floor(i.texcoord * _PixelsPerUnit) + 0.5;
                texCoord /= _PixelsPerUnit;

                // ��������
                fixed4 color = tex2D(_MainTex, texCoord);

                // ��������ɫ�������ɫ��ˣ�Ӧ����ɫ����
                color *= _OverlayColor;

                // Ԥ��Alpha����͸����
                color.rgb *= color.a;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
