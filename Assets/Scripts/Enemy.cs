using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    [Space()]
    [Header("Enemy Properties")]
    public float activationDistance = 15.0f;
    public int damage = 1;
    public float contactKnockback = 100;
    public bool autocorrectFacing = true;

    protected Transform target = null;
    protected SpriteRenderer[] _sr;
    protected float distance = 99;

    protected override void AwakeActions()
    {
        base.AwakeActions();

        target = GameObject.FindWithTag("Player").transform;     
        _sr = this.GetComponentsInChildren<SpriteRenderer>(); 
    }

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null) return;

        if (autocorrectFacing)
        {
            if (target.position.x > this.transform.position.x) this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(-1, 1, 1), 0.6f);
            else this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(1, 1, 1), 0.6f);
        }

        distance = (target.position - this.transform.position).magnitude;

        // Follow if in range
        if (distance > activationDistance && activationDistance > 0) return;
        activationDistance = -1;

        Move();
        Attack();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Character>().Damage(damage);

            this._rb.AddForce(100f * (this.transform.position - collision.gameObject.transform.position).normalized * contactKnockback);

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null) rb.AddForce((collision.gameObject.transform.position - this.transform.position).normalized * Random.Range(1000, 2000));
        }
    }

    protected virtual void Move() { }

    protected virtual void Attack() { }

    public override void Die()
    {
        base.Die();

        _rb.velocity = Vector3.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        foreach (Collider2D c in GetComponentsInChildren<Collider2D>()) c.enabled = false;

        foreach (SpriteRenderer sr in _sr) sr.sortingLayerName = "Background";
        foreach (Bullet b in GetComponentsInChildren<Bullet>()) Destroy(b.gameObject);
    }
}
