using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    [Header("Performance")]
    public GameObject bullet;
    public Transform gunNozzle;
    public float cooldown = 0.1f;
    public float bulletSpread = 0.1f;
    public string targetTag = "Enemy";

    [Space()]
    [Header("Cosmetics")]
    public ShootingFlash shootingFlash;
    public float characterKickback = -0.1f;
    public float gunKickback = -0.1f;
    public float cameraKickback = -0.3f;
    [Range(0.0f,1.0f)]
    public float recoveryRate = 0.6f;

    private float cdTimer = 0.0f;
    private Vector3 defaultPos;
    private bool lookingRight = true;
    private Transform _parent;

    private float tripleShotTimer = 0;
    private SpriteRenderer _sr;
    bool tripleTime = false;

    AudioSource _as;

    private void Awake()
    {
        defaultPos = this.transform.localPosition;
        _parent = this.transform.parent;
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (Time.timeScale == 0) return;

        // Return to the default position (recovery from the kickback of firing the gun)
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, defaultPos, recoveryRate);

        // LOOK TOWARDS MOUSE POINTER
        // Find the rotation angle towards mouse pointer
        Vector3 mouse = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse = _parent.InverseTransformPoint(mouse);
        float angle = Mathf.Atan2( Mathf.Sign(_parent.localScale.x) * mouse.y,mouse.x) * Mathf.Rad2Deg;
        
        // Flip the Gun in Y to avoid having the gun upsidedown
        if ((Mathf.Abs(angle) > 90 && lookingRight) || (Mathf.Abs(angle) < 90 && !lookingRight))
        {
            lookingRight = !lookingRight;
            this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y , this.transform.localScale.z);
        }
        this.transform.rotation = Quaternion.Euler(0,0, angle);

        // Update cooldown timer
        if (cdTimer > 0.0f) cdTimer -= Time.deltaTime;

        if (tripleShotTimer > 0) tripleShotTimer -= Time.deltaTime;
        if (tripleTime && tripleShotTimer <= 0)
        {
            tripleTime = false;
            _sr.color = Color.white;
        }
	}

    public void Fire(int dmg)
    {
        // Exit if not ready to fire
        if (cdTimer > 0.0f) return;

        if (_as != null && !_as.isPlaying)
        {
            _as.time = 0;
            _as.Play();
        }

        // Instantiate Bullet
        GameObject go = (GameObject)Instantiate(bullet, gunNozzle.position, this.transform.rotation);
        go.GetComponent<Bullet>().SetProperties(targetTag, dmg, Mathf.Sign(_parent.localScale.x) * this.transform.right + Random.Range(-bulletSpread, bulletSpread) * this.transform.up);
        shootingFlash.gameObject.SetActive(true);

        if (tripleTime)
        {
            GameObject go2 = (GameObject)Instantiate(bullet, gunNozzle.position, this.transform.rotation);
            go2.GetComponent<Bullet>().SetProperties(targetTag, dmg, Mathf.Sign(_parent.localScale.x) * this.transform.right + Random.Range(-bulletSpread, bulletSpread) * this.transform.up + 0.25f * this.transform.up);

            GameObject go3 = (GameObject)Instantiate(bullet, gunNozzle.position, this.transform.rotation);
            go3.GetComponent<Bullet>().SetProperties(targetTag, dmg, Mathf.Sign(_parent.localScale.x) * this.transform.right + Random.Range(-bulletSpread, bulletSpread) * this.transform.up - 0.25f * this.transform.up);
        }


        // Apply Kickback
        this.transform.position += gunKickback * this.transform.right;
        UnityEngine.Camera.main.transform.position += cameraKickback * this.transform.right;
        //_parent.GetComponent<Rigidbody2D>().AddForce(300f * characterKickback * this.transform.right * Mathf.Sign(_parent.localScale.x));
        Vector2 v = (Vector2)(characterKickback * this.transform.right * Mathf.Sign(_parent.localScale.x));
        v = new Vector2(v.x, 0.1f * v.y);

        _parent.GetComponent<Rigidbody2D>().velocity += v;

        // Set cooldown timer
        cdTimer = cooldown;
    }

    public void EnableTripleShot()
    {
        tripleShotTimer = 5.0f;
        _sr.color = Color.yellow;
        tripleTime = true;
    }
}
