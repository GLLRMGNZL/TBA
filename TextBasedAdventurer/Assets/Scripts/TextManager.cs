using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public float typeSpeed = 0.5f;
    private string fullText;

    private void Start()
    {
        fullText = textMeshProUGUI.text;
        textMeshProUGUI.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char character in fullText)
        {
            textMeshProUGUI.text += character;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}