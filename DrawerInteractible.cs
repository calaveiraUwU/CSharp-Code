using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerInteractible : MonoBehaviour, IInteractable
{
    public float moveDistance = 0.5f; // Distancia m�xima que el caj�n se puede mover
    public float moveSpeed = 2f;      // Velocidad de movimiento del caj�n
    public Transform playerCamera;    // Referencia a la c�mara del jugador

    private Vector3 initialPosition;  // Posici�n inicial del caj�n
    private bool _isMoving = false;    // Verifica si el caj�n se est� moviendo
    private float currentOffset = 0f; // Desplazamiento actual del caj�n

    private Coroutine drawerCoroutine;

    public bool _isDragging { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void OnEnable()
    {
        initialPosition = transform.position;
        playerCamera = Camera.main.transform;
    }

    public void EInteract()
    {
        //nada
    }

    public void ClickInteract(ref bool isMoving)
    {
        Debug.Log(isMoving);
        _isMoving = isMoving;

        if (_isMoving)
        {
            if (drawerCoroutine != null)
            {
                StopCoroutine(drawerCoroutine);
            }
            drawerCoroutine = StartCoroutine(DrawerExecutorCoroutine());
        }
        else
        {
            if (drawerCoroutine != null)
            {
                StopCoroutine(drawerCoroutine);
                drawerCoroutine = null;
            }
        }
    }

    private IEnumerator DrawerExecutorCoroutine()
    {
        while (_isMoving)
        {
            // Obtener la direcci�n desde la c�mara hacia el caj�n
            Vector3 directionToCamera = playerCamera.position - transform.position;
            directionToCamera.Normalize();

            // Invertimos el eje Y multiplic�ndolo por -1
            float mouseInputX = Input.GetAxis("Mouse X") * directionToCamera.x;
            float mouseInputY = Input.GetAxis("Mouse Y") * -directionToCamera.z; // Invertir el eje Y
            float mouseInput = mouseInputX + mouseInputY;

            float moveAmount = mouseInput * moveSpeed * Time.deltaTime;

            currentOffset = Mathf.Clamp(currentOffset + moveAmount, 0, moveDistance);
            transform.position = initialPosition + transform.forward * currentOffset;

            // Mover el chequeo de Input.GetMouseButtonUp(0) aqu� para salir del bucle
            if (Input.GetMouseButtonUp(0))
            {
                _isMoving = false;
            }

            yield return null; // Espera al siguiente frame
        }

        // Resetear la corutina
        drawerCoroutine = null;
    }
}