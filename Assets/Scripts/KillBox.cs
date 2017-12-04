using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character c = collision.gameObject.GetComponent<Character>();

        if (c != null) c.Die(); 
    }
}
