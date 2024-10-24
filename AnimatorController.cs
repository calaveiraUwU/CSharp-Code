using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class AnimatorController : MonoBehaviour
{
    //private Animator _anim;
    [SerializeField] private Animator _cameraAnimator;

    public delegate void FinishAnimation();
    public static event FinishAnimation OnFinish;
    //[SerializeField] private FirstPersonController _inputs;
    private void OnEnable()
    {   
        //_anim = GetComponent<Animator>();
         FirstPersonController.OnMovementChange += HandleMovementChange;
    }

    void OnDisable()
    {
        // Desuscribirse del evento para evitar errores
        FirstPersonController.OnMovementChange -= HandleMovementChange;
    }

    // Método que maneja el cambio de movimiento
    void HandleMovementChange(bool isMoving)
    {
        //_anim.SetBool("isMoving", isMoving);
    }

    public void Inventory(bool state)
    {

        //_anim.speed = 1;
        //_cameraAnimator.speed = 1;
        //_anim.Play

        if (!state)
        {
            //_anim.Play("Inventory", 0, -1);



            //_cameraAnimator.Play("CameraInventory", 0, 1);
            //_anim.speed = -1;
            //_cameraAnimator.speed = -1;


            //anim.SetBool("inventoryFinish", true);


            //_cameraAnimator.SetBool("inventoryFinish", true);
            //OnFinish.Invoke();
        }
        else
        {
            //_anim.SetBool("inventoryFinish", false);


            //_anim.Play("Inventory", 0, 1);



        }
        
        

        
    }
}
