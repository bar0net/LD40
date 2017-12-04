using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int damage = 3;
    public Vector3 direction = Vector3.zero;
    public float speed = 5.0f;
    public float lifeSpan = 2.0f;

    public GameObject impactEffect;

    public string targetTag = "Enemy";

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += direction * speed * Time.deltaTime;

        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0) Destroy(this.gameObject);
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == targetTag)
        {
            Character c = other.gameObject.GetComponent<Character>();
            if (c == null) return;

            c.Damage(damage);
            UnityEngine.Camera.main.transform.position += Random.Range(0f, 0.5f) * this.transform.up + Random.Range(0f, 0.5f) * this.transform.right;

            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null) rb.AddForce(direction.normalized * Random.Range(1000, 1200));
        }

        if (impactEffect != null)
        {
            GameObject go = (GameObject)Instantiate(impactEffect, transform.position + 0.1f * direction, Quaternion.AngleAxis(Random.Range(0f,360f), Vector3.forward));
        }
        Destroy(this.gameObject);
    }

    public void SetProperties(string tag, int dmg, Vector3 dir)
    {
        this.targetTag = tag;
        this.damage = dmg;
        this.direction = dir.normalized;
    }
}
