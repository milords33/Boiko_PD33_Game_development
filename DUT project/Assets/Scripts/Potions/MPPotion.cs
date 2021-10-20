using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPPotion : MonoBehaviour
{
    [SerializeField] private int _manaPoints;
    private int _potionCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null && _potionCount == 0)
        {
            _potionCount++;
            player.AddManaPoints(_manaPoints);
            Destroy(gameObject);
        }
    }
}
