using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    [SerializeField]
    private GameObject ObjetoaEquipar;
    [SerializeField]
    private bool Equipado = false;
    
    public bool EnInventario = false;

    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && EnInventario)
        {
           ObjetoaEquipar.SetActive(!Equipado);
            Equipado = !Equipado;
        }
    }
}
