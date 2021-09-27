using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField] private int _poisonDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement player = other.GetComponent<CharacterMovement>();

        if (player != null)
        {
            player.ReduceHitPoints(_poisonDamage);
            Destroy(gameObject);
        }
    }
}
