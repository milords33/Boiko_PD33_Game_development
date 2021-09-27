using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPPotion : MonoBehaviour
{
   [SerializeField] private int _manaPoints;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement player = other.GetComponent<CharacterMovement>();

        if (player != null && player.ManaPoints != 100)
        {
            player.AddManaPoints(_manaPoints);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Impossible to use the potion");
        }
    }
}
