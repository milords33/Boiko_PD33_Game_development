using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtoneSetLevelToLoad : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _currentMenu;
    [SerializeField] private GameObject _levelConfirmationMenu;
    [SerializeField] private GameObject _newGameConfirmationMenu;
    [SerializeField] private ButtonStartLevel _startLevel;

    private int _loadLevel;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {

        _currentMenu.SetActive(false);

        if (PlayerPrefs.HasKey("loadingLevel"))
        {
            if (_button.gameObject.name == "ButtonNewGame")
            {
                _startLevel.LevelToLoad = "Level_1";
                _newGameConfirmationMenu.SetActive(true);
            }
            else
            {
                _startLevel.LevelToLoad = _button.gameObject.name;
                _levelConfirmationMenu.SetActive(true);
            }
        }
        else
        {
            if (_button.gameObject.name == "ButtonNewGame")
            {
                _startLevel.LevelToLoad = "Level_1";
            }
            else
                _startLevel.LevelToLoad = _button.gameObject.name;

            _startLevel.LoadLevel();
        }
    }
}
