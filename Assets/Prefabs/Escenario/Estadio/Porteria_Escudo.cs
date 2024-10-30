using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Porteria_Escudo : MonoBehaviour
{
    [SerializeField] private List<Material> escudos;
    [SerializeField] private Transform center;
    [SerializeField] private Transform balon;
    int index;

    void Start()
    {
        int index = 0;
        GetComponent<MeshRenderer>().material = escudos[index];

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Balon" && index < 2)
        {
            index++;
            GetComponent<MeshRenderer>().material = escudos[index];
        }
        balon.position = center.position;
        balon.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
