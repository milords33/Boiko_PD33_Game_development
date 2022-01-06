using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonStartLevel : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _levelToLoad;
    [SerializeField] private LoadFromMainMenu _loading;
    [SerializeField] private GameObject _canvas;

    private const string NEWGAME = "ButtonNewGame";
    private const string LEVEL1 = "ButtonLevel1";

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        if (_button.name == NEWGAME || _button.name == LEVEL1)
        {
            PlayerPrefs.DeleteKey("loadingLevel");
            PlayerPrefs.DeleteKey("CoinsAmount");
        }

        PlayerPrefs.DeleteKey("HitPoints");
        _canvas.SetActive(false);
        _loading.AnimationEventRun();
        Invoke(nameof(LoadLevel), 2.2f);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
