using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextForDialogue : MonoBehaviour
{
    [SerializeField] private GameObject _textPanel;
    [SerializeField] private TextMeshProUGUI _textGameObject;
    [SerializeField] private Dialogue _dialogue;
    [SerializeField] private TextForDialogue _nextPanel;
    [SerializeField] private PlayerMover _player;
    [SerializeField] private AudioSource _laugh;

    private string _text;

    public bool GameObjectIsActive { get; set; } = false;

    private void Update()
    {
        if(GameObjectIsActive)
        {
            GameObjectIsActive = false;
            if (_player != null)
            {   
                if(_textGameObject.text.Length > 50)
                    _textPanel.transform.position = _player.transform.position + new Vector3(2, 2, 0);
                else
                    _textPanel.transform.position = _player.transform.position + new Vector3(1.2f, 2, 0);
            }
            CreateText();
        }

        if (!GameObjectIsActive && _textGameObject.text == _text)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return))
            {
                _textPanel.SetActive(false);

                if (_nextPanel != null)
                {
                    _nextPanel.gameObject.SetActive(true);
                    _nextPanel.GameObjectIsActive = true;
                }
                else if (_dialogue != null)
                    _dialogue.DialogueIsEnded = true;
                else
                    throw new System.Exception("Can't find dialogue data");
            }
        }
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return))
            {
            if (_textGameObject.text != _text)
                {
                   _textGameObject.text = _text;
                }
            }
        
    }

    private void CreateText()
    {
        if (_textGameObject.text == "(Laugh)" && _laugh != null)
            _laugh.Play();
        else if (_textGameObject.text == "(Laugh)" && _laugh == null)
            throw new System.Exception("cant't Find sound");

        _text = _textGameObject.text;
        _textGameObject.text = "";
        StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine()
    {
            foreach (char symbol in _text)
            {
                if (_textGameObject.text != _text)
                {
                     _textGameObject.text += symbol;
                     yield return new WaitForSeconds(0.04f);
                }
            }
    }
}
