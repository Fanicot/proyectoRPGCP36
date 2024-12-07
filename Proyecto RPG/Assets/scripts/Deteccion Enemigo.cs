using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeteccionEnemigo : MonoBehaviour
{
    public float rango;
    public float angulo;
    public float anguloOriginal;
    public string tags;
    public bool hayAlgo;
    public LayerMask layers;
    
    void Start()
    {
        anguloOriginal = angulo;
    }

   
    void Update()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, rango);
        List<Collider> targetFiltrado = new List<Collider>();
        foreach (Collider target in targets)
        {
            if (target.CompareTag(tags))
            {
                Vector3 puntocercano = target.ClosestPoint(transform.position);
                Vector3 dir = puntocercano - transform.position;
                dir = dir.normalized;
                float distan = Vector3.Distance(transform.position, puntocercano);
                if (!Physics.Raycast(transform.position, dir, distan, layers))
                {
                    //Debug.Log(Vector3.Angle(transform.forward, dir));
                    if (Vector3.Angle(transform.forward, dir) <= angulo)
                    {
                        
                        targetFiltrado.Add(target);
                    }
                }
            }
        }

        if (targetFiltrado.Count > 0)
        {
            hayAlgo = true;
        }
        else hayAlgo = false;
    }
}
