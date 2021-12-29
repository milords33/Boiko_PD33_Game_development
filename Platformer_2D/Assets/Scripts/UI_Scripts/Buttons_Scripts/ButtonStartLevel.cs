using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonStartLevel : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _levelToLoad;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
