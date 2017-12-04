using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShooter : MonoBehaviour {
    public GameObject bullet;
    public float cooldown = 1f;
    public int damage = 1;
    public Transform nozzle;
    public float timeOffset = 0;

    private float timer = 1.0f;

    private void Start()
    {
        timer += timeOffset;
    }

    void Update () {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            GameObject go = (GameObject)Instantiate(bullet, nozzle.position, this.transform.rotation);
            go.GetComponent<Bullet>().SetProperties("Player", damage, this.transform.right);
            timer += cooldown;
        }
	}
}
