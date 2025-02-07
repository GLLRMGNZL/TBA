using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EventManager : MonoBehaviour
{
    public Sprite[] sprites;
    public Image image;

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
        Debug.Log("Player health: " + PlayerManager.instance.GetHealth());
    }

    private void Ending(string end)
    {
        Debug.Log("End of game." + " You finished with ending: " + end);
        // Stop music and effects and play corresponding sound
        // Hide AdventureText, responses and BackToMainMenu button and animate Kid Panel with corresponding ending (Panel moves to the center of the screen)
        // Show corresponding ending text above Kid Panel character by character
        // Show THE END text under Kid Panel
        // Show BackToMainMenu button
    }
}