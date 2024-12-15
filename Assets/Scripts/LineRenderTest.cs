using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class LineRenderTest : MonoBehaviour
{
    public float startPos;
    public float endPos;
    [Header("玩家锚点位置")]
    public Transform player1;
    public Transform player2;
    [Header("颜色变化")] 
    public Color currentColor;
    public Color blinkColor;
    [Header("闪烁参数")] 
    public bool startBlink = false;
    public float blinkDuration = 0.5f;
    public PlayerDistance playerDistance;
    public GameObject audioObj;
    //private AudioManager _audioManager;
    
    private float timer;
    private bool isCurrentColor;
    private LineRenderer _lineRenderer;
    private Material _lineMaterial;
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        if (_lineRenderer != null)
        {
            _lineMaterial = _lineRenderer.material;
        }
        else
        {
            print("material error");
        }

        //_audioManager = audioObj.GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (player1 != null && player2 != null && _lineRenderer != null)
        {
            _lineRenderer.SetPosition(0, player1.position);
            _lineRenderer.SetPosition(1, player2.position);
        }
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (playerDistance.warning)
        {
            ChangeColor();
           // _audioManager.lineWarn.Play();
        }
        else
        {
            SetDefault();
        }
    }

    public void ChangeColor()
    {
        timer += Time.deltaTime;
        if (_lineMaterial != null)
        {
            if (timer >= blinkDuration)
            {
                timer = 0f;
                if (isCurrentColor)
                {
                    _lineMaterial.SetColor("_OverlayColor", blinkColor);
                    isCurrentColor = false;
                }
                else
                {
                    _lineMaterial.SetColor("_OverlayColor", currentColor);
                    isCurrentColor = true;
                }
            }
        }
    }

    public void SetDefault()
    {
        _lineMaterial.SetColor("_OverlapColor", currentColor);
    }
}
