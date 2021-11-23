using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignScript : MonoBehaviour
{
    [TextArea(4, 10)]
    public string DisplayText;
    public GameObject TextBox;
    public TextMeshProUGUI SignText;

    private bool hasBeenTriggered = false;

    private void Start()
    {
        DisplayText = DisplayText.Replace("\\n", "\n");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            SignText.text = DisplayText;
            TextBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            TextBox.SetActive(false);
            hasBeenTriggered = false;
        }
    }
}