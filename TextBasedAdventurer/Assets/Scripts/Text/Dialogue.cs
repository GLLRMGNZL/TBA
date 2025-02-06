using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    [Header("Start of the adventure")]
    public DialogueNode rootNode; // First node of the conversation
    [Header("Adventure nodes")]
    public List<DialogueNode> dialogueNodes; // List of all dialogue nodes
}
