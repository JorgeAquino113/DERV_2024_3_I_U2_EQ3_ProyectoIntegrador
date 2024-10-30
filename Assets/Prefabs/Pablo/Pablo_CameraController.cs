using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pablo_CameraController : MonoBehaviour
{
    [SerializeField] private Transform jugador;  // Referencia al jugador
    private Vector3 offset;                      // Distancia entre la cámara y el jugador
    [SerializeField] private float velocidadRotacion = 5f;   // Velocidad de rotación de la cámara
    private float rotacionX = 0f;                // Rotación acumulada en X (horizontal)
    private float rotacionY = 0f;                // Rotación acumulada en Y (vertical)


    void Start()
    {
        // Cálculo inicial del offset para mantener la cámara detrás del jugador
        offset = transform.position - jugador.position;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1))  // Rotación manual con clic derecho
        {
            rotacionX += Input.GetAxis("Mouse X") * velocidadRotacion;
            rotacionY -= Input.GetAxis("Mouse Y") * velocidadRotacion;

            // Limitar la rotación vertical para evitar que la cámara gire demasiado arriba o abajo
            rotacionY = Mathf.Clamp(rotacionY, -35, 60);
        }

        Quaternion rotacion = Quaternion.Euler(rotacionY, rotacionX, 0);
        Vector3 posicionObjetivo = jugador.position + rotacion * offset;

        transform.position = posicionObjetivo;
        transform.LookAt(jugador);
    }

    public void cameraActive(bool active)
    {
        gameObject.SetActive(active);
    }


}
