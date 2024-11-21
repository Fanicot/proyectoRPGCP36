using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject inv;
    private bool invActivo = false;
    
    private void Update()
    {
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
        inv.SetActive(false);
        invActivo = true;
    }

    public void ShowInventary()
    {
        mainMenu.SetActive(false);
        inv.SetActive(true);
        invActivo = false;
    }
}
