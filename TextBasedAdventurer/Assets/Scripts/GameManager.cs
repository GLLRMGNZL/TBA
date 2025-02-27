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
            Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
            Application.targetFrameRate = 60;
        }
        else
        {
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public Animator transition;
    public Texture2D cursorArrow;
    public Texture2D cursorArrowUpdate;
    public string language = "spanish";

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
        AudioManager.instance.Play("Theme");
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