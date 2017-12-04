using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [System.Serializable]
    class SpawnItem
    {
        public GameObject item;
        public float rate;
    }

    public Transform[] spawnPoints;
    [SerializeField]
    SpawnItem[] items;

    float startingDelay = 1.5f;
    float minDelay = 0.1f;
    [Range(0, 1)]
    float decay = 0.97f;
    float decayTime = 10.0f;

    float spawnTimer = 0;
    float decayTimer = 0;
	// Use this for initialization
	void Start () {
        decayTimer = decayTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (decayTimer < 0) Decay();
        if (spawnTimer < 0) Spawn();

        decayTimer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;
	}

    void Decay()
    {
        startingDelay = decay * startingDelay;

        if (startingDelay < minDelay)
        {
            startingDelay = minDelay;
            decayTimer = 1000; 
        }
        else
        {
            decayTimer = decayTime;
        }
    }

    void Spawn()
    {
        int point = Random.Range(0, spawnPoints.Length);
        float rng = Random.value;

        for (int i = 0; i < items.Length; i++)
        {
            rng -= items[i].rate;
            if (rng <= 0)
            {
                Create(i, point);
                break;
            }
        }

        spawnTimer = startingDelay;
    }

    void Create(int index, int point)
    {
        GameObject go = (GameObject)Instantiate(items[index].item, spawnPoints[point].position, Quaternion.identity);
        Enemy e = go.GetComponent<Enemy>();
        if (e != null)
        {
            e.activationDistance = 1000;
        }
    }
}
