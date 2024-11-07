using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objeto : MonoBehaviour
{
    public InventarioJugador InventarioJugador;
    public enum TipoObjeto { FrascoCuracion, Otro }
    public TipoObjeto tipoDeObjeto;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            if (InventarioJugador.EnInventario == false)
            {
                gameObject.SetActive(false);
                InventarioJugador.EnInventario = true;
            }

            if (tipoDeObjeto == TipoObjeto.FrascoCuracion)
            {
                CuracionJugador curacionJugador = other.GetComponent<CuracionJugador>();
                if (curacionJugador != null)
                {
                    curacionJugador.AumentarCura();
                }
            }
        }
    }
}