using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerPrefs.SetInt("PlayerHealth", collision.gameObject.GetComponent<Player>().health);
            PlayerPrefs.SetInt("PlayerHealth", collision.gameObject.GetComponent<Player>().health);

            int index = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            index = index % UnityEngine.SceneManagement.SceneManager.sceneCount;

            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
        }
    }
}
