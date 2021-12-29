using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{

    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        Debug.Log($"Button {_button.name} was pressed");
        if (_button.enabled)
            _button.enabled = false;
        else
            _button.enabled = true;
    }
}
