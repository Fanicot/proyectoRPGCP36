using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objeto : MonoBehaviour
{
    public InventarioJugador InventarioJugador;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisi�n detectada con: " + other.gameObject.name);
        if (other.CompareTag("Jugador"))
        {
            Debug.Log("entro");
            if (InventarioJugador.EnInventario == false)
            {
                gameObject.SetActive(false);
                InventarioJugador.EnInventario = true;
            }
        }
        
    }
}
