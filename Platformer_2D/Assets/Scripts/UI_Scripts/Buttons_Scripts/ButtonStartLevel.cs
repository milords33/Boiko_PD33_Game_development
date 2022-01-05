using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonStartLevel : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _levelToLoad;
    private const string NEWGAME = "ButtonNewGame";

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
       if (_button.name == NEWGAME)
            PlayerPrefs.DeleteKey("loadingLevel");
        SceneManager.LoadScene(_levelToLoad);
    }
}
