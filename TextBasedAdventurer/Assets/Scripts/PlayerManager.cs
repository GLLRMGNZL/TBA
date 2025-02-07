using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    #region Singleton
    public static PlayerManager instance;

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


    // Game preferences
    private float masterVolume = 0f;
    private float musicVolume = 0f;
    private float effectsVolume = 0f;

    // Player mechanics
    private List<string> inventory;
    private int health;

    private void Start()
    {
        inventory = new List<string>();
        health = 120;
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

    public void SetMasterVolume(float num)
    {
        masterVolume = num;
    }

    public void SetMusicVolume(float num)
    {
        musicVolume = num;
    }

    public void SetEffectsVolume(float num)
    {
        effectsVolume = num;
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetEffectsVolume()
    {
        return effectsVolume;
    }
}