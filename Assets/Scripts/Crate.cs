using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {
    public GameObject drop;
    public int health = 10;
    public float dropRate = 1.0f;

    public Sprite damagedBox;
    public Sprite destroyedBox;

    SpriteRenderer _sr;
    // Use this for initialization
    void Start () {
        _sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            health -= 1;

            if (health < 3 && health > 0)
            {
                _sr.sprite = damagedBox;
            }
            else if (health <= 0)
            {
                foreach (Collider2D c in GetComponents<Collider2D>()) c.enabled = false;
                this.enabled = false;
                _sr.sprite = destroyedBox;

                if (drop != null && Random.value < dropRate)
                {
                    GameObject go = (GameObject)Instantiate(drop, this.transform.position + 0.5f * Vector3.up , Quaternion.identity);
                }
            }

        }
    }
}
