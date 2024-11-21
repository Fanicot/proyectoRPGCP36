using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackJugador : MonoBehaviour
{
    private Animator anim;
    public int estado = 0;
    private bool atkReady = false;
    public GameObject[] armas;
    private bool espada = false;
    private bool hacha = false;
    private bool espadaLarga = false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (estado == 0)
        {
            armas[0].SetActive(false);
            armas[1].SetActive(false);
            armas[2].SetActive(false);
        }

        else if (estado == 1)
        {
            armas[0].SetActive(true);
            armas[1].SetActive(false);
            armas[2].SetActive(false);
        }

        else if (estado == 2)
        {
            armas[0].SetActive(false);
            armas[1].SetActive(true);
            armas[2].SetActive(false);
        }

        else if (estado == 3)
        {
            armas[0].SetActive(false);
            armas[1].SetActive(false);
            armas[2].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !atkReady)
        {
            if (estado == 0)
            {
                anim.SetTrigger("AtkPunch");
                atkReady = true;
            }

            else if (estado == 1 && espada == true)
            {
                anim.SetTrigger("AtkSword");
                atkReady = true;
            }

            else if (estado == 2 && espadaLarga == true)
            {
                anim.SetTrigger("AtkSword");
                atkReady = true;
            }

            else if (estado == 3 && hacha == true)
            {
                anim.SetTrigger("AtkSword");
                atkReady = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "espada" && Input.GetKeyDown(KeyCode.E))
        {
            estado = 1;
            espada = true;
            other.gameObject.SetActive(false);
        }

        if (other.tag == "espadaLarga" && Input.GetKeyDown(KeyCode.E))
        {
            estado = 2;
            espadaLarga = true;
            other.gameObject.SetActive(false);
        }

        if (other.tag == "hacha" && Input.GetKeyDown(KeyCode.E))
        {   
            estado = 3;
            hacha = true;
            other.gameObject.SetActive(false);
        }
    }
    public void AtkReady()
    {
        atkReady = false;
    }

}
