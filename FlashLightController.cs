using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _handPlayer;

    private Light _flashLight;
    private bool _flashLightEnabled;
    bool _ispicked =false;
    public Transform cameraTransform;   // Referencia a la cámara
    public Transform armTransform;      // Referencia al brazo derecho
    public float positionFollowSpeed = 5f; // Velocidad de movimiento del brazo (lag en la posición)
    public float rotationSpeed = 5f;    // Velocidad de rotación del brazo (lag en la rotación)
    public Vector3 positionOffset;      // Offset de posición (para que los brazos no estén pegados)
    public Vector3 rotationOffset;      // Offset de rotación (para corregir la orientación de la mano)

    private float currentYaw;           // Ángulo actual para aplicar el lag en el eje Y (lateral)
    private float currentPitch;         // Ángulo actual para aplicar el lag en el eje X (subir/bajar)
    private float targetYaw;            // Ángulo objetivo en el eje Y de la cámara
    private float targetPitch;          // Ángulo objetivo en el eje X de la cámara



    // Dibujar los límites en la pantalla
    void OnDrawGizmosSelected()
    {
//#if UNITY_EDITOR
//        // Color para los límites
//        Gizmos.color = Color.green;

//        // Dibujar un cuadro que representa los límites
//        Gizmos.DrawCube(transform.position, maxBounds);
//#endif
    }


    private void Awake()
    {
        TryGetComponent<Light>(out Light fl);
        _flashLight = fl;
        cameraTransform = Camera.main.transform;
        currentYaw = cameraTransform.eulerAngles.y;
    }

    void Update()
    {
        if (armTransform == null)
            return; // No hacer nada si no se ha asignado un objeto externo


        if (_ispicked)
        {
            // 1. Mantener la posición del brazo con un offset relativo a la cámara
            Vector3 targetPosition = cameraTransform.position + cameraTransform.TransformDirection(positionOffset);
            armTransform.position = Vector3.Lerp(armTransform.position, targetPosition, Time.deltaTime * positionFollowSpeed);

            // 2. Obtener los ángulos objetivo de la cámara para yaw (Y) y pitch (X)
            targetYaw = cameraTransform.eulerAngles.y;
            targetPitch = cameraTransform.eulerAngles.x;

            // 3. Suavizamos la transición para lograr el lag en los ejes Y (lateral) y X (subir/bajar)
            currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, Time.deltaTime * rotationSpeed);
            currentPitch = Mathf.LerpAngle(currentPitch, targetPitch, Time.deltaTime * rotationSpeed);

            // 4. Aplicamos el lag en los ejes X y Y, pero mantenemos el eje Z de la cámara (Roll) sin cambios
            Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, cameraTransform.eulerAngles.z) * Quaternion.Euler(rotationOffset);

            // 5. Aplicar la rotación con el lag suavizado al brazo
            armTransform.rotation = targetRotation;
        }

    }


    private void Activate()
    {
        _flashLight.enabled = !_flashLightEnabled;
        _flashLightEnabled = !_flashLightEnabled;
    }

    public void EInteract()
    {
        transform.parent = _handPlayer;
        //transform.rotation = _handPlayer.rotation;
        HintController.Instance.UpdateText(LanguageManager.Instance.currentLanguage.hintTexts.FindHintByName("flashLight").phrase, 3f);

        transform.localPosition = Vector3.zero;
        Debug.Log(Vector3.zero + "      " + transform.position);
        _ispicked = true;
        

    }

    public void ClickInteract(ref bool drawer)
    {

    }
}
