using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    [Range(0.0f,1.0f)]
    public float smoothing = 0.125f;
    public float zoomTarget;
    public float zoomTime = 0.25f;

    private float prevZoom;
    private float timer = 0;

	// Use this for initialization
	void Start () {
        if (target == null)
            target = GameObject.FindWithTag("Player").transform;

        prevZoom = UnityEngine.Camera.main.orthographicSize;

    }

    private void LateUpdate()
    {

        if (timer < zoomTime)
        {
            UnityEngine.Camera.main.orthographicSize = Mathf.Lerp(prevZoom, zoomTarget, timer / zoomTime);
            timer += Time.deltaTime;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        Vector3 newPos = target.position + offset + (target.right * target.localScale.x) + Mathf.Lerp(0, 3f, (UnityEngine.Camera.main.orthographicSize - 3f) / 6f) * Vector3.up;

        this.transform.position = Vector3.Lerp(this.transform.position, newPos, smoothing);
	}

    public void SetZoom(float zoom)
    {
        zoomTarget = zoom;
        timer = 0;
        prevZoom = UnityEngine.Camera.main.orthographicSize;
    }
}
