using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonStartLevel : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private LoadFromMainMenu _loading;
    [SerializeField] private GameObject _canvas;

    private string _levelToLoad;

    public string LevelToLoad
    {
        get => _levelToLoad;
        set
        {
            _levelToLoad = value;
        }
    }
    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler()
    {
        PlayerPrefs.DeleteKey("loadingLevel");
        PlayerPrefs.DeleteKey("CoinsAmount");
        PlayerPrefs.DeleteKey("ManaPoints");
        PlayerPrefs.DeleteKey("MaxHitPoints");
        PlayerPrefs.DeleteKey("MaxManaPoints");
        PlayerPrefs.DeleteKey("MaxShieldPoints");
        PlayerPrefs.DeleteKey("AttackDamage");

        PlayerPrefs.DeleteKey("HitPoints");
        PlayerPrefs.DeleteKey("ManaPoints");
        _canvas.SetActive(false);
        _loading.AnimationEventRun();
        Invoke(nameof(InvokeLoadLevel), 2.2f);
    }
    
    private void InvokeLoadLevel()
    {
        SceneManager.LoadScene(_levelToLoad);
    }

    public void LoadLevel()
    {
        _canvas.SetActive(false);
        _loading.AnimationEventRun();
        Invoke(nameof(InvokeLoadLevel), 2.2f);
    }
}
