using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
    public string nextLevelName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerPrefs.SetInt("PlayerHealth", collision.gameObject.GetComponent<Player>().health);
            PlayerPrefs.SetInt("PlayerHealth", collision.gameObject.GetComponent<Player>().health);

            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextLevelName);
        }
    }
}
