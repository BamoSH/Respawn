using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public int hoverSize;
    public Color hoverColor;

    public int originalScale;
    private Color _originalColor;
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        originalScale = (int)_text.fontSize;
        _originalColor = _text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.fontSize = hoverSize;
        _text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.fontSize = originalScale;
        _text.color = _originalColor;
    }
}
