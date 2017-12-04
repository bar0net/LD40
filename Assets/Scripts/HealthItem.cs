using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour {

    int heal = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGER");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Heal(heal);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Heal(heal);
            Destroy(this.gameObject);
        }
    }

}
