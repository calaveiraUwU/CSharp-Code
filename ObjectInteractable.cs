using System.Collections;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour, IInteractable
{
    private Transform _facePoint;


    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;  // Velocidad de rotación
    private bool isRotating = false;
    private bool isInteracting = false;
    private Vector3 _startPosition;
    private bool _isPicking = false;
    private Quaternion _initialRotation;
    

    private void Start()
    {
        _facePoint = GlobalProperties.Instance.facePoint;
        _startPosition = this.transform.position;
        _initialRotation = this.transform.rotation;
    }

    IEnumerator RotarHaciaInicial()
    {
        Quaternion rotacionActual = transform.rotation;
        float tiempo = 0f;

        while (tiempo < 1f)
        {
            // Lerp entre la rotación actual y la inicial, basado en el tiempo
            transform.rotation = Quaternion.Lerp(rotacionActual, _initialRotation, tiempo / 1f);
            tiempo += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que la rotación final sea exactamente la inicial
        transform.rotation = _initialRotation;
    }

    private IEnumerator RotateObject()
    {
        while (isInteracting)  // Usar bandera para controlar la corrutina
        {
            // Detectar si se hace clic izquierdo del ratón
            if (Input.GetMouseButtonDown(0))
            {
                isRotating = true;
                   // Detener el movimiento del jugador
            }

            // Detectar si se suelta el clic izquierdo del ratón
            if (Input.GetMouseButtonUp(0))
            {
                isRotating = false;
            }

            // Si el botón del ratón está presionado, rotar el objeto
            if (isRotating)
            {
                float mouseX = Input.GetAxis("Mouse X");  // Movimiento en el eje X del ratón
                float mouseY = Input.GetAxis("Mouse Y");  // Movimiento en el eje Y del ratón

                // Rotar el objeto en el eje Y según el movimiento del ratón en X
                transform.Rotate(Vector3.up, -mouseX * rotationSpeed * Time.deltaTime, Space.World);

                // Rotar el objeto en el eje X según el movimiento del ratón en Y
                transform.Rotate(Vector3.right, mouseY * rotationSpeed * Time.deltaTime, Space.World);
            }

            // Si se presiona "X", detener la interacción
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(MoveToFacePoint(_startPosition));
                StartCoroutine(RotarHaciaInicial());
                //StopInteraction();
                _facePoint.gameObject.SetActive(false);
            }

            yield return null;  // Añadir una pausa para evitar crasheos
        }
    }
    private IEnumerator MoveToFacePoint(Vector3 _pos)
    {
        _isPicking = !_isPicking;
        while (Vector3.Distance(transform.position, _pos) > 0.01f)  // Si aún no hemos llegado
        {
            // Interpolación lineal para acercarse al _facePoint
            transform.position = Vector3.Lerp(transform.position, _pos, moveSpeed * Time.deltaTime);
            yield return null;  // Esperar al siguiente frame
        }

        // Asegurarse de que el objeto esté exactamente en la posición del _facePoint al terminar
        transform.position = _pos;
        if (_isPicking)
            StartCoroutine(RotateObject());
        else
            StopInteraction();
    }

    public void EInteract()
    {
        if (!isInteracting)
        {
            gameObject.layer = 0;
            isInteracting = true;
            _facePoint.gameObject.SetActive(true);
            StartCoroutine(MoveToFacePoint(_facePoint.position));
            //StartCoroutine(RotateObject());
            Cursor.lockState = CursorLockMode.Confined;
            StopPlayerMovement.Instance.PlayerStop();
        }
    }

    // Método para detener la rotación y restaurar el control del cursor
    public void StopInteraction()
    {
        gameObject.layer = 10;
        isInteracting = false;  // Detener la corrutina
        isRotating = false;
        Cursor.lockState = CursorLockMode.Locked;  // Bloquear el cursor al finalizar la interacción
        StopPlayerMovement.Instance.PlayerStop();  // Reanudar el movimiento del jugador
    }

    public void ClickInteract(ref bool isMoving)
    {
        // Lógica adicional si es necesario
    }
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObjectInteractable : MonoBehaviour, IInteractable
//{

//    [SerializeField] private Transform _facePoint;

//    public float rotationSpeed = 100f;  // Velocidad de rotación

//    private bool isRotating = false;


//    private IEnumerator RotateObject()
//    {
//        while(!Input.GetKeyDown(KeyCode.Escape))
//        {
//            // Detectar si se hace clic izquierdo del ratón
//            if (Input.GetMouseButtonDown(0))
//            {
//                isRotating = true;
//            }

//            // Detectar si se suelta el clic izquierdo del ratón
//            if (Input.GetMouseButtonUp(0))
//            {
//                isRotating = false;
//            }

//            // Si el botón del ratón está presionado, rotar el objeto
//            if (isRotating)
//            {
//                float mouseX = Input.GetAxis("Mouse X");  // Movimiento en el eje X del ratón
//                float mouseY = Input.GetAxis("Mouse Y");  // Movimiento en el eje Y del ratón

//                // Rotar el objeto en el eje Y según el movimiento del ratón en X
//                transform.Rotate(Vector3.up, -mouseX * rotationSpeed * Time.deltaTime, Space.World);

//                // Rotar el objeto en el eje X según el movimiento del ratón en Y
//                transform.Rotate(Vector3.right, mouseY * rotationSpeed * Time.deltaTime, Space.World);
//            }
//        }

//        yield return null;
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    public void EInteract()
//    {
//       StopPlayerMovement.Instance.PlayerStop();
//       Cursor.lockState = CursorLockMode.Confined;
//       StartCoroutine(RotateObject());
//    }

//    public void ClickInteract(ref bool isMoving)
//    {

//    }
//}
