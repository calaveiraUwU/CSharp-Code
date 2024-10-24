using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] Camera cam;
    Transform selectedDoor;
    int leftDoor = 0;
    [SerializeField] LayerMask doorLayer;
    private Coroutine drawerCoroutine;
    JointMotor motor;
    private bool _isDragging = false;
    public HingeJoint joint;  // El joint que controla el cajón
    public Transform drawer;  // Transform del cajón
    public float speed = 5f;  // Velocidad de arrastre del cajón

    //bool IInteractable._isDragging { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void OnEnable()
    {
        cam = Camera.main;
        joint = GetComponent<HingeJoint>();
        motor = joint.motor;
        joint.useMotor = true; // Asegúrate de que el motor esté habilitado
        
    }

    public void EInteract()
    {
        // Implementa tu lógica de interacción aquí
    }

    public void ClickInteract(ref bool _ismoving)
    {
        Debug.Log("Interacting: " + _ismoving);
        _isDragging = _ismoving;
        if (_isDragging)
        {
            if (drawerCoroutine != null)
            {
                StopCoroutine(drawerCoroutine);
            }

            // Asignar selectedDoor usando un raycast
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, doorLayer))
            {
                selectedDoor = hit.transform;
            }

            if (selectedDoor != null)
            {
                drawerCoroutine = StartCoroutine(DoorExecutorCoroutine());
            }
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

    private IEnumerator DoorExecutorCoroutine()
    {
        while (_isDragging)
        {
            if (selectedDoor != null)
            {
                // Calcula la distancia del punto de arrastre a la puerta
                float delta = Mathf.Pow(Vector3.Distance(transform.position, selectedDoor.position), 3);

                // Determina la dirección de la puerta
                leftDoor = selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.localPosition.x ? 1 : -1;

                // Calcula la velocidad objetivo del motor
                float speedMultiplier = 60000;
                float directionMultiplier = ((Mathf.Abs(selectedDoor.parent.forward.z) > 0.5f) ?
                                             (transform.position.x > selectedDoor.position.x ? -1 : 1) :
                                             (transform.position.z > selectedDoor.position.z ? -1 : 1));

                // Establece la velocidad objetivo del motor
                motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor * directionMultiplier;
                joint.motor = motor;
            }

            // Verifica si el botón del ratón se ha soltado
            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                selectedDoor = null;
                motor.targetVelocity = 0;
                joint.motor = motor;
            }

            yield return null; // Espera al siguiente frame
        }

        // Resetea la coroutine
        drawerCoroutine = null;
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DoorController : MonoBehaviour, IInteractable
//{
//    [SerializeField] Camera cam;
//    Transform selectedDoor;
//    int leftDoor = 0;
//    [SerializeField] LayerMask doorLayer;
//    private Coroutine drawerCoroutine;
//    JointMotor motor;

//    public HingeJoint joint;  // El joint que controla el cajón
//    public Transform drawer;  // Transform del cajón
//    public float speed = 5f;  // Velocidad de arrastre del cajón
//    private bool isDragging = false;

//    private void OnEnable()
//    {
//        cam = Camera.main;
//        joint = GetComponent<HingeJoint>();
//        motor = joint.motor;
//    }

//    public void EInteract()
//    {
//        // Implementa tu lógica de interacción aquí
//    }

//    public void ClickInteract(bool _ismoving)
//    {
//        Debug.Log(_ismoving);
//        isDragging = _ismoving;
//        if (isDragging)
//        {
//            if (drawerCoroutine != null)
//            {
//                StopCoroutine(drawerCoroutine);
//            }
//            drawerCoroutine = StartCoroutine(DoorExecutorCoroutine());
//        }
//        else
//        {
//            if (drawerCoroutine != null)
//            {
//                StopCoroutine(drawerCoroutine);
//                drawerCoroutine = null;
//            }
//        }
//    }

//    private IEnumerator DoorExecutorCoroutine()
//    {

//        while (isDragging)
//        {
//            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//            if (selectedDoor != null)
//            {
//                // Asegúrate de que selectedDoor esté configurado antes de usarlo
//                transform.SetPositionAndRotation(ray.GetPoint(Vector3.Distance(selectedDoor.position, transform.position)), selectedDoor.rotation);

//                // Calcular delta para el movimiento de la puerta
//                float delta = Mathf.Pow(Vector3.Distance(transform.position, selectedDoor.position), 3);

//                // Determinar la dirección de la puerta
//                leftDoor = selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.localPosition.x ? 1 : -1;

//                // Calcular la velocidad objetivo del motor
//                float speedMultiplier = 60000;
//                float directionMultiplier = ((Mathf.Abs(selectedDoor.parent.forward.z) > 0.5f) ?
//                                             (transform.position.x > selectedDoor.position.x ? -1 : 1) :
//                                             (transform.position.z > selectedDoor.position.z ? -1 : 1));

//                // Establecer la velocidad objetivo del motor

//                motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor * directionMultiplier;
//                joint.motor = motor;
//            }

//            // Mover el chequeo de Input.GetMouseButtonUp(0) aquí para salir del bucle
//            if (Input.GetMouseButtonUp(0))
//            {
//                isDragging = false;
//                selectedDoor = null;
//                motor.targetVelocity = 0;
//                joint.motor = motor;
//            }

