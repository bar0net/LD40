using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingFlash : MonoBehaviour {
    public float duration = 0.04f;

    private float timer = 0;
    //private SpriteRenderer _sr;

    private void OnEnable()
    {
        timer = duration;
    }

    // Update is called once per frame
    void Update () {
        timer -= Time.unscaledDeltaTime;

        if (timer < 0) this.gameObject.SetActive(false);
	}
}
