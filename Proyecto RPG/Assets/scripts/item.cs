using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class item : MonoBehaviour
{
    public int cantidad = 1;
    private Text textoCantidad;
    public int ID;
    public bool acumulable;
    public GameObject Descripcion_;
    public Text Nombre_;
    public Text Dato_;
    public DataBase DB;

    
    void Start()
    {
        acumulable = DB.baseDatos[ID].acumulable;
        Descripcion_ = InventarioJugador.Descripcion;
        Nombre_ = Descripcion_.transform.GetChild(0).GetComponent<Text>();
        Dato_ = Descripcion_.transform.GetChild(1).GetComponent<Text>();
        Descripcion_.SetActive(false);
        if (!Descripcion_.GetComponent<Image>().enabled)
        {
            Descripcion_.GetComponent <Image>().enabled = true;
            Nombre_.enabled = true;
            Dato_.enabled = true;
        }

    }

   
    void Update()
    {
        textoCantidad.text = cantidad.ToString();

        if(transform.parent == InventarioJugador.canvas)
        {
            Descripcion_.SetActive (false);
        }
    }
}