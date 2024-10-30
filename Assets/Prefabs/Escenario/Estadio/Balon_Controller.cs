using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Balon_Controller : MonoBehaviour
{
    public bool jugadorHavingBall = false;
    public bool GummingoHavingBall = false;
    private GameObject jugador;
    private GameObject gummingo;
    private new Rigidbody rigidbody;
    private bool retardoTiempo = true;

    // Variables de rasho laser

    [SerializeField] private GameObject rayo;
    [SerializeField] public Transform incioRayo;
    [SerializeField] private Transform posicionBalonPlayer;
    [SerializeField] private Transform posicionBalonGummingo;
    [SerializeField] private float tiempoRayo = 1f;
    [SerializeField] private new Camera camera;
    private GameObject Instance;
    public float MaxLength;

    private Hovl_Laser LaserScript;
    private Hovl_Laser2 LaserScript2;

    public bool ActiveRayo = false;
    public bool isCargando = false;
    void Awake()
    {
        jugador = GameObject.Find("PabloFutbol");
        gummingo = GameObject.Find("Gummingo");
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (ActiveRayo && isCargando == false && incioRayo != null)
        {
            if (Instance != null)
            {
                Instance.transform.position = incioRayo.transform.position;
                Instance.transform.LookAt(transform); // Apunta al finRayo
            }

            RaycastHit hit;
            Vector3 direccion = (transform.position - incioRayo.transform.position).normalized;
            if (Physics.Raycast(incioRayo.transform.position, direccion, out hit, MaxLength))
            {
                direccion = (hit.point - incioRayo.transform.position).normalized;
                Instance.transform.forward = direccion;
            }
            else
            {
                Instance.transform.forward = direccion;
            }
        }
        if (ActiveRayo == false)
        {
            destruirRayo();
        }
    }

    public void TomarBalonJugador()
    {
        if (retardoTiempo)
        {
            transform.SetParent(jugador.transform);
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.rotation = Quaternion.identity;
            rigidbody.angularVelocity = Vector3.zero;
            jugadorHavingBall = true;
            GummingoHavingBall = false;
            posicionarRayo();
        }
    }

    void posicionarRayo()
    {
        if (jugadorHavingBall)
        {
            incioRayo = posicionBalonPlayer;
        }
        else if (GummingoHavingBall)
        {
            incioRayo = posicionBalonGummingo;
        }
        else
        {
            incioRayo = null;
        }
    }

    public void TomarBalonGummingo(Transform posicionRayo)
    {
        if (retardoTiempo)
        {
            transform.SetParent(gummingo.transform);
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.rotation = Quaternion.identity;
            rigidbody.angularVelocity = Vector3.zero;
            jugadorHavingBall = false;
            GummingoHavingBall = true;
            posicionarRayo();
        }
    }


    public void soltarBalonPlayer()
    {
        transform.SetParent(null);
        rigidbody.useGravity = true;
        jugadorHavingBall = false;
    }

    public void soltarBalonGummingo()
    {
        transform.SetParent(null);
        rigidbody.useGravity = true;
        GummingoHavingBall = false;
    }

    public void robarBalon(Vector3 direccion)
    {
        StartCoroutine(TomarBalon());
        transform.SetParent(null);
        rigidbody.useGravity = true;
        jugadorHavingBall = false;
        GummingoHavingBall = false;
        rigidbody.AddForce(direccion * 10, ForceMode.Impulse);
    }

    public void dispararRayo()
    {
        if (Instance == null && incioRayo != null && ActiveRayo == false)
        {
            camera.gameObject.SetActive(true);
            ActiveRayo = true;
            instanciarRayo();
        }
    }

    private void instanciarRayo()
    {
        if (Instance == null && incioRayo != null)
        {
            Destroy(Instance);
            Instance = Instantiate(rayo, incioRayo.transform.position, incioRayo.transform.rotation);
            Instance.transform.parent = transform;
            LaserScript = Instance.GetComponent<Hovl_Laser>();
            LaserScript2 = Instance.GetComponent<Hovl_Laser2>();
            StartCoroutine(TiempoRayo());
        }
    }

    void destruirRayo()
    {
        camera.gameObject.SetActive(false);
        if (LaserScript) LaserScript.DisablePrepare();
        if (LaserScript2) LaserScript2.DisablePrepare();
        Destroy(Instance);
    }

    private IEnumerator TomarBalon()
    {
        retardoTiempo = false;
        yield return new WaitForSeconds(0.5f);
        retardoTiempo = true;
    }

    private IEnumerator TiempoRayo()
    {
        yield return new WaitForSeconds(5f);
        ActiveRayo = false;
        posicionarRayo();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != jugador && collision.gameObject != gummingo && ActiveRayo)
        {
            Debug.Log("Colision Balon" + collision.gameObject.name);
            ActiveRayo = false;
            destruirRayo();
            posicionarRayo();
            StopCoroutine(TiempoRayo());
        }
    }
}
