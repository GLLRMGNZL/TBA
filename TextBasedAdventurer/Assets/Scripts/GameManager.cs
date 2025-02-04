using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // Change scene
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    // Full Screen
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}