using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemigoMov : MonoBehaviour, Damager, damagable
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
    private float iniciallAngularSpeed = 160f;
    private float stunInicio;
    public float stunDuracion;

    [SerializeField]
    private float vida;
    public int daño = 10;
    private float rangoAtaque = 1.5f;
    [SerializeField]
    private recursosPersonaje recursosJugador;

    public DeteccionEnemigo DeteccionEnemigo;
    
    public enum EstadoEnemigo { Patrulla, Perseguir, DetenidoParaAtacar, Atacando, Retroceder, Stun }
    private EstadoEnemigo estadoActual = EstadoEnemigo.Patrulla;

    
    private float tiempoDetenidoAntesdeAtacar = 1.5f;
    private float tiempoEsperaProximoAtaque = 2f;
    private Vector3 posicionRetroceso;
    private bool startingAtk;

    public TextMeshProUGUI prueba;
    void Start()
    {
        enemigo = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        IrAlSiguientePunto();
    }

    void Update()
    {

        prueba.text = estadoActual.ToString();
        float distancia = Vector3.Distance(transform.position, jugador.position);
        if (DeteccionEnemigo.hayAlgo && estadoActual == EstadoEnemigo.Patrulla)
        {
            jugadorEnRango = true;
            estadoActual = EstadoEnemigo.Perseguir;
            anim.SetBool("seguir", true);
            DeteccionEnemigo.angulo = 360f;
        }
        else if(!DeteccionEnemigo.hayAlgo && estadoActual != EstadoEnemigo.Patrulla && estadoActual != EstadoEnemigo.Stun)
        {
            jugadorEnRango = false;
            estadoActual = EstadoEnemigo.Patrulla;
            anim.SetBool("seguir", false);
            enemigo.speed = 2.5f;
            
            DeteccionEnemigo.angulo = DeteccionEnemigo.anguloOriginal;
        }

        if (vida <= 0)
        {
            enemigo.isStopped = true;
            anim.SetBool("derrotado", true);
        }

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
                    enemigo.speed = 5f; 
                    anim.SetBool("seguir", true);
                }
                break;

            case EstadoEnemigo.DetenidoParaAtacar:
                break;

            case EstadoEnemigo.Atacando:
                if (vida > 0) 
                    anim.Play("Zombie Attack");
                posicionRetroceso = transform.position - (jugador.position - transform.position).normalized * 3.5f;
                if (startingAtk)
                {
                    Invoke("IniciarRetroceso", 1.8f);
                    startingAtk = false;
                }
                    
                break;

            case EstadoEnemigo.Retroceder:
                enemigo.destination = posicionRetroceso;
                EsperarProximoAtaqueAux();
                break;

            case EstadoEnemigo.Stun:
                Debug.Log(stunInicio);
                if(Time.time > stunInicio + stunDuracion)
                {
                    estadoActual = EstadoEnemigo.Perseguir;
                    enemigo.isStopped = false;
                }
                
                break;
        }
    }

    private void DetenerAntesDeAtacar()
    {
        estadoActual = EstadoEnemigo.Atacando;
        startingAtk = true;
    }

    private void IniciarRetroceso()
    {
        enemigo.angularSpeed = 0;
        estadoActual = EstadoEnemigo.Retroceder;
        enemigo.isStopped = false;
    }

    public void EsperarProximoAtaqueAux() 
    {
        Debug.Log("se entra");
        if (Vector3.Distance(transform.position, posicionRetroceso) <= enemigo.stoppingDistance && !enemigo.isStopped)
        {
            enemigo.angularSpeed = iniciallAngularSpeed;
            Debug.Log("le entra");
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

    public int getDamage()
    {
        return daño;
    }

    public void getDamaged(int daño)
    {
        vida -= daño;
        anim.SetTrigger("stun");
        cambiarStun();
    }

    public void cambiarStun()
    {
        enemigo.isStopped = true;
        stunInicio = Time.time;
        estadoActual = EstadoEnemigo.Stun;
        CancelInvoke();
    }

    public void destoy()
    {
        Destroy(gameObject);
    }
}
