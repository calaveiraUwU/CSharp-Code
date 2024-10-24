using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

public class NoteBeheavour : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject notePositions;
    [SerializeField] private GameObject noteUI;
    [SerializeField] private List<string> notes = new List<string>();
    [SerializeField] private GameObject blur;

    private List<Transform> _notePositions = new List<Transform>();
    private TMP_Text _tmpText;
    private int _index = 0;
    public StarterAssetsInputs _inputs;
    private void Start()
    {
        _inputs = FindAnyObjectByType<StarterAssetsInputs>();
        foreach(Transform t in notePositions.transform)
            _notePositions.Add(t);
        _tmpText = noteUI.transform.GetChild(0).GetComponent<TMP_Text>();
        this.transform.position = _notePositions[_index].position;
        this.transform.rotation = _notePositions[_index].rotation;
        _tmpText.text = LanguageManager.Instance.currentLanguage.noteTexts.notes[_index];
        noteUI?.SetActive(false);
        blur?.SetActive(false);
    }
    private void OnEnable()
    {
        LanguageManager.CLHC += ChangeText;

    }
    private void OnDisable()
    {
        LanguageManager.CLHC -= ChangeText;
    }





    public void ClickInteract(ref bool drawer)
    {
    }
    
    public void EInteract()
    {
        StopPlayerMovement.Instance.PlayerStop();
        _inputs.cursorLocked = false;
        _inputs.cursorInputForLook = false;
        Cursor.lockState = CursorLockMode.Confined;
        blur.SetActive(true);
        noteUI?.SetActive(true);



        if (_index < _notePositions.Count - 1)
        {
            _tmpText.text = LanguageManager.Instance.currentLanguage.noteTexts.notes[_index];
            _index++;
            this.transform.position = _notePositions[_index].position;
            this.transform.rotation = _notePositions[_index].rotation;

        }
        else
            this.gameObject.SetActive(false);
        
        
    }

    private void ChangeText()
    {
        _tmpText.text = LanguageManager.Instance.currentLanguage.noteTexts.notes[_index];
    }


    public void CloseNote()
    {
        noteUI?.SetActive(false);
        blur?.SetActive(false);
        _inputs.cursorLocked = true;
        _inputs.cursorInputForLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        

        StopPlayerMovement.Instance.PlayerStop();
    }

    
}
