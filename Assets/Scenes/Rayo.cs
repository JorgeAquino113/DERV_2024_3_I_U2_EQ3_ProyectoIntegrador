using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rayo : MonoBehaviour
{
    [SerializeField] private GameObject rayo;
    [SerializeField] private GameObject incioRayo;
    // [SerializeField] private GameObject finRayo;
    [SerializeField] private float tiempoRayo = 1f;
    private GameObject Instance;
    public float MaxLength;

    private Hovl_Laser LaserScript;
    private Hovl_Laser2 LaserScript2;

    [SerializeField] private bool ActiveRayo = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Activa o dispara el rayo
        if (ActiveRayo)
        {
            dispararRayo();
            Instance.transform.forward = transform.up * -1;
        }
        else if (ActiveRayo == false)
        {
            destruirRayo();
        }
        // // Actualiza la posición y rotación del rayo para que siga a finRayo
        // if (Instance != null)
        // {
        //     Instance.transform.position = incioRayo.transform.position;
        //     Instance.transform.LookAt(finRayo.transform); // Apunta al finRayo
        // }

        // RaycastHit hit;
        // if (Physics.Raycast(incioRayo.transform.position, finRayo.transform.position - incioRayo.transform.position, out hit, MaxLength))
        // {
        //     // Si el rayo golpea algo, ajusta la dirección
        //     Vector3 direction = (hit.point - incioRayo.transform.position).normalized;
        //     Instance.transform.forward = direction;
        // }
        // else
        // {
        //     // Si no golpea nada, sigue apuntando a finRayo
        //     Vector3 direction = (finRayo.transform.position - incioRayo.transform.position).normalized;
        //     Instance.transform.forward = direction;
        // }
    }

    void dispararRayo()
    {
        if (Instance == null)
        {
            Destroy(Instance);
            Instance = Instantiate(rayo, incioRayo.transform.position, incioRayo.transform.rotation);
            Instance.transform.parent = transform;
            LaserScript = Instance.GetComponent<Hovl_Laser>();
            LaserScript2 = Instance.GetComponent<Hovl_Laser2>();
        }
    }

    void destruirRayo()
    {
        if (LaserScript) LaserScript.DisablePrepare();
        if (LaserScript2) LaserScript2.DisablePrepare();
        Destroy(Instance, 1);
    }

    public void Activar()
    {
        ActiveRayo = true;
    }

    public void Desactivar()
    {
        ActiveRayo = false;
    }
}
