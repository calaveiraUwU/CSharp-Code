using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Language/Note", fileName = " new Language")]
public class NoteLanguage : LanguageBehavior
{
    public List<string> notes = new List<string>();
}
