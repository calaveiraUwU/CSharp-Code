using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour, IInteractable
{
    [field: SerializeField]public string nameReference { get; private set; }
    //[SerializeField] private GameObject _handReference;
    //[SerializeField] private string _handPointName;
    //[SerializeField] private List<GameObject> _handPoints = new List<GameObject>();
    public delegate void KeyCollected(GameObject key);
    public static event KeyCollected OnCollected;
    private void Awake()
    {
        //_handReference = GlobalProperties.Instance.handReference;
        //_handPointName = GlobalProperties.Instance.handPointName;
        //_handPoints = GlobalProperties.Instance.handPoints;
        
    }
    //private void OnEnable()
    //{
    //    if(transform.parent.gameObject.name.ToLower() != _handReference.name.ToLower())
    //    {
    //        foreach (Transform p in _handReference.transform)
    //        {
    //            if (p.gameObject.name.ToLower().Contains(_handPointName.ToLower()))
    //                _handPoints.Add(p.gameObject);
    //        }
    //    }
        
    //}

    public void ClickInteract(ref bool drawer)
    {
        //es una llave no hay que arrastar nada
    }

    public void EInteract()
    {
        foreach (Transform p in GlobalProperties.Instance.handReference.transform)
        {
            if (p.gameObject.name.ToLower().Contains(GlobalProperties.Instance.handPointName.ToLower()))
            {
                if (p.childCount <= 0)
                {
                    transform.localPosition = new Vector3(0f, 0f, 0f);
                    gameObject.transform.parent = p.transform;
                    transform.localPosition = new Vector3(0f, 0f, 0f);
                    gameObject.layer = 0;
                    OnCollected.Invoke(this.gameObject);
                    gameObject.SetActive(false);
                    return;
                }
                    
            }
        }
    }
}
