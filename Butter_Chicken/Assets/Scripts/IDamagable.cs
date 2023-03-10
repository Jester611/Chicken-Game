using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamagable
{
    GameObject gameObject {get;}
    Rigidbody rb {get;}
    public float maxHealth {get; set;}
    public float currentHealth {get; set;}
    void TakeDamage(float damage);
}
