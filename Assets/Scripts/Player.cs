using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    [Space()]
    [Header("Movement")]
    public float jumpForce = 30.0f;
    public float jumpDelay = 0.1f;

    [Space()]
    [Header("Jumping Properties")]
    public float gravityScaleRaise = 0.8f;
    public float gravityScaleFall = 1.8f;

    [Space()]
    [Header("References")]
    public Gun gun;
    public GameObject deathExplosion;

    [Space()]
    [Header("UI")]
    public Image[] healthBars;

    [Space()]
    [Header("Cheats")]
    public bool undying = false; // Deactivate on release!!!

    private bool grounded = true;
    private bool lookingRight = true;
    private Animator _anim;
    private float jumpTimer = 0;

    private void Start()
    {
        health = PlayerPrefs.GetInt("PlayerHealth", 6);

        if (gun == null) gun = GetComponentInChildren<Gun>();
        gun.cooldown = Mathf.Lerp(0.05f, 0.5f, (float)health / 6f);

        _anim = this.GetComponent<Animator>();

        // Reset UI color
        PaintHealth();

        _anim.SetBool("Idle", true);
        UnityEngine.Camera.main.GetComponent<Camera>().SetZoom(9 - health);
    }

    private void Update()
    {
        this.transform.localScale = Vector3.Lerp(
            this.transform.localScale,
            new Vector3(  (lookingRight?1:-1), 1, 1),
            0.3f
            );

        if (Input.GetButton("Fire1")) gun.Fire(7 - health);

        if (jumpTimer > 0) jumpTimer -= Time.deltaTime;
    }

    void FixedUpdate () {
        MovementControl();

        if (!grounded && _rb.velocity.y < 0)
        {
            _rb.gravityScale = gravityScaleFall;
            _anim.SetBool("falling", true);
        }
        else
        {
            _anim.SetBool("falling", false);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Terrain" && !grounded)
        {
            _rb.gravityScale = 1.0f;
            grounded = true;
            jumpTimer = jumpDelay;
            _anim.SetBool("Grounded", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Terrain" && !grounded) _anim.SetBool("Grounded", false);
    }

    private void MovementControl()
    {
        float v_x = Mathf.Lerp(_rb.velocity.x, speed * Input.GetAxis("Horizontal"), 0.25f);
        float v_y = _rb.velocity.y - 0.1f;

        if (Input.GetAxis("Horizontal") > 0.1)
        {
            lookingRight = true;
            _anim.SetBool("Idle", false);
        }
        else if (Input.GetAxis("Horizontal") < -0.1)
        {
            lookingRight = false;
            _anim.SetBool("Idle", false);
        }
        else
        {
            _anim.SetBool("Idle", true);
        }
        
        _anim.speed = Mathf.Lerp(0.5f,1f, 2 * Mathf.Abs(_rb.velocity.x) / speed);



        if (Input.GetButtonDown("Jump") && grounded && jumpTimer <= 0)
        {
            grounded = false;
            v_y = jumpForce;
            _rb.gravityScale = gravityScaleRaise;
        }

        if (Input.GetButtonUp("Jump"))
        {
            _rb.gravityScale = 1.0f;
        }

        if (v_y > 1.1f * jumpForce) v_y = 1.1f * jumpForce;
        _rb.velocity = new Vector2(v_x, v_y);
    }

    private void PaintHealth()
    {
        for (int i = 0; i < healthBars.Length; i++)
        {
            if (i < health) healthBars[i].color = Color.red;
            else healthBars[i].color = Color.white;
        }
    }

    public override void LateDamage()
    {
        if (health == 0 && undying) health = 1;

        PaintHealth();
        UnityEngine.Camera.main.GetComponent<Camera>().SetZoom(9 - health);
        gun.cooldown = Mathf.Lerp(0.03f, 0.3f, (float)health / 6f);
    }

    public override void Die()
    {
        base.Die();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform t = this.transform.GetChild(i);
            t.gameObject.SetActive(t.gameObject.name == "Death");
        }
        deathExplosion.SetActive(true);
        Time.timeScale = 0.25f;

        GameObject.FindObjectOfType<GameManager>().GameOver();
    }

    public void Heal(int value)
    {
        health += value;

        if (health > 6) health = 6;

        LateDamage();
    }
}
