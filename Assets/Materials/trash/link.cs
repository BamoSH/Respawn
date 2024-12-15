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
        // ���ѡ���ʼ��ͼ
        Texture2D firstTexture = Random.Range(0, 2) == 0 ? texture1 : texture2;

        // ѡ����һ����ͼ������������
        Texture2D secondTexture = firstTexture == texture1 ? texture2 : texture1;

        // ����һ���µ�Texture2D���洢���Ӻ�Ľ��
        int width = firstTexture.width + secondTexture.width;
        int height = Mathf.Max(firstTexture.height, secondTexture.height);

        Texture2D combinedTexture = new Texture2D(width, height);

        // ����һ����ͼ����������ͼ�����
        Color[] firstPixels = firstTexture.GetPixels();
        combinedTexture.SetPixels(0, 0, firstTexture.width, firstTexture.height, firstPixels);

        // ���ڶ�����ͼ����������ͼ���Ҳ�
        Color[] secondPixels = secondTexture.GetPixels();
        combinedTexture.SetPixels(firstTexture.width, 0, secondTexture.width, secondTexture.height, secondPixels);

        // Ӧ�ø���
        combinedTexture.Apply();

        // �������ɵ���ͼӦ�õ�������
        targetMaterial.mainTexture = combinedTexture;
    }
}
