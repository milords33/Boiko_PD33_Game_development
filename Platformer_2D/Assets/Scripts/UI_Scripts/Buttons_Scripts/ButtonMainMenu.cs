using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMainMenu : MonoBehaviour
{
    [SerializeField] private Button _button;
    private const int _mainMenuLevel = 0;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        SceneManager.LoadScene(_mainMenuLevel);
    }
}