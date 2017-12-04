using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour {
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character c = collision.gameObject.GetComponent<Character>();
        if (c != null) c.Damage(damage);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Character c = collision.gameObject.GetComponent<Character>();
        if (c != null) c.Damage(damage);
    }
}
