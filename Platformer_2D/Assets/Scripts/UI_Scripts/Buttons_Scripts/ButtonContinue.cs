using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonContinue : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;


    private int _levelToLoad = 0;

    // Start is called before the first frame update
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
        SceneManager.LoadScene(_levelToLoad);
    }


}
