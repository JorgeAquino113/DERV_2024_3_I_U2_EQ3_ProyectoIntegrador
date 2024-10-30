using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRound : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float velocidad;
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        transform.RotateAround(center.transform.position, center.transform.up, velocidad * Time.deltaTime);
    }
}
