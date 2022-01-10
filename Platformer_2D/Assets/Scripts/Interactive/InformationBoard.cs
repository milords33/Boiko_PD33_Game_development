using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationBoard : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _textMenu;
    [SerializeField] private Button _button;

    private PlayerMover _player = null;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClickHandler);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player = collision.GetComponent<PlayerMover>();
        _canvas.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            _textMenu.SetActive(true);
            _player.Trade = true;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            _player.Trade = false;
            _textMenu.SetActive(false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _player.Trade = false;
        //_player = null;
        _canvas.SetActive(false);
    }

    private void OnButtonClickHandler()
    {
        _player.Trade = false;
        _textMenu.SetActive(false);
    }    
}
