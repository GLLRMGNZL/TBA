using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;
    public TMP_Dropdown languageDropdown;

    void Start()
    {
        masterSlider.value = PlayerManager.instance.GetMasterVolume();
        musicSlider.value = PlayerManager.instance.GetMusicVolume();
        effectsSlider.value = PlayerManager.instance.GetEffectsVolume();

        if (PlayerManager.instance.language == "spanish")
        {
            languageDropdown.value = 0;
        }
        if (PlayerManager.instance.language == "english")
        {
            languageDropdown.value = 1;
        }
    }
}