using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class gameController : MonoBehaviour {

    public Text timesDiedText;
    public GameObject winScreen;
    public Text winText;
    public Text timer;
    public CinemachineVirtualCamera CMCamera;
    public GameObject player; //player prefab
    public GameObject deathHandlerRetry;
    public GameObject deathHandlerLava;
    public GameObject pauseMenuUI;
    public GameObject optionsUI;
    public GameObject pauseMenuItems;
    public AudioMixerGroup musicMixer;

    public static bool gamePaused = false;
    float PrefsVolume = 0f;

    int timesDied = 0;
    bool transitioning = false;
    bool playerAlive = true;
    string elapsedTime;
    Vector2 spawnLocation;
    GameObject currentPlayer;
    GameObject spawner;
    GameObject goal;
    Stopwatch stopwatch = new Stopwatch();

    private void Awake() {
        PrefsVolume = PlayerPrefs.GetFloat("MusicVolume");
    }

    private void Start() {
        musicMixer.audioMixer.SetFloat("Volume", PrefsVolume);
        optionsUI.GetComponentInChildren<Slider>().value = PrefsVolume;
        goal = GameObject.FindWithTag("goal");
        spawner = GameObject.FindWithTag("spawner");
        currentPlayer = GameObject.FindWithTag("Player");
        spawnLocation = spawner.transform.position;
        stopwatch.Start();
    }

    private void FixedUpdate() {
        if (transitioning) {
            CMCamera.transform.position = Vector3.Lerp(new Vector3(CMCamera.transform.position.x, CMCamera.transform.position.y, -10), new Vector3(spawner.transform.position.x, spawner.transform.position.y, -10), 0.1f);
        }
    }

    private void Update() {
        TimeSpan ts = stopwatch.Elapsed;
        elapsedTime = String.Format("{0:00}:{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        timer.text = elapsedTime;
        if (winScreen.activeInHierarchy == true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                NextLevel();
            }

        }
        if (Input.GetKeyDown(KeyCode.R)) {
            Respawn("retry");
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

    public void Respawn(String diedBy) {
        if (playerAlive) {
            GameObject deathHandler;
            if (diedBy == "lava") {
                deathHandler = deathHandlerLava;
            } else if (diedBy == "door") {
                deathHandler = deathHandlerRetry;
            } else {
                deathHandler = deathHandlerRetry;
            }
            GameObject deathHandlerGO = Instantiate(deathHandler, currentPlayer.transform.position, Quaternion.identity);
            Destroy(deathHandlerGO, 5f);

            Destroy(currentPlayer);
            playerAlive = false;
            stopwatch.Stop();
            StartCoroutine(RespawnAfterWait(1f));
        }
    }

    public void ResetLevel() {
        foreach (doorScript door in FindObjectsOfType<doorScript>()) {
            door.open = false;
        }
        foreach (physicsObject obj in FindObjectsOfType<physicsObject>()) {
            obj.Reset();
        }
    }

    IEnumerator RespawnAfterWait(float time) {
        yield return new WaitForSeconds(time / 2);
        transitioning = true;
        yield return new WaitForSeconds(time / 2);
        ResetLevel();
        GameObject playerGO = Instantiate(player, spawnLocation, Quaternion.identity);
        playerAlive = true;
        currentPlayer = playerGO;
        CMCamera.Follow = currentPlayer.transform;
        stopwatch.Reset();
        stopwatch.Start();
        timesDied++;
        timesDiedText.text = "DEATHS: " + timesDied;
        transitioning = false;
    }

    public void StageClear() {
        stopwatch.Stop();
        winText.text = string.Format("STAGE CLEAR\n{0}\n{1} DEATHS", elapsedTime, timesDied);
        winScreen.gameObject.SetActive(true);
    }

    public void NextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame() {
        UnityEngine.Debug.Log("Exit Game");
        Application.Quit();
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        PlayerPrefs.SetFloat("MusicVolume", PrefsVolume);
        Time.timeScale = 1f;
        gamePaused = false;
        stopwatch.Start();
    }

    public void Pause() {
        stopwatch.Stop();
        pauseMenuUI.SetActive(true);
        optionsUI.SetActive(false);
        pauseMenuItems.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
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
}
