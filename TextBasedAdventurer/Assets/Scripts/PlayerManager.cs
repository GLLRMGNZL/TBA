using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private List<string> inventory;
    private int health;

    private void Start()
    {
        inventory = new List<string>();
        health = 100;
    }

    public void AddItem(string item)
    {
        inventory.Add(item);
    }

    public void RemoveItem(string item)
    {
        inventory.Remove(item);
    }

    public void ChangeHealth(int num)
    {
        health += num;
    }

    public int GetHealth()
    {
        return health;
    }
}