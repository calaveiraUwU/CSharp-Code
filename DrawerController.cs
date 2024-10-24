using UnityEngine;

public class DrawerController : MonoBehaviour
{
    public ConfigurableJoint joint;  // El joint que controla el cajón
    public Transform drawer;  // Transform del cajón
    public float speed = 0.01f;  // Sensibilidad de arrastre del cajón
    private bool isDragging = false;
    private Vector3 initialMousePosition;
    private float initialDrawerZPosition;

    void Update()
    {
        
        // Detectar el click del ratón
        if (Input.GetMouseButtonDown(0))
        {
            // Verifica si el ratón está sobre el cajón
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == drawer)
            {
                isDragging = true;
                initialMousePosition = Input.mousePosition.normalized;
                initialDrawerZPosition = drawer.localPosition.z; // Usar posición local para el movimiento relativo
            }
        }

        // Soltar el cajón al soltar el botón del ratón
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Mover el cajón mientras se arrastra
        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition.normalized - initialMousePosition;
            float zMove = mouseDelta.y * speed; // Cambiar a mouseDelta.y para movimiento vertical y ajustar sensibilidad

            // Calcular la nueva posición z del cajón
            float newLocalZ = initialDrawerZPosition + zMove;

            // Aplicar movimiento al Rigidbody del cajón en espacio local
            Vector3 newPosition = drawer.localPosition;
            newPosition.z = newLocalZ;

            // Limitar el movimiento según los límites del ConfigurableJoint
            float zMin = -joint.linearLimit.limit;  // Asegurarse de que los límites son coherentes
            float zMax = joint.linearLimit.limit;
            newPosition.z = Mathf.Clamp(newPosition.z, zMin, zMax);

            // Convertir la posición local a posición global antes de mover el Rigidbody
            Vector3 worldPosition = drawer.parent.TransformPoint(newPosition);
            joint.connectedBody.MovePosition(worldPosition);
        }
    }
}
