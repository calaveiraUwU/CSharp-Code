using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    #region Singleton
    private static LanguageManager _instance;



    //EVENTS
    public delegate void currentLenguageHasChanged();
    public static event currentLenguageHasChanged CLHC;

    //BORRAR
    public GameObject languageSelector;
    private Dropdown dropdown;
    private bool _lockstate = true;

    public static LanguageManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LanguageManager>();
            return _instance;
        }
    }
    #endregion

    public List<LanguageBehavior> languages;
    public LanguageBehavior currentLanguage;


    private void Start()
    {
        currentLanguage = languages[0];
        dropdown = languageSelector.transform.GetChild(0).gameObject.GetComponent<Dropdown>();
        foreach (LanguageBehavior len in languages)
            dropdown.options.Add(new Dropdown.OptionData(len.name, null));

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_lockstate)
                StopPlayerMovement.Instance.UICursorEnable();
            else
                StopPlayerMovement.Instance.UICursorDisable();

            _lockstate = !_lockstate;
            StopPlayerMovement.Instance.PlayerStop();
            languageSelector.SetActive(!languageSelector.activeInHierarchy);
        }
            
    }


    public void ChangeLanguage()
    {
        for(int i=0; i<languages.Count; i++)
        {
            if (languages[i].name.Contains(dropdown.options[dropdown.value].text))
                currentLanguage = languages[i];
        }
        CLHC.Invoke();
    }

}
