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

    [Header("ShopObject")]
    [SerializeField] private GameObject _greenFruit;
    [SerializeField] private GameObject _orangeFruit;
    [SerializeField] private GameObject _redFruit;
    [SerializeField] private GameObject _blueBottle;
    [SerializeField] private GameObject _pinkBottle;

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
        if(_merchantLaughSound.isPlaying == false)
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
        _player.Trade = false;
        _mainCanvas.SetActive(false);
    }

    private void OnButtonClickHandler()
    {
        _cancelTradeSound.Play();
        _tradeMenu.SetActive(false);
        _player.Trade = false;
    }

    public void DestroyProducts(string product)
    {
        if (product == "HealPotion")
            Destroy(_greenFruit);
        else if (product == "ManaPotion")
            Destroy(_blueBottle);
        else if (product == "Poison")
            Destroy(_redFruit);
        else if (product == "IncreaseHitPotion")
            Destroy(_orangeFruit);
        else if (product == "IncreaseManaPotion")
            Destroy(_pinkBottle);
        else
            throw new System.Exception("Invalid name object");
    }
}
