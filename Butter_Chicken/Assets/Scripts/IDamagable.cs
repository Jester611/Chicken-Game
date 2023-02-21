using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamagable
{
    GameObject gameObject {get;}
    Rigidbody rb {get;}
    void TakeDamage(float damage);
}
