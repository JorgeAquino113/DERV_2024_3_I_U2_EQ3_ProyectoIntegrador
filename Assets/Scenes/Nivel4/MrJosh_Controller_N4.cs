using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrJosh_Controller_N4 : MonoBehaviour
{
    GameObject balon;
    BalonMrJosh balonMrJosh;
    bool cambio = true;
    void Awake()
    {
        balon = GameObject.Find("Balon N4");
    }
    void Start()
    {
        balonMrJosh = balon.GetComponent<BalonMrJosh>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cambio)
        {
            float fuerza = Random.Range(1, 5);
            fuerza = fuerza / 100;
            balonMrJosh.aumentarFuerza(fuerza);
            cambio = false;
        }
    }

    private IEnumerator delayTecla()
    {
        yield return new WaitForSeconds(1f);
        cambio = true;
    }
}
