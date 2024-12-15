Shader "Custom/SingleTexturePixelPerfectRope"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OverlayColor ("Overlay Color", Color) = (1,1,1,1) // 新增的颜色属性
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
            float4 _OverlayColor;  // 用于叠加的颜色属性
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
                // 对齐纹理坐标到像素网格，确保中心采样
                float2 texCoord = floor(i.texcoord * _PixelsPerUnit) + 0.5;
                texCoord /= _PixelsPerUnit;

                // 采样纹理
                fixed4 color = tex2D(_MainTex, texCoord);

                // 将纹理颜色与叠加颜色相乘，应用颜色调整
                color *= _OverlayColor;

                // 预乘Alpha处理透明度
                color.rgb *= color.a;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
