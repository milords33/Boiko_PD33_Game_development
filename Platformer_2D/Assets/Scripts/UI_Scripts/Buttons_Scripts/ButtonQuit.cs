using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonQuit : MonoBehaviour
{

    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }    
}
