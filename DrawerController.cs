using UnityEngine;

public class DrawerController : MonoBehaviour
{
    public ConfigurableJoint joint;  // El joint que controla el caj�n
    public Transform drawer;  // Transform del caj�n
    public float speed = 0.01f;  // Sensibilidad de arrastre del caj�n
    private bool isDragging = false;
    private Vector3 initialMousePosition;
    private float initialDrawerZPosition;

    void Update()
    {
        
        // Detectar el click del rat�n
        if (Input.GetMouseButtonDown(0))
        {
            // Verifica si el rat�n est� sobre el caj�n
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == drawer)
            {
                isDragging = true;
                initialMousePosition = Input.mousePosition.normalized;
                initialDrawerZPosition = drawer.localPosition.z; // Usar posici�n local para el movimiento relativo
            }
        }

        // Soltar el caj�n al soltar el bot�n del rat�n
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Mover el caj�n mientras se arrastra
        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition.normalized - initialMousePosition;
            float zMove = mouseDelta.y * speed; // Cambiar a mouseDelta.y para movimiento vertical y ajustar sensibilidad

            // Calcular la nueva posici�n z del caj�n
            float newLocalZ = initialDrawerZPosition + zMove;

            // Aplicar movimiento al Rigidbody del caj�n en espacio local
            Vector3 newPosition = drawer.localPosition;
            newPosition.z = newLocalZ;

            // Limitar el movimiento seg�n los l�mites del ConfigurableJoint
            float zMin = -joint.linearLimit.limit;  // Asegurarse de que los l�mites son coherentes
            float zMax = joint.linearLimit.limit;
            newPosition.z = Mathf.Clamp(newPosition.z, zMin, zMax);

            // Convertir la posici�n local a posici�n global antes de mover el Rigidbody
            Vector3 worldPosition = drawer.parent.TransformPoint(newPosition);
            joint.connectedBody.MovePosition(worldPosition);
        }
    }
}
