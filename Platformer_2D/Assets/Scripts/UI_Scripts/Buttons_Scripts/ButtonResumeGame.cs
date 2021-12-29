using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonResumeGame : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
