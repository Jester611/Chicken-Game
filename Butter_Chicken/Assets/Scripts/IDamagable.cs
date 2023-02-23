using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamagable
{
    GameObject gameObject {get;}
    Rigidbody rb {get;}
    public float maxHealth {get; set;}
    public float currentHealth {get; set;}
    public event System.Action OnDamaged;
    public event System.Action OnDeath;
    void TakeDamage(float damage);
}
