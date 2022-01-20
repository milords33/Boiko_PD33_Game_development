using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private PlayerMover _player;
    [SerializeField] private float _distanceToStartDialogue;
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private TextForDialogue _firstDialogueWindow;
    [SerializeField] private BanditBoss _banditBoss;

    private float _playerSpeed;

    private bool _dialogueIsBeggin;

    public bool DialogueIsEnded { get; set; } = false;

    private void Start()
    {
        _playerSpeed = _player.Speed;
        if (PlayerPrefs.HasKey("BeginDialogue"))
        {
                _dialogueIsBeggin = true;
                if(_banditBoss != null)
                    _banditBoss.ViewingDistance = _distanceToStartDialogue;
        }
            else
                _dialogueIsBeggin = false;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= _distanceToStartDialogue & !_dialogueIsBeggin)
        {
            _dialogueIsBeggin = true;
            _mainCanvas.gameObject.SetActive(true);        
            _player.Speed = 0;
            _firstDialogueWindow.GameObjectIsActive = true;
        }

        if (DialogueIsEnded)
        {
            _player.Speed = _playerSpeed;

            if(_banditBoss != null)
                _banditBoss.ViewingDistance = distanceToPlayer;

            PlayerPrefs.SetInt("BeginDialogue", 1);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (_banditBoss != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_banditBoss.transform.position, new Vector3(_distanceToStartDialogue * 2, 2f, 0));
        }
    }
}
