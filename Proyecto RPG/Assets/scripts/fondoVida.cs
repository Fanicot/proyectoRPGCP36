using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fondoVida : MonoBehaviour
{
    [SerializeField]
    private float vidaMax;
    [SerializeField]
    private float vidaBase;
    private float anchoBase;
    [SerializeField]
    private RectTransform rectTransform;
    private Vector3 originalPos;
    [SerializeField]
    private float aumentoVida;
    [SerializeField]
    private float vida;

    void Start()
    {
        originalPos = rectTransform.localPosition;
        anchoBase = rectTransform.rect.width;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IncrementoVida(aumentoVida);
        }
    }

    public void IncrementoVida(float cantidad)
    {
        vidaMax += cantidad;
        vida = vidaMax;
        IncrementoBarraVida();
    }

    public void IncrementoBarraVida()
    {
        float newWidth = (vidaMax / vidaBase) * anchoBase;
        rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
        rectTransform.localPosition = originalPos + new Vector3((newWidth - anchoBase) / 2, 0, 0);
    }
}
