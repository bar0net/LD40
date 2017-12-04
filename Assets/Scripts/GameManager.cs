using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject pauseMenu;
    bool pauseMenuActive = false;

    public AudioClip deathClip;
    public AudioSource musicSource;

    bool endcount = false;
    float endTimer = 0;

    public Text menuText;
    public Button resumeButton;

    public Text timerText;
    public Text timerShadow;

    int accTime;

    // Use this for initialization
    void Start () {
        TogglePauseMenu(false);
        accTime = PlayerPrefs.GetInt("Time", 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause")) TogglePauseMenu(!pauseMenuActive);

        if (endcount) endTimer += Time.unscaledDeltaTime;

        if (endTimer > 2)
        {
            endcount = false;
            endTimer = 0;
            Time.timeScale = 0;

            TogglePauseMenu(true, "Game Over", false);
        }

        timerText.text = GetTime();
        timerShadow.text = timerText.text;
	}

    public void TogglePauseMenu(bool setActive, string title = "Pause", bool resumeActive = true)
    {
        resumeButton.interactable = resumeActive;
        menuText.text = title;

        pauseMenu.SetActive(setActive);
        pauseMenuActive = setActive;

        if (setActive) Time.timeScale = 0f;
        else Time.timeScale = 1.0f; 
    }

    public void GameOver()
    {
        musicSource.clip = deathClip;
        musicSource.loop = false;
        musicSource.Play();

        endcount = true;
    }

    string GetTime()
    {
        int time = accTime + (int)Time.timeSinceLevelLoad;

        return Mathf.Floor((float)time / 60f).ToString("00") + ":" + (time % 60).ToString("00");
    }

    public void SaveTime()
    {
        PlayerPrefs.SetInt("Time", accTime + (int)Time.timeSinceLevelLoad);
    }
}
