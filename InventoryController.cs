using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _keys = new List<GameObject>();
    [SerializeField] private bool _inventoryState = false;
    [SerializeField] FirstPersonController _firstPersonController;
    [SerializeField] private GameObject _objective, _mainCamera, _rootCamera, _inventoryCamera, _originalMainCamera;
    [SerializeField] AnimatorController _ac;
    [SerializeField] private float _lerpSpeed = 0.5f;
    [SerializeField] private Quaternion _cameraStarterPointOnOpenInventory;
    private Quaternion rotacionOriginal;
    //private Action onComplete();
    public delegate void setVelocityAndSensivility(float _vel, float sens);
    public static event setVelocityAndSensivility SVAS;
    public delegate void resVelocityAndSensivility();
    public static event resVelocityAndSensivility RVAS;
    [SerializeField] CinemachineBrain _cmb = null;
    private void OnEnable()
    {
        _ac = GetComponent<AnimatorController>();
        AnimatorController.OnFinish += ReactivateFPC;
        KeyController.OnCollected += InventoryPlus;
        _originalMainCamera = Camera.main.gameObject;
        //rotacionOriginal = _originalMainCamera.transform.rotation;
        rotacionOriginal = _objective.transform.rotation;
        if (_cmb == null)
            _cmb = FindFirstObjectByType<CinemachineBrain>();
    }

    private void OnDisable()
    {
        _ac = null;
        AnimatorController.OnFinish -= ReactivateFPC;
        KeyController.OnCollected -= InventoryPlus;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        //{
        //    InventoryBeheavour();
        //}
    }


    private void InventoryPlus(GameObject key)
    {
        _keys.Add(key);
    }

    private void ReactivateFPC()
    {
        _firstPersonController.enabled = true;
    }
    private void InventoryBeheavour()
    {
        _inventoryState = !_inventoryState;

        //_firstPersonController.enabled = !_inventoryState;
        if (_inventoryState)
        {
            _cameraStarterPointOnOpenInventory = _inventoryCamera.transform.rotation;
            _objective.transform.rotation.SetLookRotation(new Vector3(_objective.transform.rotation.x, _originalMainCamera.transform.rotation.y, _objective.transform.rotation.z));
            

            Debug.Log("On start animation" + _cameraStarterPointOnOpenInventory);
            //Debug.LogError("Abre el inventario: " + _objective.transform.rotation.ToString() + " " + _originalMainCamera.transform.rotation.ToString());
            //_firstPersonController.enabled = false;
            //_mainCamera.SetActive(false);
            StopPlayerMovement.Instance.PlayerStop();
            StartCoroutine(RotarHaciaObjetivo());
        }
        else
        {
            //Coroutine rtsp =
            StartCoroutine(RotarHaciaRotacionInicial());
            Debug.Log("On end animation" + _cameraStarterPointOnOpenInventory);
            //Coroutine tsr = StartCoroutine(RotarHaciaObjetivo(rotacionOriginal.eulerAngles));
            _originalMainCamera.transform.rotation = _objective.transform.rotation;
            //Debug.LogError("Cierra el inventario: " + _objective.transform.rotation.ToString() + " " + _originalMainCamera.transform.rotation.ToString());
        }

        _ac.Inventory(_inventoryState);
        if (_keys.Count > 0)
        {
            foreach (GameObject key in _keys)
            {
                key.SetActive(_inventoryState);
            }
        }
    }

    IEnumerator RotarHaciaObjetivo()
    {

        _cmb.enabled = false;
        //SVAS.Invoke(0, 0);
        // Determina la dirección en la que la cámara debe mirar
        Vector3 direccionObjetivo = _objective.transform.position - _inventoryCamera.transform.position;

        // Calcula la rotación objetivo usando Quaternion.LookRotation
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionObjetivo);

        // Mientras la cámara no haya llegado a la rotación objetivo
        while (Quaternion.Angle(_inventoryCamera.transform.rotation, rotacionObjetivo) > 0.1f)
        {
            // Interpola la rotación actual hacia la rotación objetivo
            _inventoryCamera.transform.rotation = Quaternion.Lerp(_inventoryCamera.transform.rotation, rotacionObjetivo, Time.deltaTime * _lerpSpeed);

            // Espera hasta el siguiente frame
            yield return null;
            if (Quaternion.Angle(_inventoryCamera.transform.rotation, rotacionObjetivo) <= 0.1f)
                _inventoryCamera.transform.rotation = rotacionObjetivo;

        }

        // Asegúrate de que la rotación final sea exactamente la rotación objetivo
        yield return null;

    }
    IEnumerator RotarHaciaCamara()
    {

        _cmb.enabled = false;
        //SVAS.Invoke(0, 0);
        // Determina la dirección en la que la cámara debe mirar
        Vector3 direccionObjetivo = _cameraStarterPointOnOpenInventory.eulerAngles - _inventoryCamera.transform.position;

        // Calcula la rotación objetivo usando Quaternion.LookRotation
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionObjetivo);

        // Mientras la cámara no haya llegado a la rotación objetivo
        while (Quaternion.Angle(_inventoryCamera.transform.rotation, rotacionObjetivo) > 0.1f)
        {
            // Interpola la rotación actual hacia la rotación objetivo
            _inventoryCamera.transform.rotation = Quaternion.Lerp(_inventoryCamera.transform.rotation, rotacionObjetivo, -Time.deltaTime * (-_lerpSpeed) );

            // Espera hasta el siguiente frame
            yield return null;
            if (Quaternion.Angle(_inventoryCamera.transform.rotation, rotacionObjetivo) <= 0.1f)
                _inventoryCamera.transform.rotation = rotacionObjetivo;

        }

        // Asegúrate de que la rotación final sea exactamente la rotación objetivo
        yield return null;

    }

    IEnumerator RotarHaciaRotacionInicial()
    {
        // Guarda la rotación inicial de la cámara (solo el ángulo en el eje X)
        Vector3 rotacionInicial = _cameraStarterPointOnOpenInventory.eulerAngles;

        Debug.Log(Mathf.Abs(_inventoryCamera.transform.eulerAngles.x - rotacionInicial.x) > 0.001f);

        // Mientras la cámara no haya alcanzado la rotación inicial en el eje X
        while (Mathf.Abs(_inventoryCamera.transform.eulerAngles.x - rotacionInicial.x) > 0.5f)
        {
            Debug.Log("Entra en la rotación hacia arriba");

            // Interpola suavemente solo la rotación en el eje X
            Vector3 nuevaRotacion = new Vector3(
                Mathf.LerpAngle(_inventoryCamera.transform.eulerAngles.x, rotacionInicial.x, Time.deltaTime * _lerpSpeed),
                _inventoryCamera.transform.eulerAngles.y, // Mantiene la rotación Y actual
                _inventoryCamera.transform.eulerAngles.z  // Mantiene la rotación Z actual
            );

            _inventoryCamera.transform.eulerAngles = nuevaRotacion;

            yield return null;
        }

        // Asegúrate de que la rotación final sea exactamente la rotación inicial
        _inventoryCamera.transform.eulerAngles = rotacionInicial;

        // Activa los componentes deseados después de la rotación
        _mainCamera.SetActive(true);
        //_firstPersonController.enabled = true;
        //_cmb.enabled = true;
        StopPlayerMovement.Instance.PlayerStop();
    }


    //IEnumerator RotarHaciaRotacionInicial()
    //{
    //    // Guarda la rotación inicial de la cámara antes de la acción
    //    Quaternion rotacionInicial = _cameraStarterPointOnOpenInventory.eulerAngles;


    //    Debug.Log(Quaternion.Angle(_inventoryCamera.transform.rotation, rotacionInicial) > 0.001f);
    //    // Mientras la cámara no haya alcanzado la rotación inicial
    //    while (Quaternion.Angle(_inventoryCamera.transform.rotation, rotacionInicial) > 0.5f)
    //    {
    //        Debug.Log("Entra en la subida de camara");
    //        // Interpola suavemente entre la rotación actual y la rotación inicial
    //        _inventoryCamera.transform.rotation = Quaternion.Slerp(
    //            _inventoryCamera.transform.rotation,
    //            rotacionInicial,
    //            Time.deltaTime * -_lerpSpeed
    //        );

    //        yield return null;
    //    }

    //    // Asegúrate de que la rotación final sea exactamente la rotación inicial
    //    _inventoryCamera.transform.rotation = rotacionInicial;

    //    // Activa los componentes deseados después de la rotación
    //    _mainCamera.SetActive(true);
    //    _firstPersonController.enabled = true;
    //    _cmb.enabled = true;
    //}



    IEnumerator RotarHaciaObjetivo(Vector3 _posicionObjetivo)
    {
        // Dirección hacia el objetivo (arreglado)
        Vector3 direccionObjetivo = _posicionObjetivo - _inventoryCamera.transform.position;

        // Crear la rotación objetivo con un vector "up" explícito
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionObjetivo, Vector3.up);

        // Mientras la cámara no haya alcanzado la rotación objetivo
        while (Quaternion.Angle(_inventoryCamera.transform.rotation, rotacionObjetivo) > 0.01f)
        {
            // Interpolamos suavemente entre la rotación actual y la rotación objetivo
            _inventoryCamera.transform.rotation = Quaternion.Slerp(
                _inventoryCamera.transform.rotation,
                rotacionObjetivo,
                Time.deltaTime * _lerpSpeed
            );

            yield return null;
        }

        // Asegúrate de que la rotación final sea exactamente la rotación objetivo
        _inventoryCamera.transform.rotation = rotacionObjetivo;

        // Activamos la cámara principal y el controlador
        _mainCamera.SetActive(true);
        _firstPersonController.enabled = true;
        _cmb.enabled = true;
    }


    //IEnumerator RotarHaciaObjetivo(Vector3 _posicionObjetivo)
    //{
    //    //_rotacionObjetivo = new Quaternion(_inventoryCamera.transform.rotation.x, _objective.transform)
    //    // Mientras la cámara no haya alcanzado la rotación objetivo
    //    Quaternion _pos = new Quaternion(_posicionObjetivo.x, _posicionObjetivo.y, _posicionObjetivo.z, gameObject.transform.rotation.w);
    //    Vector3 _Objectiverotation = _posicionObjetivo - _inventoryCamera.transform.position;
    //    while (Quaternion.Angle(_inventoryCamera.transform.rotation, _pos ) > 0.1f)
    //    {
    //        // Interpola la rotación actual hacia la rotación objetivo
    //        //_inventoryCamera.transform.rotation = Quaternion.Lerp(_inventoryCamera.transform.rotation, _rotacionObjetivo, Time.deltaTime * _lerpSpeed);
    //        Vector3 _newPositionForRestartTheCamera = new Vector3(_Objectiverotation.x,(Mathf.Lerp(_inventoryCamera.transform.rotation.y, _Objectiverotation.y, Time.deltaTime * _lerpSpeed)), _posicionObjetivo.z);
    //        _inventoryCamera.transform.rotation = new Quaternion(_newPositionForRestartTheCamera.x, _newPositionForRestartTheCamera.y, _newPositionForRestartTheCamera.z, _inventoryCamera.transform.rotation.w);
    //        yield return null;
    //        Quaternion _mangante = new Quaternion(_newPositionForRestartTheCamera.x, _newPositionForRestartTheCamera.y, _newPositionForRestartTheCamera.z, gameObject.transform.rotation.w);
    //        if(Quaternion.Angle(_inventoryCamera.transform.rotation, _mangante) <= 0.15)
    //        {
    //            Debug.Log("Ha entrado en setPositions");
    //            _mainCamera.SetActive(true);
    //            _firstPersonController.enabled = true;
    //            //yield return new WaitForSeconds(0.2f);

    //            _cmb.enabled = true;
    //            Debug.Log("Ha salido de setPositions");
    //        }
    //        //// Espera hasta el siguiente frame
    //        //if(Quaternion.Angle(_camera.transform.rotation, rotacionObjetivo) > 0.15f)
    //        //    _firstPersonController.enabled = true;


    //        // --> RVAS.Invoke();
    //        // Llamada al callback cuando la corutina finaliza
    //        //onComplete?.Invoke();

    //    }


    // Asegúrate de que la rotación final sea exactamente la rotación objetivo
    //_rootCamera.transform.rotation = rotacionObjetivo;

    //_inventoryCamera.SetActive(false);

    //yield return null;
}

