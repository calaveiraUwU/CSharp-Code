using System.Collections;
using UnityEngine;

public class DoorTest1 : MonoBehaviour, IInteractable
{
    private HingeJoint _doorHinge;
    private Transform _camera;
    private Coroutine _doorControlCoroutine;
    private bool _isDragging = false;

    //[SerializeField] private Transform player; // Referencia al transform del jugador
    [SerializeField] private float _vel = 500f;
    [SerializeField] private float directionMultiplier = 1f; // Factor para invertir el movimiento
    [SerializeField] private Vector3 doorForward; // Direcci�n frontal de la puerta

    private void OnEnable()
    {
        _camera = Camera.main.transform;
        _doorHinge = GetComponent<HingeJoint>();

        // Definir la direcci�n frontal de la puerta para calcular la posici�n del jugador
        doorForward = transform.forward; // O puedes usar otra direcci�n si el transform est� orientado de otra manera
    }

    public void EInteract()
    {
        // Este m�todo puede estar vac�o si no lo necesitas en tu implementaci�n.
    }

    public void ClickInteract(ref bool isMoving)
    {
        if (isMoving && !_isDragging)
        {
            // Comenzar a arrastrar y mover la puerta
            _isDragging = true;
            _doorControlCoroutine = StartCoroutine(ControlDoor());
        }
        else if (!isMoving && _isDragging)
        {
            // Dejar de arrastrar y detener la puerta
            _isDragging = false;

            if (_doorControlCoroutine != null)
            {
                StopCoroutine(_doorControlCoroutine);
                _doorControlCoroutine = null;
            }

            // Detener el movimiento del motor del HingeJoint
            var motor = _doorHinge.motor;
            motor.targetVelocity = 0;
            _doorHinge.motor = motor;

            // Desactivar el motor del HingeJoint
            _doorHinge.useMotor = false;
        }
    }

    private IEnumerator ControlDoor()
    {
        while (_isDragging)
        {
            // Actualizar el multiplicador de direcci�n dependiendo de la posici�n del jugador en relaci�n a la puerta
            UpdateDirectionMultiplier();

            // Obtener el movimiento del mouse en el eje X
            float mouseX = Input.GetAxis("Mouse X");

            // Ajustar la velocidad del motor del HingeJoint basado en el movimiento del mouse y la direcci�n
            JointMotor motor = _doorHinge.motor;
            motor.targetVelocity = mouseX * _vel * directionMultiplier; // Aplicar el multiplicador de direcci�n
            motor.force = _vel; // Asegurarse de tener suficiente fuerza
            _doorHinge.motor = motor;

            // Activar el motor del HingeJoint
            _doorHinge.useMotor = true;

            yield return null; // Esperar hasta el siguiente frame
        }
    }

    private void UpdateDirectionMultiplier()
    {
        // Calcular la posici�n relativa del jugador en relaci�n a la puerta
        Vector3 playerToDoor = _camera.position - transform.position;

        // Proyectar la posici�n del jugador en la direcci�n de la puerta (frontal)
        float dotProduct = Vector3.Dot(playerToDoor.normalized, doorForward);

        // Si el jugador est� delante de la puerta, usar un multiplicador positivo, si est� detr�s, invertirlo
        if (dotProduct > 0)
        {
            directionMultiplier = 1f; // Jugador est� delante de la puerta
        }
        else
        {
            directionMultiplier = -1f; // Jugador est� detr�s de la puerta
        }
    }
}