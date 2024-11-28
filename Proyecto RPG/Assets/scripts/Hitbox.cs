using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    public GameObject damager;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<damagable>() != null && damager.GetComponent<Damager>() != null)
        {
            other.GetComponent<damagable>().getDamaged(damager.GetComponent<Damager>().getDamage());
        }

        else
            Debug.Log("falta damager o damagable");
    }
}

public interface Damager
{
    public abstract int getDamage();
}

public interface damagable
{
    public abstract void getDamaged(int daño);
}
