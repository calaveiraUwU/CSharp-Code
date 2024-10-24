using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour, IInteractable
{
    public delegate void setTrashOnBin(GameObject _obj);
    public static event setTrashOnBin STOM;

    private bool _binState = false;

    private void OnEnable()
    {
        BinController.BS += UpdateBinState;
    }
    private void OnDisable()
    {
        BinController.BS -= UpdateBinState;
    }

    private void UpdateBinState(bool _state)
    {
        _binState = _state;
    }



    public void ClickInteract(ref bool drawer)
    {
        
    }

    public void EInteract()
    {
        if(_binState)
            STOM.Invoke(this.gameObject);
        else
        {
            HintController.Instance.UpdateText(LanguageManager.Instance.currentLanguage.hintTexts.recycleBinHint, 3f);
        }


    }

}
