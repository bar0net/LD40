using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    
    [Header("Basic Character Properties")]
    public int health = 6;
    public float speed = 10.0f;
    public Material defaultMaterial;
    public Material hitMaterial;
    public float hitTimer = 0.05f;
    public float invulnerabilityTime = 0.5f;

    private float timer = 0.0f;
    private bool isHit = false;
    private float invulTimer = 0.0f;
    
    protected Rigidbody2D _rb;
    void Awake()
    {
        AwakeActions();
    }

    protected virtual void AwakeActions()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        // Return character to normal state after being hit
        if (timer < 0 && isHit)
        {
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material = defaultMaterial;
            }
            isHit = false;
        } 

        // Update timers if necessary
        if (timer > 0) timer -= Time.deltaTime;
        if (invulTimer > 0) invulTimer -= Time.deltaTime;
    }

    public void Damage(int value)
    {
        // Short Invulnerability after being damage
        if (invulTimer > 0) return;
        invulTimer = invulnerabilityTime;

        health -= value;
        if (health < 0) health = 0; // For Testing purposes

        // Make Sprite White after being damage
        VisualHit();

        // Sub-class specific damage behaviours
        LateDamage();

        // Check if still alive
        if (health <= 0) Die();
    }

    public virtual void LateDamage() { }

    public virtual void Die()
    {
        // Set to Dead Layer
        this.gameObject.layer = LayerMask.NameToLayer("Dead");

        // Stop any animations
        Animator anim = this.GetComponent<Animator>();
        if (anim != null) anim.speed = 0;

        // _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        this.enabled = false;
    }

    protected void VisualHit()
    {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.material = hitMaterial;
        }
        timer = hitTimer;
        isHit = true;
    }
}
