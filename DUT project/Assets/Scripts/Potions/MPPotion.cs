using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPPotion : MonoBehaviour
{
   [SerializeField] private int _manaPoints;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null)
        {
            player.AddManaPoints(_manaPoints);
            Destroy(gameObject);
        }
    }
}
