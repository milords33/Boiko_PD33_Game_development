using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotion : MonoBehaviour
{
    [SerializeField] private int _hitPoints;
    private int _potionCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null &&_potionCount == 0)
        {
            _potionCount++;
            player.AddHitPoints(_hitPoints);  
            Destroy(gameObject);
        }
    }
}
