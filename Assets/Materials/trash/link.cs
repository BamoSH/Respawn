using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTextureConnector : MonoBehaviour
{
    public Texture2D texture1;
    public Texture2D texture2;
    public Material targetMaterial;

    void Start()
    {
        // 随机选择初始贴图
        Texture2D firstTexture = Random.Range(0, 2) == 0 ? texture1 : texture2;

        // 选择另一张贴图并将它们连接
        Texture2D secondTexture = firstTexture == texture1 ? texture2 : texture1;

        // 创建一个新的Texture2D来存储连接后的结果
        int width = firstTexture.width + secondTexture.width;
        int height = Mathf.Max(firstTexture.height, secondTexture.height);

        Texture2D combinedTexture = new Texture2D(width, height);

        // 将第一张贴图拷贝到新贴图的左侧
        Color[] firstPixels = firstTexture.GetPixels();
        combinedTexture.SetPixels(0, 0, firstTexture.width, firstTexture.height, firstPixels);

        // 将第二张贴图拷贝到新贴图的右侧
        Color[] secondPixels = secondTexture.GetPixels();
        combinedTexture.SetPixels(firstTexture.width, 0, secondTexture.width, secondTexture.height, secondPixels);

        // 应用更改
        combinedTexture.Apply();

        // 将新生成的贴图应用到材质上
        targetMaterial.mainTexture = combinedTexture;
    }
}
