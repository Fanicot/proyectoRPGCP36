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
    private Animator anim;
    

    [SerializeField]
    private float vida;
    private float daño = 10;
    private float rangoAtaque = 5;
    [SerializeField]
    private recursosPersonaje recursosJugador;

    void Start()
    {
        enemigo = GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();

        

        IrAlSiguientePunto();
    }

    void Update()
    {

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (jugadorEnRango)

            PerseguirJugador();
        

        else if (!enemigo.pathPending && enemigo.remainingDistance <= enemigo.stoppingDistance)
        {
            IrAlSiguientePunto();
        }

        if (distancia <= rangoAtaque)
        {
            anim.Play("Zombie Attack");
        }
    }

    public void IrAlSiguientePunto()
    {
        if (PuntosPatrulla.Length == 0) return;

        enemigo.destination = PuntosPatrulla[PuntoActual].position;

        PuntoActual = (PuntoActual + 1) % PuntosPatrulla.Length;

        anim.SetBool("seguir", false);
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
            enemigo.speed = 3.5f;
            IrAlSiguientePunto();
        }
    }

    public void PerseguirJugador()
    {
        persiguiendo = true;
        enemigo.destination = jugador.position;
        enemigo.speed = 6f;
        anim.SetBool("seguir", true);
    }

    public void RealizarDaño()
    {
        recursosJugador.RestarVida(daño);
    }

    void animAttack()
    {
        anim.SetBool("IsAttack", true);
    }
}
