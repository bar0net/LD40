using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy {

    [Space()]
    [Header("Wizard Properties")]
    public float chargeTime = 1f;
    public float idleTime = 1f;
    public GameObject projectile;
    public GameObject nozzle;
    public Vector2 targetRange = new Vector2(3,6); 

    Vector3 destination = Vector3.zero;
    bool shooting = false;
    float shootTimer = 0;
    float restTimer = 0;

    private Transform t_projectile;
    private Bullet b_projectile;

    private void Start()
    {
        GetNewDestination();
    }

    protected override void Move()
    {
        base.Move();
        if (shooting) return;

        Vector3 diff = destination - this.transform.position;
        if ((target.position - destination).magnitude > 2 * targetRange.y) GetNewDestination();

        if (diff.magnitude > 0.2) _rb.velocity = Vector3.Lerp(_rb.velocity, speed * diff.normalized, 0.6f);
        // else destinationReached = true;
        else GetNewDestination();

        restTimer -= Time.deltaTime;
    }

    protected override void Attack()
    {
        base.Attack();

        if (restTimer > 0) return;
        shooting = true;
        _rb.velocity = Vector3.zero;

        if (t_projectile == null)
        {
            GameObject go = (GameObject)Instantiate(projectile, nozzle.transform.position, Quaternion.identity, this.transform);
            t_projectile = go.transform;
            t_projectile.localScale = new Vector3(0.01f, 0.01f, 1);

            b_projectile = go.GetComponent<Bullet>();
            b_projectile.SetProperties("Player", 1, Vector3.zero);
        }

        if (shootTimer > 0)
        {
            // Charge attack
            t_projectile.localScale = Vector3.Lerp(new Vector3(0.01f, 0.01f, 1), 0.3f*Vector3.one, 1 - shootTimer / chargeTime);
        }
        else
        {
            // Release projectile
            t_projectile.SetParent(null);
            b_projectile.SetProperties("Player", 1, target.position - t_projectile.position);
            t_projectile = null;
            b_projectile = null;

            GetNewDestination();
            shootTimer = chargeTime;
            restTimer = idleTime;
            shooting = false;
        }
        
        shootTimer -= Time.deltaTime;
    }

    void GetNewDestination()
    {
        Vector3 diff = this.transform.position - target.position;

        int sign = 1;
        if (Random.value < 0.5) sign = -1;

        float angle = sign * Random.Range(0.5f, 0.9f) + Mathf.Atan2(diff.y, diff.x);

        destination = (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)) * Random.Range(targetRange.x, targetRange.y) + target.position;
    }
    
}
