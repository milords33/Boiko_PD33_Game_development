using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotion : MonoBehaviour
{
    [SerializeField] private int _hitPoints;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement player = other.GetComponent<CharacterMovement>();

        if (player != null)
        { 
            player.AddHitPoints(_hitPoints);  
            Destroy(gameObject);
        }
    }
}
