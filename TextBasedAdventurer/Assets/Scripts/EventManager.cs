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
    public Sprite[] deathSprites;
    public Sprite[] goodEndingSprites;
    public float deathAnimSpeed;
    public float goodEndingAnimSpeed;
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
                Ending("Dead");
                break;
            default : break;
        }
        Debug.Log("Player health: " + PlayerManager.instance.GetHealth());
    }

    private void Ending(string end)
    {
        Debug.Log("End of game. You finished with ending: " + end);
        switch (end)
        {
            case "Dead":
                UIEnding("dead");
                break;
            case "GoodEnd":
                UIEnding("goodEnding");
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

    private IEnumerator animateDeath()
    {
        foreach(Sprite s in deathSprites)
        {
            image.sprite = s;
            yield return new WaitForSeconds(deathAnimSpeed);
        }
    }

    private IEnumerator animateGoodEnding()
    {
        foreach (Sprite s in goodEndingSprites)
        {
            image.sprite = s;
            yield return new WaitForSeconds(deathAnimSpeed);
        }
    }

    private void UIEnding(string ending)
    {
        // Stop music and effects and play corresponding sound
        AudioManager.instance.Stop("Theme");
        // Hide AdventureText, responses and BackToMainMenu button and animate Kid Panel with corresponding ending (Panel moves to the center of the screen)
        for (int i = 0; i < gameObjectsToDissapear.Length; i++)
        {
            gameObjectsToDissapear[i].SetActive(false);
            Debug.Log(gameObjectsToDissapear[i]);
        }
        // Animation Cave Kid eye falls + eye fall sound
        if (ending == "dead")
        {
            // Play death theme
            AudioManager.instance.Play("Death");
            // TODO: endingText = death Text
            endingText.text = "Texto predeterminado para final en el que Cave Kid muere.";
            // Show death animation
            StartCoroutine(animateDeath());
        }
        if (ending == "goodEnding") 
        {
            // Play good ending theme
            AudioManager.instance.Play("GoodEnding");
            // TODO: endingText = good Ending Text
            endingText.text = "Texto predeterminado para final en el que Cave Kid logra escapar.";
            // Show good ending animation
            StartCoroutine(animateGoodEnding()); 
        }
        caveKidPanelAnimator.SetBool("isEnding", true);
        buttonParentAnimator.SetBool("isClosed", true);
        // Show THE END text under Kid Panel
        endingTextAnimator.SetBool("isOpen", true);
        StartCoroutine(ShowUIAfterTextCoroutine());
    }
}