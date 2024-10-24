using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HintBase
{
    [field: SerializeField] public string refName { get; private set; }
    [field: SerializeField] public string phrase { get; private set; }
}



[CreateAssetMenu(menuName = "Language/Hint", fileName = " new Language")]
public class HintLanguage : LanguageBehavior
{
    public string recycleBinHint;
    public List<HintBase> hints = new List<HintBase>();

    public HintBase FindHintByName(string name)
    {
        return hints.Find(hint => hint.refName == name);
    }
}
