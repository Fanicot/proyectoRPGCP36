using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class recursosPersonaje : MonoBehaviour
{
    [Header("Vida, Stamina")]
    public float vida;
    public float vidaMax = 100;
    [SerializeField]
    private float aumentoVida;
    [SerializeField]
    private float stamina;
    public float staminaMax = 100;
    [SerializeField]
    private Image barradevida, barradestamina;

    [Header("Curacion")]
    public float cantidadcuracion = 20;
    private int frascosMax = 3;
    [SerializeField]
    private int frascosActuales;
    [SerializeField]
    private float tiempoCuracion;

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
        frascosActuales = frascosMax;
    }


    void Update()
    {
        if (stamina / staminaMax > staminaRecoveryMargin)
        {
            sprintReady = true;
        }
        barradevida.fillAmount = vida / vidaMax;
        barradestamina.fillAmount = stamina / staminaMax;
        CambioStamina();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncrementoVida(aumentoVida);
        }

        if (Input.GetKeyDown(KeyCode.R) && frascosActuales > 0 && vida < vidaMax)
        {
            StartCoroutine(Curacion(vida, cantidadcuracion, tiempoCuracion));
            frascosActuales--;
        }
    }

    #region stamina
    public void CambioStamina()
    {
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
    public bool EmpezarCorrer()
    {
        if (sprintReady)
        {
            corriendo = true;
        }


        return sprintReady;

    }

    public void DejarCorrer()
    {
        corriendo = false;
    }

    #endregion stamina
    #region vida
    public void RestarVida(float daño)
    {
        vida -= daño;
        sombra.bajarVida(vida / vidaMax);
        if (vida < 0) vida = 0;
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

    public void RecargarCuras()
    {
        frascosActuales = frascosMax;
    }

    public int ObtenerFrascosActuales()
    {
        return frascosActuales;
    }
    public void AumentarCura()
    {
        frascosActuales++;
        frascosMax++;
    }

    public IEnumerator Curacion(float vidaActual, float Curacion, float tiempoCuracion)
    {
        float tiempoTranscurrido = 0f;
        Curacion = vida + cantidadcuracion;

        while (tiempoTranscurrido < tiempoCuracion)
        {
            vida = Mathf.Lerp(vidaActual, Curacion, tiempoTranscurrido / tiempoCuracion);
            tiempoTranscurrido += Time.deltaTime;
            if (vida > vidaMax) vida = vidaMax;
            yield return null;
        }
    }
#endregion
}    