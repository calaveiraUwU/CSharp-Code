using UnityEngine;
using System.Collections;

public class DoorInteractible : MonoBehaviour, IInteractable
{
    public float maxRotation = 90f; // Rotaci�n m�xima de la puerta
    public float minRotation = 0f;  // Rotaci�n m�nima (cerrada)
    public float rotationSpeed = 45f; // Velocidad de rotaci�n
    private bool isOpening = false;  // Controla si la puerta est� abri�ndose
    public float mouseX;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine rotateCoroutine;
    private bool _isDragging;

    //bool IInteractable._isDragging { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void OnEnable()
    {
        // Define la rotaci�n cerrada y abierta
        closedRotation = Quaternion.Euler(0, minRotation, 0);
        openRotation = Quaternion.Euler(0, maxRotation, 0);
    }

    public void EInteract()
    {
        // Aqu� puedes a�adir la funcionalidad que desees para la interacci�n.
    }

    public void ClickInteract(ref bool isDragging)
    {
        _isDragging = isDragging;
        if (isOpening)
        {
            if (rotateCoroutine == null)
            {
                // Inicia la corutina si no est� en ejecuci�n
                rotateCoroutine = StartCoroutine(RotateDoor());
            }
        }
        else
        {
            if (rotateCoroutine != null)
            {
                // Detiene la corutina si el bot�n no est� presionado
                StopCoroutine(rotateCoroutine);
                rotateCoroutine = null;
            }

            // Rota la puerta hacia la posici�n cerrada
            StopAllCoroutines();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, closedRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator RotateDoor()
    {
        while (isOpening && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Im in!!!!!");
            // Obtiene el movimiento horizontal del rat�n
            mouseX = Input.GetAxis("Mouse X");
            float step = rotationSpeed * Time.deltaTime;

            // Calcula la rotaci�n deseada en funci�n del movimiento del rat�n
            float targetRotationY = Mathf.Clamp(transform.eulerAngles.y + mouseX * rotationSpeed * Time.deltaTime * 100, minRotation, maxRotation);
            Debug.Log(targetRotationY);
            Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);

            // Rota la puerta hacia la rotaci�n deseada
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
//        // Aqu� puedes a�adir la funcionalidad que desees para la interacci�n.
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
//        Vector3 lastMousePosition = Input.mousePosition; // Guardar la �ltima posici�n del rat�n

//        while (isDragging)
//        {
//            Vector3 mouseDelta = Input.mousePosition - lastMousePosition; // Calcular el cambio en la posici�n del rat�n
//            lastMousePosition = Input.mousePosition; // Actualizar la �ltima posici�n del rat�n
//            //mouseDelta.Normalize();
//            // Proyectar el movimiento del rat�n en el espacio local de la puerta
//            Vector3 localMouseDelta = transform.InverseTransformDirection(mouseDelta);

//            // Usar la componente X del delta del rat�n para determinar la velocidad del motor
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
