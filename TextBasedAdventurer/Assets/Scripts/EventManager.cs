using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public TextManager textManager;
    public Sprite[] sprites;
    public Sprite[] deathSprites;
    public Sprite[] goodEndingSprites;
    public float[] goodEndingDelays;
    public string[] goodEndingSounds;
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
        AudioManager.instance.Play("damage");
        Debug.Log("Sprite activo " + sprite);
    }

    private void AddItem(string item)
    {
        PlayerManager.instance.AddItem(item);
        AudioManager.instance.Play("success");
        Debug.Log("Item " + item + " añadido al inventario del jugador");
    }

    private void ChangeHealth(string health)
    {
        PlayerManager.instance.ChangeHealth(int.Parse(health));
        switch (PlayerManager.instance.GetHealth())
        {
            case 100:
                ChangeToSprite("caveKid2");
                break;
            case 80:
                ChangeToSprite("caveKid3");
                break;
            case 60:
                ChangeToSprite("caveKid4");
                break;
            case 40:
                ChangeToSprite("caveKid5");
                break;
            case 20:
                ChangeToSprite("caveKid6");
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
        // Show THE END text under Kid Panel
        endingTextAnimator.SetBool("isOpen", true);
        StartCoroutine(ShowUIAfterTextCoroutine());
    }

    private IEnumerator animateGoodEnding()
    {
        for (int i = 0; i < goodEndingSprites.Length; i++)
        {
            image.sprite = goodEndingSprites[i];

            if (!string.IsNullOrEmpty(goodEndingSounds[i]))
            {
                AudioManager.instance.Play(goodEndingSounds[i]);
            }

            yield return new WaitForSeconds(goodEndingDelays[i]);
        }
        // Show THE END text under Kid Panel
        endingTextAnimator.SetBool("isOpen", true);
        StartCoroutine(ShowUIAfterTextCoroutine());
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
            // Play death theme and eye falls
            AudioManager.instance.Play("EyeFalls");
            AudioManager.instance.Stop("Theme");
            AudioManager.instance.Play("Death");

            // Set ending language
            if (PlayerManager.instance.language == "spanish")
            {
                textManager.endingText = "Texto predeterminado en el que Cave Kid muere.";
            }
            else if (PlayerManager.instance.language == "english")
            {
                textManager.endingText = "Predeterminated text in which Cave Kid dies.";
            }
            // Show death animation
            StartCoroutine(animateDeath());
        }
        if (ending == "goodEnding") 
        {
            // Play good ending theme
            AudioManager.instance.Play("Success");
            AudioManager.instance.Stop("Theme");
            AudioManager.instance.Play("GoodEnding");

            // Set ending language
            if (PlayerManager.instance.language == "spanish")
            {
                textManager.endingText = "Texto predeterminado en el que Cave Kid logra escapar.";
            } 
            else if (PlayerManager.instance.language == "english")
            {
                textManager.endingText = "Predeterminated text in which Cave Kid manages to scape.";
            }
            // Show good ending animation
            StartCoroutine(animateGoodEnding()); 
        }
        caveKidPanelAnimator.SetBool("isEnding", true);
        buttonParentAnimator.SetBool("isClosed", true);
    }
}