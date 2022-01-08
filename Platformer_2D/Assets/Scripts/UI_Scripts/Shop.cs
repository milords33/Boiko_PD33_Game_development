using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _tradeMenu;
    [SerializeField] private Button _interactiveButton;
    [SerializeField] private Button _cancelButton;

    [Header("Audio")]
    [SerializeField] private AudioSource _openShopSound;
    [SerializeField] private AudioSource _cancelTradeSound;
    [SerializeField] private AudioSource _merchantLaughSound;

    PlayerMover _player;

    private void Awake()
    {
        _cancelButton.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _merchantLaughSound.Play();
        _player = collision.GetComponent<PlayerMover>();
        _mainCanvas.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.E))
        {
            _openShopSound.Play();
            _player.Trade = true;
            _tradeMenu.SetActive(true);
            _interactiveButton.gameObject.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Escape) && _player.Trade)
        {
            _tradeMenu.SetActive(false);
            _cancelTradeSound.Play();
            _interactiveButton.gameObject.SetActive(true);

            _player.Trade = false;
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _player = null;
        _mainCanvas.SetActive(false);
    }

    private void OnButtonClickHandler()
    {
        _cancelTradeSound.Play();
        _tradeMenu.SetActive(false);
        _player.Trade = false;
    }

}
