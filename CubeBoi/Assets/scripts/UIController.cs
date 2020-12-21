using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Diagnostics;
using System;

public class UIController : MonoBehaviour {

    public Text timesDiedText;
    public GameObject winScreen;
    public Text winText;
    public Text timer;
    public GameObject pauseMenuUI;
    public GameObject optionsUI;
    public GameObject pauseMenuItems;
    public AudioMixerGroup musicMixer;
    gameController gameController;

    string elapsedTime;

    public static bool gamePaused = false;
    float PrefsVolume = 0f;

    public Stopwatch stopwatch = new Stopwatch();

    private void Awake() {
        PrefsVolume = PlayerPrefs.GetFloat("MusicVolume");
        stopwatch.Start();
    }
    // Use this for initialization
    void Start () {
        musicMixer.audioMixer.SetFloat("Volume", PrefsVolume);
        optionsUI.GetComponentInChildren<Slider>().value = PrefsVolume;
        gameController = FindObjectOfType<gameController>();
    }
	
	// Update is called once per frame
	void Update () {
        TimeSpan ts = stopwatch.Elapsed;
        elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        timer.text = elapsedTime;

        if (winScreen.activeInHierarchy == true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                gameController.NextLevel();
            }

        }
        if (Input.GetKeyDown(KeyCode.R)) {
            gameController.Respawn("retry");
            winScreen.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gamePaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void ToggleSettings() {
        if (optionsUI.activeInHierarchy) {
            optionsUI.SetActive(false);
            pauseMenuItems.SetActive(true);
        } else {
            optionsUI.SetActive(true);
            pauseMenuItems.SetActive(false);
        }
    }

    public void SetVolume(float volume) {
        musicMixer.audioMixer.SetFloat("Volume", volume);
        PrefsVolume = volume;
    }

    public void StageCleared() {
        stopwatch.Stop();
        winText.text = string.Format("stage clear\n{0}\n{1} deaths", elapsedTime, gameController.timesDied);
        winScreen.gameObject.SetActive(true);
    }

    public void Respawn() {
        stopwatch.Reset();
        stopwatch.Start();
        timesDiedText.text = "deaths: " + gameController.timesDied;
    }

    public void Pause() {
        stopwatch.Stop();
        pauseMenuUI.SetActive(true);
        optionsUI.SetActive(false);
        pauseMenuItems.SetActive(true);
        gamePaused = true;
        gameController.Pause();
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        gamePaused = false;
        stopwatch.Start();
        gameController.Resume();
    }

    public void GoToMenu() {
        Resume();
        gameController.LoadLevel(0);
    }
}
