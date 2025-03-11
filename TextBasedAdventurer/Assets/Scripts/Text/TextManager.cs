using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class TextManager : MonoBehaviour
{

    /*#region Singleton
    public static TextManager instance { get; private set; }
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;

            sentences = new Queue<string>();
            dialogueText.text = "";
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion*/

    public Dialogue[] dialogues;
    public Dialogue currentDialogue;
    public TextMeshProUGUI dialogueText;
    public ScrollRect scrollRect;
    public float typeSpeed = 0.5f;
    public float scrollSpeed = 1f;
    public GameObject responseButtonPrefab;
    public GameObject textView;
    public GameObject theEndText;
    public Transform responseButtonContainer;
    public EventManager eventManager;
    private string fullText;
    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;

    private void OnEnable()
    {
        sentences = new Queue<string>();
        dialogueText.text = "";
        string lang = PlayerManager.instance.language;
        Debug.Log(lang);

        if (lang == "spanish")
        {
            currentDialogue = Resources.Load<Dialogue>("Texts/AdventureSpanish");

        }
        if (lang == "english")
        {
            currentDialogue = Resources.Load<Dialogue>("Texts/AdventureEnglish");
        }

        StartDialogue(currentDialogue.rootNode);
    }

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
        string fullText = node.dialogueText;
        string previousText = dialogueText.text; // Guarda el texto anterior para mantenerlo opaco
        string visible = "";
        string invisible = fullText;

        for (int i = 0; i < fullText.Length; i++)
        {
            // Construye el texto con el efecto de transparencia
            visible += fullText[i];
            invisible = invisible.Substring(1);

            dialogueText.text = $"{previousText}<color=#FFFFFF>{visible}</color><color=#00000000>{invisible}</color>"; // Mantiene el texto anterior opaco

            AudioManager.instance.Play("TypeCharacter"); // Reproduce el sonido de tipeo

            Canvas.ForceUpdateCanvases();
            if (scrollRect.verticalNormalizedPosition > 0f)
            {
                Debug.Log(scrollRect.verticalNormalizedPosition);
                scrollRect.verticalNormalizedPosition -= ((scrollSpeed * scrollRect.verticalNormalizedPosition) * 0.6f) * Time.deltaTime;
            }

            yield return new WaitForSeconds(typeSpeed);

            // Opción para saltar la animación
            if (!isTyping || !gameObject.activeSelf)
            {
                dialogueText.text = previousText + fullText;
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
                break;
            }
        }
        isTyping = false;
        scrollRect.verticalNormalizedPosition = 0f;
        GenerateResponseButtons(node);
    }

    public IEnumerator TypeEndText(TextMeshProUGUI endText)
    {
        string textToWrite = endText.text;
        Debug.Log(textToWrite);
        endText.text = "";

        foreach (char character in textToWrite)
        {
            endText.text += character;
            AudioManager.instance.Play("TypeCharacter");
            yield return new WaitForSeconds(typeSpeed);
        }
        theEndText.SetActive(true);
    }
}