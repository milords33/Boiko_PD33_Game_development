using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TMP_DropdownInteraction : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _TMP_dropdown;

    private void Awake()
    {
        _TMP_dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    private void OnDropdownValueChanged(int number)
    {
        Debug.Log($"{_TMP_dropdown.name} changed value.");
        Debug.Log($"Was selected element {number + 1}.");
    }
}
