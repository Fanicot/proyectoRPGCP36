using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class recursosPersonaje : MonoBehaviour
{
    [Header("Vida, Stamina")]
    [SerializeField]
    private float vida;
    [SerializeField]
    private float vidaMax = 100;
    [SerializeField]
    private float aumentoVida;
    [SerializeField]
    private float stamina;
    [SerializeField]
    private float staminaMax = 100;
    [SerializeField]
    private Image barradevida, barradestamina;
    private float daño = 10;
    private float restaurarVida = 10;

    [Header("Costo, recarga")]
    [SerializeField]
    private float CostoCorrer;
    [SerializeField]
    private bool corriendo;
    [SerializeField]
    private float recargaStarmina;
    private Coroutine recarga;
    [SerializeField]
    private float staminaRecoveryMargin;
    [SerializeField]
    private bool sprintReady;
    private bool corrutina;
    
    [Header("Canvas")]
    [SerializeField]
    private healtshadow sombra;
    private Vector3 originalPos;
    [SerializeField]
    private float vidaBase;
    private float anchoBase; 
    [SerializeField]
    private RectTransform rectTransform;



    void Start()
    {
        vida = vidaMax;
        stamina = staminaMax;
        originalPos = rectTransform.localPosition;
        anchoBase = rectTransform.rect.width;
    }


    void Update()
    {
        if (stamina / staminaMax > staminaRecoveryMargin)
        {
            sprintReady = true;
        }
        barradevida.fillAmount = vida / vidaMax;
        barradestamina.fillAmount = stamina / staminaMax;
        if (Input.GetKeyDown(KeyCode.R))
        {
            SumarVida(restaurarVida);
        }
        CambioStamina();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncrementoVida(aumentoVida);
        }
    }

    public void RestarVida(float daño)
    {
        
        vida -= daño;
        sombra.bajarVida(vida / vidaMax);
        if (vida < 0) vida = 0;

    }

    public void SumarVida(float restaurar)
    {
        vida += restaurar;
        sombra.SubirVida(vida / vidaMax);
        if (vida > vidaMax) vida = vidaMax;
    }
    public void CambioStamina()
    {

        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            corriendo = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            corriendo = false;
        }*/

        if (corriendo)
        {
            if (stamina > 0)
            {
                stamina -= CostoCorrer * Time.deltaTime;
                barradestamina.fillAmount = stamina / staminaMax;
            }

            if (stamina <= 0)
            {
                stamina = 0;
                sprintReady = false;
                if (recarga != null) StopCoroutine(recarga);
            }

        }

        if (!corriendo)
        {
            
           if (!corrutina)
            recarga = StartCoroutine(RecargaStam());
        }
    }   
    
    public IEnumerator RecargaStam()
    {
        corrutina = true;
        yield return new WaitForSeconds(1f);

        while (stamina < staminaMax && !corriendo)
        {
            
            stamina += recargaStarmina / 10f;
            if (stamina > staminaMax) stamina = staminaMax;
            barradestamina.fillAmount = stamina / staminaMax;
            yield return new WaitForSeconds(.02f);  
        }
        corrutina = false;
    }

    public void IncrementoVida(float cantidad)
    {
        vidaMax += cantidad;
        vida = vidaMax;
        IncrementoBarraVida();
    }

    public void IncrementoBarraVida() 
    {
        //Debug.Log("originalPos: " + originalPos + " basehp: " + basehp + " anchoBase: " + anchoBase);
        float newWidth = (vidaMax / vidaBase) * anchoBase;
        //Debug.Log("newWidth: " + newWidth);
        //Debug.Log("ecuacion: " + new Vector3((newWidth - anchoBase / 2), 0, 0));
        rectTransform.sizeDelta = new Vector2 (newWidth, rectTransform.sizeDelta.y);
        rectTransform.localPosition = originalPos + new Vector3((newWidth - anchoBase) / 2, 0 , 0);
        //rectTransform.rect.Set((newWidth - anchoBase /2) + originalPos.x, rectTransform.rect.y, newWidth, rectTransform.rect.height);
        //Debug.Log(rectTransform.position);Debug.Log(originalPos);
    }

    public bool EmpezarCorrer()
    {
        if (sprintReady)
        {
            corriendo= true;
        }


        return sprintReady;

    }

    public void DejarCorrer()
    {
        corriendo = false;
    }
}