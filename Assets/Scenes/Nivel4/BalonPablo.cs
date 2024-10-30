using System.Collections;
using UnityEngine;

public class BalonPablo : MonoBehaviour
{
    [SerializeField] private GameObject rayo;
    [SerializeField] public Transform inicioRayo;
    [SerializeField] private float tiempoRayo = 1f;
    [SerializeField] public float fuerza;
    private GameObject Instance;
    public float MaxLength;
    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        instanciarRayo();
        if (Instance != null)
        {
            Instance.transform.position = inicioRayo.transform.position;
            Instance.transform.LookAt(transform);
        }

        RaycastHit hit;
        Vector3 direccion = (transform.position - inicioRayo.transform.position).normalized;
        if (Physics.Raycast(inicioRayo.transform.position, direccion, out hit, MaxLength))
        {
            direccion = (hit.point - inicioRayo.transform.position).normalized;
            Instance.transform.forward = direccion;
        }
        else
        {
            Instance.transform.forward = direccion;
        }

        rigidbody.AddForce(transform.right * fuerza, ForceMode.Acceleration);

    }

    private void instanciarRayo()
    {
        if (Instance == null && inicioRayo != null)
        {
            Destroy(Instance);
            Instance = Instantiate(rayo, inicioRayo.transform.position, inicioRayo.transform.rotation);
            Instance.transform.parent = transform;
        }
    }

    public void aumentarFuerza(float fuerza)
    {
        this.fuerza += fuerza;
    }

    public void disminuirFuerza(float fuerza)
    {
        this.fuerza = Mathf.Max(this.fuerza - fuerza, 0); // Asegura que no sea negativa
    }

    public void setFuerza(float fuerza)
    {
        this.fuerza = fuerza;
    }

    public float getfuerza()
    {
        return this.fuerza;
    }
}
