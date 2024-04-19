using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text; // Include for StringBuilder

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;

    public Text DialogueText { get => dialogueText; set => dialogueText = value; }
    public float TypingSpeed { get => typingSpeed; set => typingSpeed = value; }

    public void DisplayText(string textToDisplay)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(textToDisplay));
    }

    IEnumerator TypeText(string textToType)
    {
        dialogueText.text = "";
        StringBuilder sb = new StringBuilder();

        foreach (char letter in textToType.ToCharArray())
        {
            sb.Append(letter);
            dialogueText.text = sb.ToString();
            yield return new WaitForSeconds(typingSpeed);
        }

        OnTypingComplete(); // Callback when typing is done
    }

    private void OnTypingComplete()
    {
        // Optional: Notify other scripts that typing has finished
        Debug.Log("Typewriter effect completed.");
    }
}