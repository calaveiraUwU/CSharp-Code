using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


interface IInteractable
{
    //bool _isDragging { get; set; }
    public void EInteract();
    public void ClickInteract(ref bool drawer);
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform InteractorSource;
    [SerializeField] private float InteractRange;
    [SerializeField] private GameObject _scopePlayer;
    [SerializeField] private LayerMask _layerMask;
    private IInteractable _interactable;
    [SerializeField] private static bool _isClicking;
    void Start()
    {
    }
    private void Update()
    {
        // Realizar el raycast una sola vez y usar el resultado
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        bool hitSomething = Physics.Raycast(r, out RaycastHit hitInfo, InteractRange, _layerMask);
        IInteractable _rayResoult = null;

        if (hitSomething && hitInfo.collider.gameObject.TryGetComponent(out _rayResoult))
        {
            _scopePlayer.SetActive(true);
        }
        else
        {
            _scopePlayer.SetActive(false);
        }
        
        // Manejar la entrada del usuario
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.DrawRay(InteractorSource.position, InteractorSource.forward, Color.green, 2f);

            // Reutilizar el resultado del raycast
            if (hitSomething)
            {
                _rayResoult.EInteract();
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            _isClicking = true;
            if (hitSomething)
            {
                _rayResoult.ClickInteract(ref _isClicking);
                
                if(_rayResoult != null)
                    _interactable = _rayResoult;
            }
                
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isClicking = false;
            Debug.Log(_interactable);
            if(_interactable != null)
            {
                _interactable.ClickInteract(ref _isClicking);
                _interactable = null;
            }
                
        }
            
    }
}
