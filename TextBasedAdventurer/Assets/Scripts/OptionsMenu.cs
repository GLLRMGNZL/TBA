using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;

    // Start is called before the first frame update
    void Start()
    {
        masterSlider.value = PlayerManager.instance.GetMasterVolume();
        musicSlider.value = PlayerManager.instance.GetMusicVolume();
        effectsSlider.value = PlayerManager.instance.GetEffectsVolume();
    }
}
