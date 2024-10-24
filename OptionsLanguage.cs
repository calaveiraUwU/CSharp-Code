using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Language/Options", fileName = " new Language")]
public class OptionsLanguage : LanguageBehavior
{
    public string controlsMenuName;
    public string controlsSensivilityName;
    public string soundMenuName;
    public string soundGeneralAudio;
    public string soundMusic;
    public string soundAudioEffects;
    public string graphicsMenuName;
    public string graphicsQuality;
    public List<string> graphicsQualityPresets = new List<string>();
}