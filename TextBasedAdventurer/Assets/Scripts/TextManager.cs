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
            StartCoroutine(TypeText());
        }
        else
        {
            Destroy(gameObject);
        }

    }
    #endregion

    public Dialogue currentDialogue;
    public TextMeshProUGUI dialogueText;
    public float typeSpeed = 0.5f;
    public GameObject responseButtonPrefab;
    public Transform responseButtonContainer;
    private string fullText;
    private Queue<string> sentences = new Queue<string>();

    public void StartDialogue(DialogueNode node)
    {
        // Set dialogue title and body dialogueText
        fullText = node.dialogueText;
        StartCoroutine(TypeText());

        // Remove any existing response buttons
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create and setup response buttons based on current dialogue node
        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            // Setup button to trigger SelectResponse when clicked
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response));
        }
    }

    public void SelectResponse(DialogueResponse response)
    {

        DialogueNode nextNode = GetDialogueNodeById(response.nextNodeId);

        // Continuar al siguiente nodo si existe
        if (nextNode != null && !nextNode.IsLastNode())
        {
            Debug.Log("nextNode responses --> " + nextNode.responses.Count());
            StartDialogue(nextNode);
        }
    }

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
                return node; // Se encontró el nodo con el ID especificado
            }
        }

        // Si no se encuentra el nodo, muestra un error
        Debug.Log($"No se encontró un nodo con el ID: {id}");
        return null;
    }

    // Coroutine writes text letter by letter
    IEnumerator TypeText()
    {
        foreach (char character in fullText)
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}