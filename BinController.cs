using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _staticBin, _playerBin, rfp;
    [SerializeField] private bool _playerBinIsCurrentActive = false;
    [SerializeField] private List<GameObject> _binPositions = new List<GameObject>();
    [SerializeField, Range(1,0)]private float _binSpawnPointController = 0;
    [SerializeField] private Vector3 _dimensions;
    private BoxCollider _spawnZone;
    private MeshRenderer _sMR, _pMR;
    private GameObject _recycleBin;
    private float heightLastObject = 0;

    //EVENTS
    public delegate void binState(bool _state);
    public static event binState BS;
    

    private void OnEnable()
    {
        TrashController.STOM += SetObjectInsideTheTrash;
    }
    private void OnDisable()
    {
        TrashController.STOM -= SetObjectInsideTheTrash;
    }

    void Start()
    {
        _recycleBin = _staticBin.transform.GetChild(0).gameObject;

        _recycleBin.transform.parent = _staticBin.transform;

        _spawnZone = rfp.GetComponent<BoxCollider>();

    }

    void OnDrawGizmosSelected()
    {

#if UNITY_EDITOR

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(rfp.transform.position, _dimensions);

      
#endif
    }

    public void SetObjectInsideTheTrash(GameObject _obj)
    {
        _obj.transform.parent = _recycleBin.transform;
        _obj.transform.position = Vector3.zero;
        _obj.transform.position = GenerarPosicionAleatoriaDentroDeCaja();
        _obj.GetComponent<BoxCollider>().enabled = false;
        heightLastObject = _obj.transform.localScale.y;
        _binSpawnPointController += heightLastObject;
    }
    private Vector3 GetRandomPointInSphere()
    {
        //Vector3 boxSize = _spawnZone.size;

        //// Calculamos el centro del BoxCollider en el espacio mundial
        //Vector3 boxCenter = rfp.transform.TransformPoint(_spawnZone.center);
        //float uwu = 2;
        //// Generamos un punto aleatorio dentro del volumen del BoxCollider (en coordenadas locales)
        //Vector3 localRandomPoint = new Vector3(
        //    Random.Range(boxSize.x / uwu, -boxSize.x / uwu),  // Coordenada X local
        //    Mathf.Min(_binSpawnPointController * boxSize.y, -boxSize.y) - boxSize.y / uwu, // Coordenada Y local (controlada por la altura)
        //    Random.Range(boxSize.z / uwu, -boxSize.z / uwu)   // Coordenada Z local
        //);

        //// Convertimos el punto local en un punto mundial, teniendo en cuenta la rotación del BoxCollider
        //Vector3 worldRandomPoint = rfp.transform.TransformPoint(localRandomPoint);
        Vector3 rndPosWithin;

        //rndPosWithin = new Vector3(Random.Range(-_spawnZone.size.x, _spawnZone.size.x), Random.Range(-_spawnZone.size.y, _spawnZone.size.y), Random.Range(-_spawnZone.size.z, _spawnZone.size.z));
        rndPosWithin= new Vector3((Random.value - 0.5f) * _spawnZone.size.x,(Random.value - 0.5f) * _spawnZone.size.y,(Random.value - 0.5f) * _spawnZone.size.z);
        //rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);

        //return worldRandomPoint;
        return _spawnZone.center + rndPosWithin;
    }

    Vector3 GenerarPosicionAleatoriaDentroDeCaja()
    {
        //float x = Random.Range(_spawnZone.transform.position.x - _dimensions.x / 2, _spawnZone.transform.position.x + _dimensions.x / 2);
        //float z = Random.Range(_spawnZone.transform.position.z - _dimensions.y / 2, _spawnZone.transform.position.z + _dimensions.y / 2);


        float x = Random.Range(_spawnZone.transform.position.x - _dimensions.x / 2, _spawnZone.transform.position.x + _dimensions.x / 2);
        float y = Random.Range(_spawnZone.transform.position.y - (_dimensions.y + _binSpawnPointController) / 2, _spawnZone.transform.position.y + (_dimensions.y - _binSpawnPointController) / 2);
        float z = Random.Range(_spawnZone.transform.position.z - _dimensions.z / 2, _spawnZone.transform.position.z + _dimensions.z / 2);
        // Mantener la altura (y) de la caja o del objeto
        //float y = _spawnZone.transform.position.y;
        return _spawnZone.center + new Vector3(x, y, z);
    }

    public void EInteract()
    {
        _playerBinIsCurrentActive = !_playerBinIsCurrentActive;
        BS?.Invoke(_playerBinIsCurrentActive);
        _recycleBin.transform.parent = _binPositions[_playerBinIsCurrentActive ? 1 : 0].transform;
        _recycleBin.transform.localPosition = Vector3.zero;
    }


    public void ClickInteract(ref bool drawer)
    {
        throw new System.NotImplementedException();
    }
}
