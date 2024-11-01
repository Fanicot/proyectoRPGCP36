using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objeto : MonoBehaviour
{
    public InventarioJugador InventarioJugador;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            if (InventarioJugador.EnInventario == false)
            {
                gameObject.SetActive(false);
                InventarioJugador.EnInventario = true;
            }
        }
        
    }
}
