using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignScript : MonoBehaviour
{
    public string DisplayText;
    public GameObject TextBox;
    public TextMeshProUGUI SignText;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && !hasBeenTriggered)
        {
            SignText.text = DisplayText;
            TextBox.SetActive(true);
            hasBeenTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            TextBox.SetActive(false);
            hasBeenTriggered = false;
        }
    }
}