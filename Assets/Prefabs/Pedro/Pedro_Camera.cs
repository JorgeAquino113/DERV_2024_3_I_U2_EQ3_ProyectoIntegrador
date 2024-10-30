using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedro_CameraOrbit : MonoBehaviour
{
    public Transform jugador;
    [SerializeField] private float sensibilidadMouse = 100f;

    private float rotacionX = 0f;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

            jugador.Rotate(Vector3.up * mouseX);
            rotacionX -= mouseY;
            rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);
            transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        }
    }
}
