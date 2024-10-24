using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _handPlayer;

    private Light _flashLight;
    private bool _flashLightEnabled;
    bool _ispicked =false;
    public Transform cameraTransform;   // Referencia a la c�mara
    public Transform armTransform;      // Referencia al brazo derecho
    public float positionFollowSpeed = 5f; // Velocidad de movimiento del brazo (lag en la posici�n)
    public float rotationSpeed = 5f;    // Velocidad de rotaci�n del brazo (lag en la rotaci�n)
    public Vector3 positionOffset;      // Offset de posici�n (para que los brazos no est�n pegados)
    public Vector3 rotationOffset;      // Offset de rotaci�n (para corregir la orientaci�n de la mano)

    private float currentYaw;           // �ngulo actual para aplicar el lag en el eje Y (lateral)
    private float currentPitch;         // �ngulo actual para aplicar el lag en el eje X (subir/bajar)
    private float targetYaw;            // �ngulo objetivo en el eje Y de la c�mara
    private float targetPitch;          // �ngulo objetivo en el eje X de la c�mara



    // Dibujar los l�mites en la pantalla
    void OnDrawGizmosSelected()
    {
//#if UNITY_EDITOR
//        // Color para los l�mites
//        Gizmos.color = Color.green;

//        // Dibujar un cuadro que representa los l�mites
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
            // 1. Mantener la posici�n del brazo con un offset relativo a la c�mara
            Vector3 targetPosition = cameraTransform.position + cameraTransform.TransformDirection(positionOffset);
            armTransform.position = Vector3.Lerp(armTransform.position, targetPosition, Time.deltaTime * positionFollowSpeed);

            // 2. Obtener los �ngulos objetivo de la c�mara para yaw (Y) y pitch (X)
            targetYaw = cameraTransform.eulerAngles.y;
            targetPitch = cameraTransform.eulerAngles.x;

            // 3. Suavizamos la transici�n para lograr el lag en los ejes Y (lateral) y X (subir/bajar)
            currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, Time.deltaTime * rotationSpeed);
            currentPitch = Mathf.LerpAngle(currentPitch, targetPitch, Time.deltaTime * rotationSpeed);

            // 4. Aplicamos el lag en los ejes X y Y, pero mantenemos el eje Z de la c�mara (Roll) sin cambios
            Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, cameraTransform.eulerAngles.z) * Quaternion.Euler(rotationOffset);

            // 5. Aplicar la rotaci�n con el lag suavizado al brazo
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
