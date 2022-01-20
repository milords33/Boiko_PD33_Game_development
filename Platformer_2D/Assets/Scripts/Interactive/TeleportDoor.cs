using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _doorOpenSprite;
    [SerializeField] private AudioSource _openDoorSound;
    [SerializeField] private GameObject _interactionButton;

    [Header("Position")]
    [SerializeField] private float _posX;
    [SerializeField] private float _posY;

    private PlayerMover _player = null;
    private bool _teleportIsActive = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _player = other.GetComponent<PlayerMover>();
        if (_player != null)
            _interactionButton.gameObject.SetActive(true);
    }
    private void OnTriggerStay2D()
    {
        if (Input.GetKey(KeyCode.E) && _player != null && !_teleportIsActive)
        {
            _teleportIsActive = true;
            _spriteRenderer.sprite = _doorOpenSprite;
            _openDoorSound.Play();
            Invoke(nameof(Teleport), 1f);
        }
    }

    private void OnTriggerExit2D()
    {
        _interactionButton.gameObject.SetActive(false);
        //_player = null;
    }

    private void Teleport()
    {
        _player.gameObject.transform.position = new Vector2(_posX, _posY);
    }


}
