using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy {

    [Space()]
    [Header("Ghost Properties")]
    public Collider2D modelCollider;
    public float onTime = 0.4f;
    public float offTime = 0.6f;

    protected override void Move()
    {
        base.Move();

        //this.transform.position += (target.position - this.transform.position).normalized * speed * Time.deltaTime; 
        this._rb.velocity = Vector3.Lerp(this._rb.velocity, (target.position - this.transform.position).normalized * speed, 0.3f);
    }

    protected override void Attack()
    {
        base.Attack();
        
        // Manage Duty Cycle (Using proximity threshold to ensure contact)
        if (Time.time % (onTime + offTime) < offTime && distance > 1.0f)
        {
            modelCollider.enabled = false;
            foreach (SpriteRenderer s in _sr) s.color = Color.Lerp(s.color, new Color(1, 1, 1, 0.4f), 0.1f);
        }
        else
        {
            modelCollider.enabled = true;
            foreach(SpriteRenderer s in _sr) s.color = Color.Lerp(s.color, Color.white, 0.1f);
        }
    }

}
