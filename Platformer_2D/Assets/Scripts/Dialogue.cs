using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private PlayerMover _player;
    [SerializeField] private float _distanceToStartDialogue;
    [SerializeField] private GameObject _mainCanvas;


    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer <= _distanceToStartDialogue)
        {
           // _player._rigidbody.simulated = false;
        }
    }
}
