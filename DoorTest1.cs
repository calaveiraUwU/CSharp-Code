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
    [SerializeField] private Vector3 doorForward; // Dirección frontal de la puerta

    private void OnEnable()
    {
        _camera = Camera.main.transform;
        _doorHinge = GetComponent<HingeJoint>();

        // Definir la dirección frontal de la puerta para calcular la posición del jugador
        doorForward = transform.forward; // O puedes usar otra dirección si el transform está orientado de otra manera
    }

    public void EInteract()
    {
        // Este método puede estar vacío si no lo necesitas en tu implementación.
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
            // Actualizar el multiplicador de dirección dependiendo de la posición del jugador en relación a la puerta
            UpdateDirectionMultiplier();

            // Obtener el movimiento del mouse en el eje X
            float mouseX = Input.GetAxis("Mouse X");

            // Ajustar la velocidad del motor del HingeJoint basado en el movimiento del mouse y la dirección
            JointMotor motor = _doorHinge.motor;
            motor.targetVelocity = mouseX * _vel * directionMultiplier; // Aplicar el multiplicador de dirección
            motor.force = _vel; // Asegurarse de tener suficiente fuerza
            _doorHinge.motor = motor;

            // Activar el motor del HingeJoint
            _doorHinge.useMotor = true;

            yield return null; // Esperar hasta el siguiente frame
        }
    }

    private void UpdateDirectionMultiplier()
    {
        // Calcular la posición relativa del jugador en relación a la puerta
        Vector3 playerToDoor = _camera.position - transform.position;

        // Proyectar la posición del jugador en la dirección de la puerta (frontal)
        float dotProduct = Vector3.Dot(playerToDoor.normalized, doorForward);

        // Si el jugador está delante de la puerta, usar un multiplicador positivo, si está detrás, invertirlo
        if (dotProduct > 0)
        {
            directionMultiplier = 1f; // Jugador está delante de la puerta
        }
        else
        {
            directionMultiplier = -1f; // Jugador está detrás de la puerta
        }
    }
}