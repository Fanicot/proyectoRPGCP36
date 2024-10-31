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
    private bool jugadorEnRango = false;
    private Animator anim;

    [SerializeField]
    private float vida;
    private float da�o = 10;
    private float rangoAtaque = 2f;
    [SerializeField]
    private recursosPersonaje recursosJugador;

    public enum EstadoEnemigo { Patrulla, Perseguir, DetenidoParaAtacar, Atacando, Retroceder }
    private EstadoEnemigo estadoActual = EstadoEnemigo.Patrulla;

    
    private float tiempoDetenidoAntesdeAtacar = 1.5f;
    private float tiempoEsperaProximoAtaque = 2f;
    private Vector3 posicionRetroceso;

    void Start()
    {
        enemigo = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        IrAlSiguientePunto();
    }

    void Update()
    {
        float distancia = Vector3.Distance(transform.position, jugador.position);

        switch (estadoActual)
        {
            case EstadoEnemigo.Patrulla:
                if (jugadorEnRango)
                {
                    estadoActual = EstadoEnemigo.Perseguir;
                    anim.SetBool("seguir", true);
                }
                else if (!enemigo.pathPending && enemigo.remainingDistance <= enemigo.stoppingDistance)
                    IrAlSiguientePunto();
                break;

            case EstadoEnemigo.Perseguir:
                if (distancia <= rangoAtaque)
                {
                    estadoActual = EstadoEnemigo.DetenidoParaAtacar;
                    enemigo.isStopped = true;
                    anim.SetBool("seguir", false);
                    Invoke("DetenerAntesDeAtacar", tiempoDetenidoAntesdeAtacar);
                }
                else
                {
                    enemigo.destination = jugador.position;
                    anim.SetBool("seguir", true);
                }
                break;

            case EstadoEnemigo.DetenidoParaAtacar:
                break;

            case EstadoEnemigo.Atacando:
                anim.Play("Zombie Attack");
                posicionRetroceso = transform.position - (jugador.position - transform.position).normalized * 3.5f;
                Invoke("IniciarRetroceso", 1.3f);
                break;

            case EstadoEnemigo.Retroceder:
                enemigo.destination = posicionRetroceso;
                EsperarProximoAtaqueAux();
                break;
        }
    }

    private void DetenerAntesDeAtacar()
    {
        estadoActual = EstadoEnemigo.Atacando;
    }

    private void IniciarRetroceso()
    {
        estadoActual = EstadoEnemigo.Retroceder;
        enemigo.isStopped = false;
    }

    public void EsperarProximoAtaqueAux() 
    {
        if (Vector3.Distance(transform.position, posicionRetroceso) >= 3.5f && !enemigo.isStopped)
        {
            Invoke("EsperarProximoAtaque", tiempoEsperaProximoAtaque);
            enemigo.isStopped = true;
        }
    }

    private void EsperarProximoAtaque()
    {
      
       
        
        if (jugadorEnRango)
        {
            estadoActual = EstadoEnemigo.Perseguir;
            enemigo.isStopped = false;
        }
        
        else
        {
            estadoActual = EstadoEnemigo.Patrulla;
            IrAlSiguientePunto();
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
            estadoActual = EstadoEnemigo.Perseguir;
            anim.SetBool("seguir", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            jugadorEnRango = false;
            estadoActual = EstadoEnemigo.Patrulla;
            anim.SetBool("seguir", false);
            enemigo.speed = 3.5f;
            IrAlSiguientePunto();
        }
    }

    public void RealizarDa�o()
    {
        recursosJugador.RestarVida(da�o);
    }
}
