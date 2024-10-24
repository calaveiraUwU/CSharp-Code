using UnityEngine;
using System.Collections;

public class DoorInteractible : MonoBehaviour, IInteractable
{
    public float maxRotation = 90f; // Rotación máxima de la puerta
    public float minRotation = 0f;  // Rotación mínima (cerrada)
    public float rotationSpeed = 45f; // Velocidad de rotación
    private bool isOpening = false;  // Controla si la puerta está abriéndose
    public float mouseX;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine rotateCoroutine;
    private bool _isDragging;

    //bool IInteractable._isDragging { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void OnEnable()
    {
        // Define la rotación cerrada y abierta
        closedRotation = Quaternion.Euler(0, minRotation, 0);
        openRotation = Quaternion.Euler(0, maxRotation, 0);
    }

    public void EInteract()
    {
        // Aquí puedes añadir la funcionalidad que desees para la interacción.
    }

    public void ClickInteract(ref bool isDragging)
    {
        _isDragging = isDragging;
        if (isOpening)
        {
            if (rotateCoroutine == null)
            {
                // Inicia la corutina si no está en ejecución
                rotateCoroutine = StartCoroutine(RotateDoor());
            }
        }
        else
        {
            if (rotateCoroutine != null)
            {
                // Detiene la corutina si el botón no está presionado
                StopCoroutine(rotateCoroutine);
                rotateCoroutine = null;
            }

            // Rota la puerta hacia la posición cerrada
            StopAllCoroutines();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, closedRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator RotateDoor()
    {
        while (isOpening && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Im in!!!!!");
            // Obtiene el movimiento horizontal del ratón
            mouseX = Input.GetAxis("Mouse X");
            float step = rotationSpeed * Time.deltaTime;

            // Calcula la rotación deseada en función del movimiento del ratón
            float targetRotationY = Mathf.Clamp(transform.eulerAngles.y + mouseX * rotationSpeed * Time.deltaTime * 100, minRotation, maxRotation);
            Debug.Log(targetRotationY);
            Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);

            // Rota la puerta hacia la rotación deseada
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

            if(Input.GetMouseButtonUp(0))
                yield return null;
        }
    }
}


//using System.Collections;
//using UnityEngine;

//public class DoorInteractible : MonoBehaviour, IInteractable
//{
//    private HingeJoint _hingeJoint;
//    private JointMotor motor;
//    private bool isDragging = false;
//    private Transform playerCamera;
//    [SerializeField]private float dragSpeed = 10f; // Ajusta este valor para cambiar la sensibilidad del arrastre

//    void OnEnable()
//    {
//        _hingeJoint = GetComponent<HingeJoint>();
//        motor = _hingeJoint.motor;
//        _hingeJoint.useMotor = false;
//        playerCamera = Camera.main.transform;
//    }

//    public void EInteract()
//    {
//        // Aquí puedes añadir la funcionalidad que desees para la interacción.
//    }

//    public void ClickInteract(bool _isDragging)
//    {
//        isDragging = _isDragging;

//        if (isDragging)
//        {
//            _hingeJoint.useMotor = true;
//            StartCoroutine(DoorFunction());
//        }
//        else
//        {
//            _hingeJoint.useMotor = false;
//            StopCoroutine(DoorFunction());
//        }
//    }


//    private IEnumerator DoorFunction()
//    {
//        Vector3 lastMousePosition = Input.mousePosition; // Guardar la última posición del ratón

//        while (isDragging)
//        {
//            Vector3 mouseDelta = Input.mousePosition - lastMousePosition; // Calcular el cambio en la posición del ratón
//            lastMousePosition = Input.mousePosition; // Actualizar la última posición del ratón
//            //mouseDelta.Normalize();
//            // Proyectar el movimiento del ratón en el espacio local de la puerta
//            Vector3 localMouseDelta = transform.InverseTransformDirection(mouseDelta);

//            // Usar la componente X del delta del ratón para determinar la velocidad del motor
//            motor.targetVelocity = localMouseDelta.x * dragSpeed * 100;
//            _hingeJoint.motor = motor;

//            if (Input.GetMouseButtonUp(0))
//            {
//                isDragging = false;
//                _hingeJoint.useMotor = false;
//            }

//            yield return null;
//        }
//    }


    //private IEnumerator DoorFunction()
    //{
    //    while (isDragging)
    //    {
    //        Vector3 directionToCamera = playerCamera.position - transform.position;
    //        directionToCamera.Normalize();
    //        float mouseInputX = Input.GetAxis("Mouse X") * -directionToCamera.x;
    //        float mouseInputY = Input.GetAxis("Mouse Y") * -directionToCamera.z;

    //        motor.targetVelocity = (mouseInputX + mouseInputY) * dragSpeed* 100;
    //        _hingeJoint.motor = motor;

    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            isDragging = false;
    //            _hingeJoint.useMotor = false;
    //        }

    //        yield return null;
    //    }
    //}
