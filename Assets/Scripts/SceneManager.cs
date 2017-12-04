using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

	public void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        string name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        LoadScene(name);
    }

    public void ResetGameState()
    {
        PlayerPrefs.SetInt("PlayerHealth", 6);
        PlayerPrefs.SetInt("Time", 0);
    }
}
