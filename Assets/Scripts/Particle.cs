using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
    public float timer = 0.03f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        if (timer < 0) Destroy(this.gameObject);
	}
}
