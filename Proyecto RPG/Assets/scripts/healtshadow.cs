using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healtshadow : MonoBehaviour
{

    private float vida = 1;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float delay;
    private bool ready;
    private float currentdelay;
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
    
    

    void Start()
    {
        vida = vidaMax;
        originalPos = rectTransform.localPosition;
        anchoBase = rectTransform.rect.width;
    }


    void Update()
    {
        ready = Time.time > currentdelay + delay;
        if (ready)
            GetComponent<Image>().fillAmount = Mathf.Max(Mathf.Lerp(GetComponent<Image>().fillAmount, 0f, speed * Time.deltaTime), vida);
        if (Input.GetKeyDown(KeyCode.E))
        {
            IncrementoVida(aumentoVida);
        }
        
    }

    public void bajarVida(float t)
    {
        vida = t;
        currentdelay = Time.time;
    }

    public void SubirVida(float t)
    {
        vida = t;
        GetComponent<Image>().fillAmount = vida;
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
