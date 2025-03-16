using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
            Application.targetFrameRate = 60;
        }
        else
        {
            Destroy(instance);
            return;
        }

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    public Animator transition;
    public Texture2D cursorArrow;
    public Texture2D cursorArrowUpdate;
    public TMP_Dropdown languageDropdown;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.SetCursor(cursorArrowUpdate, Vector2.zero, CursorMode.ForceSoftware);
            AudioManager.instance.Play("MouseClick");
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    // Change scene
    public void ReturnToMenu()
    {
        AudioManager.instance.Play("sound_stop");
        AudioManager.instance.Stop("game_music");
        AudioManager.instance.Play("Theme");
        Debug.Log("ReturnToMenu called");
        StartCoroutine(LoadLevel(0));
    }
    public void NewGame()
    {
        //Debug.Log("NewGame called");
        AudioManager.instance.Stop("Theme");
        AudioManager.instance.Play("sound_stop");
        AudioManager.instance.Play("game_music");
        StartCoroutine(LoadLevel(1));
    }

    // Transition between scenes
    IEnumerator LoadLevel(int level)
    {
        //Debug.Log("LoadLevel" + level + " called");
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(level);
    }

    // Full Screen
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void ChangeLanguage()
    {
        Debug.Log(languageDropdown.value);
        switch (languageDropdown.value)
        {
            case 0:
                PlayerManager.instance.language = "spanish";
                break;
            case 1:
                PlayerManager.instance.language = "english";
                break;
            default:
                break;
        }
    }
}