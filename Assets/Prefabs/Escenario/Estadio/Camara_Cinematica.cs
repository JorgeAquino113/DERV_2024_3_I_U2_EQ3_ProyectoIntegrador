using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara_Cinematica : MonoBehaviour
{
    [SerializeField] private Transform objetivo; // El balón u otro objeto que la cámara sigue
    [SerializeField] private Vector3 offset;     // Distancia fija entre la cámara y el balón

    private Quaternion rotacionInicial;

    void Start()
    {
        rotacionInicial = transform.rotation;
    }
    void Update()
    {
        transform.position = objetivo.position + offset;
        transform.rotation = rotacionInicial;
    }
}
