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

    public CinemachineVirtualCamera CMCamera;
    public GameObject player; //player prefab
    public GameObject deathHandlerRetry;
    public GameObject deathHandlerLava;

    public int timesDied = 0;
    bool transitioning = false;
    bool playerAlive = true;
    Vector2 spawnLocation;
    GameObject currentPlayer;
    GameObject spawner;
    GameObject goal;

    UIController UIController;

    private void Start() {
        goal = GameObject.FindWithTag("goal");
        spawner = GameObject.FindWithTag("spawner");
        currentPlayer = GameObject.FindWithTag("Player");
        spawnLocation = spawner.transform.position;
        UIController = FindObjectOfType<UIController>();
    }

    private void FixedUpdate() {
        if (transitioning) {
            CMCamera.transform.position = Vector3.Lerp(new Vector3(CMCamera.transform.position.x, CMCamera.transform.position.y, -10), new Vector3(spawner.transform.position.x, spawner.transform.position.y, -10), 0.1f);
        }
    }

    private void Update() {
        
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
        UIController.stopwatch.Stop();
        yield return new WaitForSeconds(time / 2);
        transitioning = true;
        yield return new WaitForSeconds(time / 2);
        ResetLevel();
        GameObject playerGO = Instantiate(player, spawnLocation, Quaternion.identity);
        playerAlive = true;
        currentPlayer = playerGO;
        CMCamera.Follow = currentPlayer.transform;
        timesDied++;
        transitioning = false;
        UIController.Respawn();
    }

    public void StageClear() {
        UIController.StageCleared();
    }

    public void NextLevel() {
        if (SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex) {
            UIController.GoToMenu();
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }

    public void LoadLevel(int levelIndex) {
        SceneManager.LoadScene(levelIndex);
    }

    public void ExitGame() {
        UnityEngine.Debug.Log("Exit Game");
        Application.Quit();
    }

    public void Resume() {
        Time.timeScale = 1f;
    }

    public void Pause() {
        Time.timeScale = 0f;
    }
}
