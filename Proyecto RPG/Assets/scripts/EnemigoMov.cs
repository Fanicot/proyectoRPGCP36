using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoMov : MonoBehaviour
{
    [SerializeField]
    private Transform[] PuntosPatrulla;
    [SerializeField]
    private int PuntoActual = 0;
    private NavMeshAgent enemigo;
    [SerializeField]
    private Transform jugador;
    [SerializeField]
    private bool persiguiendo = false;
    [SerializeField]
    private bool jugadorEnRango = false;


    void Start()
    {
        enemigo = GetComponent<NavMeshAgent>();
        enemigo.autoBraking = false;

        IrAlSiguientePunto();
    }

    void Update()
    {
        if (jugadorEnRango)
            PerseguirJugador();

        if (!enemigo.pathPending && enemigo.remainingDistance < 0.5f)
        {
            IrAlSiguientePunto();
        }
    }

    public void IrAlSiguientePunto()
    {
        if (PuntosPatrulla.Length == 0) return;

        enemigo.destination = PuntosPatrulla[PuntoActual].position;

        PuntoActual = (PuntoActual + 1) % PuntosPatrulla.Length;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            jugadorEnRango = false;
            persiguiendo = false;
            IrAlSiguientePunto();
        }
    }

    public void PerseguirJugador()
    {
        persiguiendo = true;
        enemigo.destination = jugador.position;
    }
}
