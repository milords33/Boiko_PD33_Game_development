using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _tradeMenu;
    [SerializeField] private Button _interactiveButton;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _mainCanvas.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        PlayerMover player = other.GetComponent<PlayerMover>();

        if (Input.GetKey(KeyCode.E))
        {
            player.Trade = true;
            _tradeMenu.SetActive(true);
            _interactiveButton.gameObject.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            _tradeMenu.SetActive(false);
            _interactiveButton.gameObject.SetActive(true);

            player.Trade = false;
        }
            
    }

        private void OnTriggerExit2D(Collider2D collision)
    {
        _mainCanvas.SetActive(false);
    }

}
