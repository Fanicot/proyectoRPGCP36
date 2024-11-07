using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuracionJugador : MonoBehaviour
{
    public recursosPersonaje recursosPersonaje;
    public float cantidadcuracion = 20;
    private int frascosMax = 3;
    [SerializeField]
    private int frascosActuales;
    [SerializeField]
    private float tiempoCuracion;
    void Start()
    {
        frascosActuales = frascosMax;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && frascosActuales > 0 && recursosPersonaje.vida < recursosPersonaje.vidaMax)
        {
            StartCoroutine(Curacion(recursosPersonaje.vida, cantidadcuracion, tiempoCuracion));
            frascosActuales--;
        }
            
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
        Curacion = recursosPersonaje.vida + cantidadcuracion;

        while (tiempoTranscurrido < tiempoCuracion)
        {
            recursosPersonaje.vida = Mathf.Lerp(vidaActual, Curacion, tiempoTranscurrido / tiempoCuracion);
            tiempoTranscurrido += Time.deltaTime;
            if (recursosPersonaje.vida > recursosPersonaje.vidaMax) recursosPersonaje.vida = recursosPersonaje.vidaMax;
            yield return null;
        }
    }
}
