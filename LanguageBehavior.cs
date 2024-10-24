using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Language", fileName = " new Language")]
public class LanguageBehavior : ScriptableObject
{
    public NoteLanguage noteTexts;
    public MenuLanguage menuTexts;
    public HintLanguage hintTexts;
    public OptionsLanguage optionsTexts;
    public NarrationTextsLanguage narrationsTexts;
}
