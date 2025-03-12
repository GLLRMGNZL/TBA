using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsLanguage : MonoBehaviour
{

    public TextMeshProUGUI newGame;
    public TextMeshProUGUI options;
    public TextMeshProUGUI exit;
    public TextMeshProUGUI optionsBack;
    public TextMeshProUGUI optionsText;
    public TextMeshProUGUI masterText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI effectsText;
    public TextMeshProUGUI fullScreen;

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instance.language == "spanish")
        {
            newGame.text = "Nueva partida";
            options.text = "Opciones";
            exit.text = "Salir";
            optionsBack.text = "Volver";
            optionsText.text = "Opciones";
            masterText.text = "Máster";
            musicText.text = "Música";
            effectsText.text = "Efectos";
            fullScreen.text = "Pantalla completa";
        }
        if (PlayerManager.instance.language == "english")
        {
            newGame.text = "New game";
            options.text = "Options";
            exit.text = "Exit";
            optionsBack.text = "Back";
            optionsText.text = "Options";
            masterText.text = "Master";
            musicText.text = "Music";
            effectsText.text = "Effects";
            fullScreen.text = "Full screen";
        }
    }
}
