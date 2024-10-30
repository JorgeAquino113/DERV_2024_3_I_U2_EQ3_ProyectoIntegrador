using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gummingo_Controller : MonoBehaviour
{
    [SerializeField] private float velocidad; // Velocidad al caminar
    [SerializeField] private float velocidadCarrera; // Velocidad al correr
    [SerializeField] private float distanciaBalon = 1.5f; // Distancia del balón al dribblear
    [SerializeField] private float fuerzaPase; // Fuerza del pase
    [SerializeField] private float fuerzaDisparo; // Fuerza del disparo
    [SerializeField] private float distanciaControl = 2.0f; // Distancia máxima para controlar el balón
    [SerializeField] private Transform posicionBalon; // Posición para dribblear con el balón
    [SerializeField] private GameObject porteriaRival; // Portería donde disparar

    private new Rigidbody rigidbody;
    private Animator animator;
    private Rigidbody balonRigidbody;
    private GameObject balon;
    private Balon_Controller balon_Controller;
    private GameObject player;

    private bool IsTouchingBall = false;
    private bool tomarTiempo = false;

    private bool IsWalking = false;
    private bool IsKicking = false;
    private bool IsAlive = true;

    void Awake()
    {
        balon = GameObject.FindGameObjectWithTag("Balon");
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        balonRigidbody = balon.GetComponent<Rigidbody>();
        balon_Controller = balon.GetComponent<Balon_Controller>();
    }

    void Update()
    {
        Vector3 direccionHaciaBalon = (balon.transform.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direccionHaciaBalon, out hit, distanciaBalon))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            if (hit.collider.CompareTag("Balon"))
            {
                IsTouchingBall = true;
                balon_Controller.TomarBalonGummingo(posicionBalon);
            }
            if (balon_Controller.GummingoHavingBall)
            {
                posicionarBalon();
                PerseguirPorteria();
            }
            else
            {
                IsWalking = false;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * distanciaBalon, Color.red);
            IsTouchingBall = false;
            PerseguirBalon(direccionHaciaBalon);
            balon_Controller.soltarBalonGummingo();
        }
        rotarGummingo(direccionHaciaBalon);
        animator.SetBool("IsWalking", IsWalking);
    }

    private void posicionarBalon()
    {
        if (balon_Controller.GummingoHavingBall)
        {
            balon.transform.position = posicionBalon.position;
        }
    }
    private void PerseguirBalon(Vector3 direccion)
    {
        IsWalking = true;
        rigidbody.MovePosition(transform.position + direccion * velocidad * Time.deltaTime);

        // Limitar la dirección solo al plano XZ
        direccion.y = 0f; // Asegurarse de que no haya rotación en el eje Y (vertical)
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

        // Aplicar la rotación en el plano Y (horizontal)
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
    }

    private void PerseguirPorteria()
    {
        IsWalking = true;

        // Obtener la dirección hacia la portería, pero limitándola al plano XZ
        Vector3 direccion = (porteriaRival.transform.position - transform.position).normalized;
        direccion.y = 0f; // Ignorar la componente vertical

        Quaternion rotacionPorteria = Quaternion.LookRotation(direccion);

        // Mover hacia la portería
        rigidbody.MovePosition(transform.position + direccion * velocidad * Time.deltaTime);

        // Rotación suave en el plano horizontal (solo en el eje Y)
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionPorteria, Time.deltaTime * 10f);
    }

    private void rotarGummingo(Vector3 direccion)
    {
        direccion.y = 0;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
    }

    private IEnumerator TomarBalon()
    {
        balon_Controller.TomarBalonGummingo(posicionBalon);
        posicionarBalon();
        yield return new WaitForSeconds(2f);
        tomarTiempo = false;
    }


}
