using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] private InputActionReference RebindAction = null;
    [SerializeField] private EventSystem eventSystem = null;
    [SerializeField] private TMP_Text bindingDisplayNameText = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    
    public void StartRebinding()
    {
        eventSystem.sendNavigationEvents = false;
        bindingDisplayNameText.SetText("Waiting...");

        rebindingOperation = RebindAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        eventSystem.sendNavigationEvents = true;
        int bindingIndex = RebindAction.action.GetBindingIndexForControl(RebindAction.action.controls[0]);

        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(RebindAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();
    }
}
