using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Drawing;

enum EParagraph
{
    FIRST, LAST
}


public class ChangeColorText : MonoBehaviour
{
    private TMP_Text _text;
    public UnityEngine.Color SelectColor;
    [SerializeField]
    private EParagraph WichWord;


    private void OnEnable()
    {
        _text = GetComponent<TMP_Text>();
        ChangeColor();
    }

    public void ChangeColor()
    {
        string[] words = _text.text.Split(" ");
        switch (WichWord)
        {
            case EParagraph.FIRST:
                words[0] = "<color=#" + ColorUtility.ToHtmlStringRGBA(SelectColor) + ">" + words[0] + "</color>";
                break;
            case EParagraph.LAST:
                words[words.Length - 1] = "<color=#" + ColorUtility.ToHtmlStringRGBA(SelectColor) + ">" + words[words.Length -1] + "</color>";
                break;
            default:
                Debug.Log("You have not selected anything!");
                break;
        }
        _text.text = string.Join(" ", words);

    }
}