using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pablo_FutbolController : MonoBehaviour
{
    // Variables de movimientos
    [SerializeField] private float velocidad; // Velocidad al caminar
    [SerializeField] private float velocidadCarrera; // Velocidad al correr
    [SerializeField] private float aceleracion; // Aceleración
    [SerializeField] private float desaceleracion; // Desaceleración
    [SerializeField] private int fuerzaSalto; // Fuerza de salto
    [SerializeField] private float fuerzaPatearNormal; // Fuerza de patear
    [SerializeField] private float fuerzaToque; // Fuerza de toque
    [SerializeField] private float fuerzaZetSen; // Fuerza de patear
    [SerializeField] private float distanciaPatear; // Tiempo de patear

    // Variables de estado
    private bool IsJumping = false; // Estado de salto
    private bool IsAlive = true; // Estado de vida
    private bool IsWalking = false; // Estado de caminar
    private bool IsRunning = false; // Estado de correr
    public bool IsKicking = false; // Estado de patear
    public bool isAtackPosition = false; // Estado de posición de ataque
    public bool IsTouchingBall = false; // Estado de tocar el balón
    //public bool IsHavingBall = false; // Estado de disparar
    public bool HavingDisparoZetSen = false; // Estado de disparar

    // Componentes
    private new Rigidbody rigidbody; // Componente Rigidbody
    private Animator animator; // Componente Animator

    // Gameobjects
    [SerializeField] private GameObject camara; // Referencia a la cámara
    private Pablo_CameraController camaraController; // Referencia al script de la cámara

    [SerializeField] private Camera camaraDisparo; // Referencia a la portería
    [SerializeField] private Transform porteria; // Referencia a la portería
    private Transform balon; // Referencia al balón
    [SerializeField] private Transform balonPosicionATaque;
    Balon_Controller balonController;

    // Detección de doble clic
    private float tiempoDobleClick = 0.2f; // Tiempo máximo entre clics para considerar como doble clic
    private float tiempoUltimoClick = 0f; // Tiempo del último clic

    Rayo rayoController;

    void Awake()
    {
        balon = GameObject.FindGameObjectWithTag("Balon").GetComponent<Transform>();
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        camaraController = camara.GetComponent<Pablo_CameraController>();
        balonController = balon.GetComponent<Balon_Controller>();
        rayoController = GetComponent<Rayo>();
    }

    void Update()
    {
        if (IsAlive)
        {
            // Manejar salto
            if (Input.GetKeyDown(KeyCode.Space) && !IsJumping)
            {
                rigidbody.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
                IsJumping = true;
                animator.SetBool("IsJumping", true); // Activar la animación de salto
            }
            // Manejar movimiento
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");

            // Comprobar si se presiona "W" para correr
            if (ver > 0)
            {
                // Detectar doble clic
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
                {
                    if (Time.time - tiempoUltimoClick < tiempoDobleClick)
                    {
                        IsRunning = !IsRunning;
                    }
                    tiempoUltimoClick = Time.time;
                }
            }
            // if (Input.GetKey(KeyCode.LeftControl))
            // {
            //     IsRunning = true;
            // }
            // else
            // {
            //     IsRunning = false;
            // }
            float velocidadObjetivo = IsRunning ? velocidadCarrera : velocidad;
            Vector3 direccionCamara = camara.transform.forward;
            direccionCamara.y = 0f;
            Vector3 movimiento = (camara.transform.right * hor + direccionCamara * ver).normalized * velocidadObjetivo;

            // Aplicar suavizado (aceleración y desaceleración)
            if (movimiento != Vector3.zero)
            {
                // Aceleración
                Vector3 nuevaVelocidad = Vector3.Lerp(rigidbody.velocity, movimiento, aceleracion * Time.deltaTime);
                nuevaVelocidad.y = rigidbody.velocity.y;
                rigidbody.velocity = nuevaVelocidad; // Actualizar la velocidad del Rigidbody

                // Rotar el personaje hacia la dirección de movimiento
                Quaternion rotacionObjetivo = Quaternion.LookRotation(movimiento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
            }
            else
            {
                // Desaceleración si no hay movimiento
                Vector3 nuevaVelocidad = Vector3.Lerp(rigidbody.velocity, Vector3.zero, desaceleracion * Time.deltaTime);
                nuevaVelocidad.y = rigidbody.velocity.y;
                rigidbody.velocity = nuevaVelocidad;
            }
            animator.SetBool("IsRunning", IsRunning && (ver != 0 || hor != 0));
            animator.SetBool("IsWalking", ver != 0 || hor != 0);

            mecanicaBalon();

        }
    }

    private IEnumerator Kick()
    {
        IsKicking = true;
        animator.SetBool("Iskicking", IsKicking);
        yield return new WaitForSeconds(0.2f);
        IsKicking = false;
        animator.SetBool("Iskicking", IsKicking);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Resetear el estado de salto al colisionar con el suelo
        IsJumping = false;
        animator.SetBool("IsJumping", IsJumping);

    }


    private void mecanicaBalon()
    {
        Vector3 direccionHaciaBalon = (balon.position - transform.position).normalized;
        float anguloConBalon = Vector3.Angle(transform.forward, direccionHaciaBalon);
        RaycastHit hit;
        if (anguloConBalon < 90f)
        {
            if (Physics.Raycast(transform.position, direccionHaciaBalon, out hit, distanciaPatear))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
                if (hit.collider.CompareTag("Balon"))
                {
                    IsTouchingBall = true;
                }
                if (balonController.jugadorHavingBall)
                {
                    balon.position = balonPosicionATaque.position;
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * distanciaPatear, Color.red);
                IsTouchingBall = false;
                balonController.soltarBalonPlayer();
            }


            if (Input.GetKeyDown(KeyCode.E))
            {
                if (balonController.GummingoHavingBall)
                {
                    balonController.robarBalon(transform.forward.normalized);
                }
                else if (IsTouchingBall)
                {
                    tomarBalon();
                }

            }

            if (Input.GetMouseButton(0) && balonController.jugadorHavingBall && !IsKicking)
            {
                disparo();
            }
        }

        disparoZetSen();
    }
    void tomarBalon()
    {
        balon.position = balonPosicionATaque.position;
        balonController.TomarBalonJugador();
    }

    void disparo()
    {
        StartCoroutine(Kick());
        Vector3 direccionPateo = transform.forward.normalized;
        balon.GetComponent<Rigidbody>().AddForce(direccionPateo * fuerzaPatearNormal, ForceMode.Impulse);
        balonController.soltarBalonPlayer();

    }
    void disparoZetSen()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isAtackPosition == false && balonController.jugadorHavingBall)
        {
            isAtackPosition = true;
            camaraController.cameraActive(false);
            camaraDisparo.gameObject.SetActive(true);
            rayoController.Activar();
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false); // Animación de caminar si hay movimiento
        }
        else if (isAtackPosition && Input.GetMouseButtonDown(0))
        {
            rayoController.Desactivar();
            Vector3 mousePosition = Input.mousePosition;
            float x = (mousePosition.x / Screen.width) * 2 - 1;
            float y = (mousePosition.y / Screen.height) * 2 - 1;
            Vector3 direccionPateo = new Vector3(x, y, 1).normalized;
            direccionPateo = camaraDisparo.transform.TransformDirection(direccionPateo);
            balon.GetComponent<Rigidbody>().AddForce(direccionPateo * fuerzaZetSen, ForceMode.Impulse);
            StartCoroutine(Kick());

            isAtackPosition = false;
            camaraController.cameraActive(true);
            camaraDisparo.gameObject.SetActive(false);
            balonController.dispararRayo();
            balonController.soltarBalonPlayer();
        }
        else if (isAtackPosition)
        {
            transform.LookAt(porteria);
            camaraDisparo.transform.LookAt(porteria);
            balon.position = balonPosicionATaque.position;
            balon.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if (balonController.jugadorHavingBall == false)
        {
            rayoController.Desactivar();
            isAtackPosition = false;
            camaraController.cameraActive(true);
            camaraDisparo.gameObject.SetActive(false);
        }
    }


    public void matarPedro()
    {
        IsAlive = false; // Cambiar el estado a muerto
        animator.SetBool("IsAlive", IsAlive); // Activar la animación de muerte

    }

    public void revivirPedro()
    {
        IsAlive = true; // Cambiar el estado a vivo
        animator.SetBool("IsAlive", IsAlive); // Desactivar la animación de muerte
    }
}