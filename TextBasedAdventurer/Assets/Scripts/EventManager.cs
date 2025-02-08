using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EventManager : MonoBehaviour
{
    public TextManager textManager;
    public Sprite[] sprites;
    public Image image;
    public GameObject[] gameObjectsToDissapear;
    public TextMeshProUGUI endingText;
    public Animator caveKidPanelAnimator;
    public Animator buttonParentAnimator;
    public Animator endingTextAnimator;

    public void ExecuteEvent(string eventName){

        string[] strings = eventName.Split('/');

        switch (strings[0])
        {
            case "sprite":
                ChangeToSprite(strings[1]);
                break;

            case "addItem":
                AddItem(strings[1]);
                break;

            case "health":
                ChangeHealth(strings[1]);
                break;

            case "end":
                Ending(strings[1]);
                break;

            default: break;
        }
    }

    private void ChangeToSprite(string sprite)
    {
        foreach (Sprite s in sprites)
        {
            if (s.name == sprite)
            {
                image.sprite = s;
            }
        }
        // Play damage sound
        Debug.Log("Sprite activo " + sprite);
    }

    private void AddItem(string item)
    {
        PlayerManager.instance.AddItem(item);
        Debug.Log("Item " + item + " añadido al inventario del jugador");
    }

    private void ChangeHealth(string health)
    {
        PlayerManager.instance.ChangeHealth(int.Parse(health));
        switch (PlayerManager.instance.GetHealth())
        {
            case 100:
                ChangeToSprite("CaveKid2");
                break;
            case 80:
                ChangeToSprite("CaveKid3");
                break;
            case 60:
                ChangeToSprite("CaveKid4");
                break;
            case 40:
                ChangeToSprite("CaveKid5");
                break;
            case 20:
                ChangeToSprite("CaveKid6");
                break;
            case 0:
                ChangeToSprite("CaveKid10");
                Ending("Dead");
                break;
            default : break;
        }
        Debug.Log("Player health: " + PlayerManager.instance.GetHealth());
    }

    private void Ending(string end)
    {
        Debug.Log("End of game." + " You finished with ending: " + end);
        switch (end)
        {
            case "Dead":
                // Stop music and effects and play corresponding sound
                // Hide AdventureText, responses and BackToMainMenu button and animate Kid Panel with corresponding ending (Panel moves to the center of the screen)
                for (int i = 0; i < gameObjectsToDissapear.Length; i++)
                {
                    gameObjectsToDissapear[i].SetActive(false);
                    Debug.Log(gameObjectsToDissapear[i]);
                }
                caveKidPanelAnimator.SetBool("isEnding", true);
                buttonParentAnimator.SetBool("isClosed", true);
                // Animation Cave Kid eye falls + eye fall sound
                // Show THE END text under Kid Panel
                endingTextAnimator.SetBool("isOpen", true);
                StartCoroutine(ShowUIAfterTextCoroutine());
                break;
            default: break;
        }
    }

    private IEnumerator ShowUIAfterTextCoroutine()
    {
        // Type ending text
        yield return StartCoroutine(textManager.TypeEndText(endingText));
        // Show BackToMainMenu button
        gameObjectsToDissapear[1].SetActive(true);
    }

    private IEnumerator animateDeath(string spriteName)
    {

        yield return new WaitForSeconds(0.5f);
    }
}