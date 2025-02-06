using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class TextManager : MonoBehaviour
{

    #region Singleton
    public static TextManager Instance {  get; private set; }
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            sentences = new Queue<string>();
            //fullText = currentDialogue.rootNode.dialogueText;
            dialogueText.text = "";
            StartDialogue(currentDialogue.rootNode);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public Dialogue currentDialogue;
    public TextMeshProUGUI dialogueText;
    public ScrollRect scrollRect;
    public float typeSpeed = 0.5f;
    public GameObject responseButtonPrefab;
    public Transform responseButtonContainer;
    public EventManager eventManager;
    private string fullText;
    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;

    private void Update()
    {
        if (isTyping && Input.anyKeyDown)
        {
            // Show full text
            dialogueText.text = fullText;
            isTyping = false;
        }
    }

    public void StartDialogue(DialogueNode node)
    {
        // Set dialogue title and body dialogueText
        fullText = node.dialogueText;
        StartCoroutine(TypeText(node));

        // Remove any existing response buttons
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void SelectResponse(DialogueResponse response)
    {
        // Execute Event if existing
        if (response.associatedEvents.Length > 0)
        {
            for (int i = 0; i < response.associatedEvents.Length; i++)
            {
                eventManager.ExecuteEvent(response.associatedEvents[i]);
            }
        }

        DialogueNode nextNode = GetDialogueNodeById(response.nextNodeId);

        // Continue to next node if exists
        if (nextNode != null && !nextNode.IsLastNode())
        {
            Debug.Log("nextNode responses --> " + nextNode.responses.Count());
            StartDialogue(nextNode);
        }
    }

    // Search next node by id. It avoids recursive serialization
    private DialogueNode GetDialogueNodeById(string id)
    {
        if (currentDialogue == null)
        {
            Debug.LogError("currentDialogue no está asignado.");
            return null;
        }

        if (currentDialogue.dialogueNodes == null || currentDialogue.dialogueNodes.Count == 0)
        {
            Debug.Log("La lista dialogueNodes de currentDialogue no está inicializada o está vacía.");
            return null;
        }

        foreach (DialogueNode node in currentDialogue.dialogueNodes)
        {
            if (node.id == id)
            {
                return node;
            }
        }
        Debug.Log($"No se encontró un nodo con el ID: {id}");
        return null;
    }

    private void GenerateResponseButtons(DialogueNode node)
    {
        // Create and setup response buttons based on current dialogue node
        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            // Setup button to trigger SelectResponse when clicked
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response));
        }
    }

    // Coroutine writes text letter by letter
    IEnumerator TypeText(DialogueNode node)
    {
        isTyping = true;
        string previousText = dialogueText.text;
        foreach (char character in fullText)
        {
            dialogueText.text += character;
            AudioManager.instance.Play("TypeCharacter");
            yield return new WaitForSeconds(typeSpeed);

            // It ensures we always see last text written
            scrollRect.verticalNormalizedPosition = 0f;

            if (!isTyping)
            {
                dialogueText.text = previousText + fullText;
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
                break;
            }
        }
        if (isTyping) isTyping = false;
        GenerateResponseButtons(node);
    }
}