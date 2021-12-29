using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] public MeshRenderer _meshRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _pushPower;
    public float Speed { get; set; } = 0;
    public int Damage  { get; set; } = 0;
    public float Delay { get; set; } = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
            player.TakeDamage(Damage, _pushPower);
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
    public void StartFly(Vector2 direction)
    {
        _rigidbody.velocity = direction * Speed;
        Invoke(nameof(Destroy), Delay);
    }
}
