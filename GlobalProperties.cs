using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalProperties : MonoBehaviour
{
    
    [field: SerializeField] public Transform facePoint { get; private set; }
    [field: SerializeField] public GameObject handReference { get; private set; }
    [field: SerializeField] public string handPointName { get; private set; }
    [field: SerializeField] public List<GameObject> handPoints { get; private set; } = new List<GameObject>();

    public static GlobalProperties Instance { get; private set; }

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

        #region KeyStarter
        //if (transform.parent.gameObject.name.ToLower() != handReference.name.ToLower())
        //{
            foreach (Transform p in handReference.transform)
            {
                if (p.gameObject.name.ToLower().Contains(handPointName.ToLower()))
                    handPoints.Add(p.gameObject);
            }
        //}
        #endregion
 
    }
}
