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

    public Animator transition;

    // Change scene
    public void ReturnToMenu()
    {
        Debug.Log("ReturnToMenu called");
        StartCoroutine(LoadLevel(0));
    }
    public void NewGame()
    {
        Debug.Log("NewGame called");
        StartCoroutine(LoadLevel(1));
    }

    // Transition between scenes
    IEnumerator LoadLevel(int level)
    {
        Debug.Log("LoadLevel" + level + " called");
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(level);
    }

    // Full Screen
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}