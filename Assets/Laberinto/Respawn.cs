using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint; // Arrastra aquí el objeto "RespawnPoint" en el Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto tocado es un pico (puedes asignar una etiqueta)
        if (collision.CompareTag("Pico"))
        {
            // Teletransporta el personaje a la posición de inicio
            transform.position = respawnPoint.position;
        }
    }
}
