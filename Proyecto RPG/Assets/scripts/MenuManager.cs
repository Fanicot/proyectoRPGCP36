using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject stats;
    private bool invActivo = false;

    public recursosPersonaje recursosPersonaje;
    private float stamina;
    private float vida;
    public TextMeshProUGUI staminaView;
    public TextMeshProUGUI vidaView;
    //public TextMeshProUGUI nivelView;
    //public TextMeshProUGUI manaView;
    //public TextMeshProUGUI dañoView;

    private void Update()
    {
        vida = recursosPersonaje.vidaMax;
        stamina = recursosPersonaje.staminaMax;
        vidaView.text = vida.ToString();
        staminaView.text = stamina.ToString();

        if (invActivo)
            mainMenu.SetActive(true);
        else 
            mainMenu.SetActive(false);

        if (Input.GetKeyDown(KeyCode.I))
        {
            invActivo = !invActivo;
        }
    }
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        stats.SetActive(false);
        invActivo = true;
    }

    public void ShowStats()
    {
        mainMenu.SetActive(false);
        stats.SetActive(true);
        invActivo = false;
    }
}
