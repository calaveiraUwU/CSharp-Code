using System.Collections;
using UnityEngine;
using TMPro;

public class HintController : MonoBehaviour
{
    [SerializeField] private TMP_Text m_TextMeshPro;
    public static HintController Instance { get; private set; }

    private void Awake()
    {
        // Verificamos si ya existe una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Esto asegura que no se destruya al cargar nuevas escenas
        }
        else
        {
            Destroy(gameObject); // Si ya existe, destruimos este objeto
        }

        m_TextMeshPro = GetComponent<TMP_Text>();
    }

    public void UpdateText(string _newText, float seconds)
    {
        m_TextMeshPro.text = _newText;
        StartCoroutine(ShowText(seconds));
    }

    private IEnumerator ShowText(float _sec)
    {
        yield return new WaitForSeconds(_sec);
        m_TextMeshPro.text = "";
    }
}