//            yield return null; // Espera al siguiente frame
//        }

//        // Resetear la coroutine
//        drawerCoroutine = null;
//    }
//}



//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using UnityEngine;

//public class DoorController : MonoBehaviour, IInteractable
//{

//    [SerializeField] Camera cam;
//    Transform selectedDoor;
//    GameObject dragPointGameobject;
//    int leftDoor = 0;
//    [SerializeField] LayerMask doorLayer;
//    private Coroutine drawerCoroutine;



//    public HingeJoint joint;  // El joint que controla el cajón
//    public Transform drawer;  // Transform del cajón
//    public float speed = 5f;  // Velocidad de arrastre del cajón
//    private bool isDragging = false;

//    private void OnEnable()
//    {
//        cam = Camera.main;
//        joint = GetComponent<HingeJoint>();
//        dragPointGameobject = gameObject;
//    }

//    public void EInteract()
//    {
//        //xd
//    }

//    public void ClickInteract(bool _ismoving)
//    {
//        isDragging = _ismoving;
//        if (isDragging)
//        {
//            if (drawerCoroutine != null)
//            {
//                StopCoroutine(drawerCoroutine);
//            }

//            // Initialize dragPointGameobject if it hasn't been created yet
//            if (dragPointGameobject == null)
//            {
//                dragPointGameobject = new GameObject("DragPoint");
//            }

//            drawerCoroutine = StartCoroutine(DoorExecutorCoroutine());
//        }
//        else
//        {
//            if (drawerCoroutine != null)
//            {
//                StopCoroutine(drawerCoroutine);
//                drawerCoroutine = null;
//            }
//        }
//    }


//    //public void ClickInteract(bool _ismoving)
//    //{
//    //    isDragging = _ismoving;
//    //    if (isDragging)
//    //    {
//    //        if (drawerCoroutine != null)
//    //        {
//    //            StopCoroutine(drawerCoroutine);
//    //        }
//    //        drawerCoroutine = StartCoroutine(DoorExecutorCoroutine());
//    //    }
//    //    else
//    //    {
//    //        if (drawerCoroutine != null)
//    //        {
//    //            StopCoroutine(drawerCoroutine);
//    //            drawerCoroutine = null;
//    //        }
//    //    }
//    //}
//    private IEnumerator DoorExecutorCoroutine()
//    {
//        if (dragPointGameobject == null)
//        {
//            Debug.LogError("Drag point GameObject is not set.");
//            yield break;
//        }

//        JointMotor motor = joint.motor; // Cache motor to reduce property access overhead

//        while (isDragging)
//        {
//            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
//            dragPointGameobject.transform.SetPositionAndRotation(
//                ray.GetPoint(Vector3.Distance(selectedDoor.position, transform.position)),
//                selectedDoor.rotation
//            );

//            float delta = Mathf.Pow(Vector3.Distance(dragPointGameobject.transform.position, selectedDoor.position), 3);
//            leftDoor = selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.localPosition.x ? 1 : -1;

//            float speedMultiplier = 60000;
//            float directionMultiplier = ((Mathf.Abs(selectedDoor.parent.forward.z) > 0.5f) ?
//                (dragPointGameobject.transform.position.x > selectedDoor.position.x ? -1 : 1) :
//                (dragPointGameobject.transform.position.z > selectedDoor.position.z ? -1 : 1));

//            motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor * directionMultiplier;
//            joint.motor = motor;

//            if (Input.GetMouseButtonUp(0))
//            {
//                isDragging = false;
//                selectedDoor = null;
//                motor.targetVelocity = 0;
//                joint.motor = motor;
//                Destroy(dragPointGameobject);
//            }

//            yield return null;
//        }

//        drawerCoroutine = null;
//    }
//}
////    private IEnumerator DoorExecutorCoroutine()
////    {
////        while (isDragging)
////        {
////            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
////            dragPointGameobject.transform.SetPositionAndRotation(ray.GetPoint(Vector3.Distance(selectedDoor.position, transform.position)), selectedDoor.rotation);

////            // Calculate delta for door movement
////            float delta = Mathf.Pow(Vector3.Distance(dragPointGameobject.transform.position, selectedDoor.position), 3);

////            // Determine door direction
////            leftDoor = selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.localPosition.x ? 1 : -1;

////            // Calculate motor target velocity
////            float speedMultiplier = 60000;
////            float directionMultiplier = ((Mathf.Abs(selectedDoor.parent.forward.z) > 0.5f) ?
////                                         (dragPointGameobject.transform.position.x > selectedDoor.position.x ? -1 : 1) :
////                                         (dragPointGameobject.transform.position.z > selectedDoor.position.z ? -1 : 1));

////            // Set motor target velocity
////            JointMotor motor = joint.motor;
////            motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor * directionMultiplier;
////            joint.motor = motor;

////            // Mover el chequeo de Input.GetMouseButtonUp(0) aquí para salir del bucle
////            if (Input.GetMouseButtonUp(0))
////            {
////                isDragging = false;
////                selectedDoor = null;
////                motor.targetVelocity = 0;
////                joint.motor = motor;
////                Destroy(dragPointGameobject);
////            }

////            yield return null; // Espera al siguiente frame
////        }

////        // Resetear la corutina
////        drawerCoroutine = null;
////    }
////}