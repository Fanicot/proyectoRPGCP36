using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objeto : MonoBehaviour
{

    public int cantidad;
    public int ID;
    public InventarioJugador Inv;
    public bool entra = false;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Jugador"))
        {
            Inv.AgregarItem(ID, cantidad);
        }
    }

    
}