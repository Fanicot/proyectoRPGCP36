using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbitCamara : MonoBehaviour
{
    private Vector2 angulo = new Vector2(90 * Mathf.Deg2Rad, 0f);
    [SerializeField]
    private Transform follow;
    [SerializeField] 
    private float distancia;
    [SerializeField]
    private float mCam;
    [SerializeField]
    private Transform jugador;
    [SerializeField]
    private float hor;
    [SerializeField]
    private float ver;
    void Update()
    {
        hor = Input.GetAxisRaw("Mouse X");
        ver = Input.GetAxisRaw("Mouse Y");

        if ( hor != 0)
        {
            angulo.x += hor * Mathf.Deg2Rad;
        }

        if ( ver != 0)
        {
            angulo.y += ver * Mathf.Deg2Rad;
            angulo.y = Mathf.Clamp(angulo.y, -35 * Mathf.Deg2Rad, 5 * Mathf.Rad2Deg);
        }
    }

    private void LateUpdate()
    {
        Vector3 orbita = new Vector3(
            Mathf.Cos(angulo.x) * Mathf.Cos(angulo.y),
            -Mathf.Sin(angulo.y),
            -Mathf.Sin(angulo.x) * Mathf.Cos(angulo.y));

        transform.position = follow.position + orbita * distancia;
        transform.rotation = Quaternion.LookRotation(follow.position - transform.position);
    }
}
