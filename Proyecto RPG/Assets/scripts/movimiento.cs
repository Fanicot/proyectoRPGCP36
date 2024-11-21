using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class movimiento : MonoBehaviour
{

    private CharacterController jugador;
    [SerializeField]
    private Vector3 camFoward;
    private Vector3 camRight;
    private Quaternion camRotation;
    private float gravedad = -9.8f;
    private float velocidadVertical = 0f;
    private float horizontal;
    private float vertical;
    private Vector3 jugadorInput;
    private Vector3 mov;
    [SerializeField]
    private float velocidad;
    public recursosPersonaje recursosPersonaje;
    [SerializeField]
    private float velocidadDash;
    [SerializeField]
    private float distanciaDash;
    private Vector3 direccionDash;
    private bool isDashing;
    private float dashtime;
    private Animator anim;
    [SerializeField]
    private LayerMask mask;
    private bool estaenSuelo;
    [SerializeField]
    private GameObject rayoSuelo;
    [SerializeField] 
    private float alturaSuelo;

    [SerializeField]
    private GameObject cofre;
    public AnimationCurve curva;

    void Start()
    {
        jugador = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();
    }

  
    void Update()
    {
        estaenSuelo = Physics.Raycast(rayoSuelo.transform.position, Vector3.down, alturaSuelo, mask);
        Debug.DrawLine(rayoSuelo.transform.position, transform.position + (Vector3.down * alturaSuelo), Color.red);

        float distanciaCofre = Vector3.Distance(transform.position, cofre.transform.position);

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jugadorInput = new Vector3(horizontal, 0, vertical).normalized;

        if (horizontal != 0 || vertical != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsSprinting", false);
            anim.SetBool("IsRunning", false);
        }

        if (isDashing)
        {
            jugadorInput = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (horizontal != 0 || vertical != 0)
            {
                if (recursosPersonaje.EmpezarCorrer())
                {
                    jugadorInput = jugadorInput * 1.5f;
                    anim.SetBool("IsSprinting", true);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            recursosPersonaje.DejarCorrer();
            anim.SetBool("IsSprinting" , false);
        }

        camFoward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;
        camRotation = Camera.main.transform.rotation;

        camFoward.y = 0;
        camRight.y = 0;
        camRotation.z = 0;
        camRotation.x = 0;

        camFoward = camFoward.normalized;
        camRight = camRight.normalized;

        mov = jugadorInput.x * camRight * velocidad  + jugadorInput.z * camFoward * velocidad ;
        
        if (!jugador.isGrounded)
        {
            velocidadVertical += gravedad * Time.deltaTime;
        }
        else 
            velocidadVertical = 0;

        mov.y += velocidadVertical;
        
        if (isDashing)
        {
            float speedAux = velocidadDash;
            dashtime -= Time.deltaTime;
            if ( dashtime < 0)
            {
                float diff = Time.deltaTime + dashtime;
                float speedFactor = diff / Time.deltaTime;
                speedAux *= speedFactor;
            }
            mov += direccionDash * speedAux;
            if (dashtime <= 0)
            isDashing = false;
        }

       

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            direccionDash = new Vector3 (0, 0, 0);
            isDashing = true;
            if (Input.GetKey(KeyCode.W)) 
                direccionDash += transform.forward;
            if ( Input.GetKey(KeyCode.S))
                direccionDash += -transform.forward;
            if (Input.GetKey(KeyCode.A))
                direccionDash += -transform.right;
            if (Input.GetKey(KeyCode.D))
                direccionDash += transform.right;

            dashtime = DashTime(velocidadDash, distanciaDash);
        }

        jugador.Move(mov * Time.deltaTime);
        jugador.transform.rotation = camRotation;


        if (Input.GetKeyDown(KeyCode.G) && distanciaCofre < 5f)
        {
            Interpolacion(transform.position, cofre.transform.position, 2f, transform.gameObject);
        }
    }

    public float DashTime(float velocidad, float distancia)
    {
        float tiempo = distancia / velocidad;

        return tiempo;
    }

    public IEnumerator LerpInterp(Vector3 posIni, Vector3 PosFin, float timpoMov, GameObject cofre)
    {
        float timePassed = 0f;

        Debug.Log(timePassed / timpoMov);

        while (timePassed < timpoMov)
        {
            cofre.transform.position = Vector3.Lerp(posIni, PosFin,
                curva.Evaluate(timePassed / timpoMov));

            timePassed += Time.deltaTime;

            yield return null;
        }
    }

    public void Interpolacion(Vector3 posIni, Vector3 PosFin, float timpoMov, GameObject cofre)
    {
        StartCoroutine(LerpInterp(posIni, PosFin, timpoMov, cofre));
    }
}
