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
        if (PlayerManager.instance.GetMasterVolume() != 150f)
        {
            masterSlider.value = PlayerManager.instance.GetMasterVolume();
        }
        if (PlayerManager.instance.GetMusicVolume() != 150f)
        {
            musicSlider.value = PlayerManager.instance.GetMusicVolume();
        }
        if (PlayerManager.instance.GetEffectsVolume() != 150f)
        {
            effectsSlider.value = PlayerManager.instance.GetEffectsVolume();
        }
    }

}
