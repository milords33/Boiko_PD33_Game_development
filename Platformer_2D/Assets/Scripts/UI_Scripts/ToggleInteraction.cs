using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInteraction : MonoBehaviour
{
    [SerializeField] private Toggle _togglePlay;

    private void Awake()
    {
        _togglePlay.onValueChanged.AddListener(OnTogglePlayChangedHandler);
    }

    private void OnTogglePlayChangedHandler(bool isOn)
    {
        if(isOn)
            Debug.Log($"{_togglePlay.name} = selected");
        else
            Debug.Log($"{_togglePlay.name} = unselected");
    }
}
