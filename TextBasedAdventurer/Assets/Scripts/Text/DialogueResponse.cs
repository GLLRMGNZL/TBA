using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public string nextNodeId; // Use an ID to reference the next node
    public string associatedEvent;
}