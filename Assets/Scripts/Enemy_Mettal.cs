using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mettal : Enemy {
    public bool moving = true;
    public float thrustDistance = 5f;
    public float idleTime = 1.2f;
    public Transform leftPoint;
    public Transform RightPoint;

    private float timeout;

    Vector3 targetPosition;
    Animator _anim;

    protected override void AwakeActions()
    {
        base.AwakeActions();

        timeout = 0.5f + thrustDistance / speed;
        _anim = GetComponent<Animator>();
    }

    protected override void Move()
    {
        if (!moving) return;

        timeout -= Time.deltaTime;
        //if ( Mathf.Abs(targetPosition.x - this.transform.position.x) < 0.5)
        if ( Mathf.Sign(this.transform.localScale.x) * this.transform.position.x > Mathf.Sign(this.transform.localScale.x) * targetPosition.x)
        {
            // Stop Movement
            _rb.velocity = Vector3.zero;

            // Pause for Idle animation
            moving = false;
            _anim.SetBool("Idle", true);

            timeout = idleTime;
        }
        else
        {
            _rb.velocity = Vector3.Lerp(_rb.velocity, Mathf.Sign(this.transform.localScale.x) * this.transform.right * speed, 0.4f);
        }
    }

    protected override void Attack()
    {
        base.Attack();

        if (moving) return;

        timeout -= Time.deltaTime;

        if (timeout < 0)
        {
            // Define new objective                
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            if (this.transform.localScale.x < 0) targetPosition = leftPoint.position;
            else targetPosition = RightPoint.position;

            // Set animations
            _anim.SetBool("Idle", false);
            moving = true;

            // Define timeout
            timeout = 0.5f + thrustDistance / speed;
        }
    }


}
