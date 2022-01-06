using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonContinue : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private LoadFromMainMenu _loading;
    [SerializeField] private GameObject _canvas;


    private int _levelToLoad = 0;

    void Awake()
    {
        if (PlayerPrefs.HasKey("loadingLevel"))
        {
            _buttonImage.color = Color.white;
            _levelToLoad = PlayerPrefs.GetInt("loadingLevel");
            _button.onClick.AddListener(OnButtonClickHandler); 
        }
        else
        {
            _buttonImage.color = Color.gray;
            _button.colors = ColorBlock.defaultColorBlock;
        }
    }

    private void OnButtonClickHandler()
    {
        _canvas.SetActive(false);
        _loading.AnimationEventRun();
        Invoke(nameof(LoadLevel), 2.2f);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(_levelToLoad);
    }

}
