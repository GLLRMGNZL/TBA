using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class EventManager : MonoBehaviour
{
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

            default: break;
        }
    }

    private void ChangeToSprite(string sprite)
    {
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
    }
}