using UnityEngine;
using System.Collections;
using StarterAssets;
using Cinemachine;
using System.Collections.Generic;

public class CameraToggleRotation : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 45f;  // Ángulo de rotación
    [SerializeField] private float rotationSpeed = 2f;   // Velocidad de la rotación
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private CinemachineBrain _cmb;
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private GameObject _objectiveRotationObject;
    [SerializeField] private List<GameObject> _keys = new List<GameObject>();
    [SerializeField] private AnimatorController _ac;
    [SerializeField] private bool _inventoryState;


    private bool isRotated = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Coroutine currentCoroutine;


    //EVENTS


    private void Start()
    {
        _firstPersonController = FindAnyObjectByType<FirstPersonController>();
        _cmb = gameObject.GetComponent<CinemachineBrain>();
        _cam = FindAnyObjectByType<CinemachineVirtualCamera>();
        initialRotation = transform.rotation;  // Guarda la rotación inicial
    }

    private void OnEnable()
    {

        KeyController.OnCollected += InventoryPlus;

    }

    private void OnDisable()
    {

        KeyController.OnCollected -= InventoryPlus;

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            _inventoryState = !_inventoryState;

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Calcula la diferencia de ángulo solo en el eje Y (rotación horizontal)
            rotationAngle = DistanceBetweenAngles(gameObject.transform.position, _objectiveRotationObject.transform.position);

            if (!isRotated)
            {
                initialRotation = transform.rotation;
                //_firstPersonController.enabled = false;
                //_cmb.enabled = false;
                //_cam.enabled = false;
                StopPlayerMovement.Instance.PlayerStop();

                // Rotación solo en el eje Y
                targetRotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x + rotationAngle, -60f, 60f), transform.eulerAngles.y, transform.eulerAngles.z);
                currentCoroutine = StartCoroutine(RotateCamera(transform.rotation, targetRotation));
            }
            else
            {
                currentCoroutine = StartCoroutine(RotateCamera(transform.rotation, initialRotation));
            }

            _ac.Inventory(_inventoryState);

            if (_keys.Count > 0)
            {
                foreach (GameObject key in _keys)
                {
                    key.SetActive(_inventoryState);
                }
            }

            isRotated = !isRotated;
        }
    }

    private void GVControll()
    {
        
    }



    private IEnumerator RotateCamera(Quaternion fromRotation, Quaternion toRotation)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 2.0f)
        {
            transform.rotation = Quaternion.Slerp(fromRotation, toRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
            transform.rotation = toRotation;
        }

        if (!isRotated)
        {
            //_firstPersonController.enabled = true;
            //_cam.enabled = true;
            //_cmb.enabled = true;
            StopPlayerMovement.Instance.PlayerStop();
        }
         // Asegura la rotación final
        rotationAngle = 0;
    }

    private float DistanceBetweenAngles(Vector3 _d1, Vector3 _d2)
    {
        // Dirección hacia el objeto (diferencia en altura, solo queremos rotar hacia arriba o abajo)
        Vector3 _directionToObject = _d2 - _d1;

        // Calculamos el ángulo en el eje X (rotación hacia arriba o hacia abajo)
        // Proyectamos en el plano YZ para calcular la diferencia vertical
        float _angle = Vector3.SignedAngle(Vector3.forward, _directionToObject.normalized, Vector3.right);

        // Solo nos interesa la rotación hacia abajo (ángulo positivo), puedes ajustar el signo si es necesario
        return _angle;
    }


    private void InventoryPlus(GameObject key)
    {
        _keys.Add(key);
    }
}