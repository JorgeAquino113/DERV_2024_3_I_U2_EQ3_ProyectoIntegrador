using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pablo_Controller : MonoBehaviour
{

    // Variables de movimientos
    [SerializeField] private float velocidad; // Velocidad al caminar
    [SerializeField] private float velocidadCarrera; // Velocidad al correr
    [SerializeField] private float aceleracion; // Aceleración
    [SerializeField] private float desaceleracion; // Desaceleración
    [SerializeField] private int fuerzaSalto; // Fuerza de salto

    // Variables de estado
    private bool IsJumping = false; // Estado de salto
    private bool IsAlive = true; // Estado de vida
    private bool IsWalking = false; // Estado de caminar
    private bool IsRunning = false; // Estado de correr
    private bool IsCrouching = false; // Estado de agacharse

    // Componentes
    private new Rigidbody rigidbody; // Componente Rigidbody
    private Animator animator; // Componente Animator

    // Gameobjects
    [SerializeField] private Transform camara; // Referencia a la cámara

    // Detección de doble clic
    private float tiempoDobleClick = 0.2f; // Tiempo máximo entre clics para considerar como doble clic
    private float tiempoUltimoClick = 0f; // Tiempo del último clic

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (Time.time - tiempoUltimoClick < tiempoDobleClick)
                    {
                        IsRunning = !IsRunning;
                    }
                    tiempoUltimoClick = Time.time;
                }
            }
            else // Si no se está presionando W, dejar de correr
            {
                IsRunning = false;
                IsWalking = false;
            }

            // Actualizar los estados en el Animator
            animator.SetBool("IsRunning", IsRunning);
            animator.SetBool("IsWalking", ver != 0 || hor != 0); // Animación de caminar si hay movimiento

            // Determinar velocidad de movimiento
            float velocidadObjetivo = IsRunning ? velocidadCarrera : velocidad;

            // Movimiento del jugador
            Vector3 direccionCamara = camara.forward;
            direccionCamara.y = 0f;
            Vector3 movimiento = (camara.right * hor + direccionCamara * ver).normalized * velocidadObjetivo;

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

            // Manejar el estado de agacharse
            if (Input.GetKey(KeyCode.LeftControl))
            {
                IsCrouching = true;
            }
            else
            {
                IsCrouching = false;
            }
            animator.SetBool("IsCrouching", IsCrouching);

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Resetear el estado de salto al colisionar con el suelo
        IsJumping = false;
        animator.SetBool("IsJumping", IsJumping); // Desactivar la animación de salto
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
