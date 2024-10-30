using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level04_Controller : MonoBehaviour
{
    GameObject balon;
    BalonPablo balonPablo;
    BalonMrJosh balonMrJosh;

    List<KeyCode> keys = new List<KeyCode> {
        KeyCode.Space, KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D
    };

    KeyCode currentKey;
    int index;
    private float fuerzaGlobal = 100f;

    void Awake()
    {
        balon = GameObject.Find("Balon N4");
    }

    void Start()
    {
        balonPablo = balon.GetComponent<BalonPablo>();
        balonMrJosh = balon.GetComponent<BalonMrJosh>();

        // Inicializar fuerza de cada balón en 50 para que suma total sea 100
        balonPablo.setFuerza(fuerzaGlobal / 2);
        balonMrJosh.setFuerza(fuerzaGlobal / 2);

        StartCoroutine(GenerarFuerzaNPC());
        StartCoroutine(CambiarTeclaJugador());
    }

    void Update()
    {
        if (Input.GetKeyDown(currentKey))
        {
            AumentarFuerzaPablo(0.1f); // Aumento en décimas
        }
    }

    private IEnumerator GenerarFuerzaNPC()
    {
        while (true)
        {
            float fuerza = Random.Range(0.15f, 0.25f);  // Cambios en décimas
            AumentarFuerzaMrJosh(fuerza);
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator CambiarTeclaJugador()
    {
        while (true)
        {
            index = Random.Range(0, keys.Count);
            currentKey = keys[index];  // Cambia la tecla cada 5 segundos
            Debug.Log("Presiona la tecla: " + currentKey);

            yield return new WaitForSeconds(2f);
        }
    }

    void AumentarFuerzaPablo(float fuerza)
    {
        if (balonPablo.getfuerza() + fuerza <= fuerzaGlobal)
        {
            balonPablo.aumentarFuerza(fuerza);
            balonMrJosh.disminuirFuerza(fuerza);
        }
    }

    void AumentarFuerzaMrJosh(float fuerza)
    {
        if (balonMrJosh.getfuerza() + fuerza <= fuerzaGlobal)
        {
            balonMrJosh.aumentarFuerza(fuerza);
            balonPablo.disminuirFuerza(fuerza);
        }
    }
}
