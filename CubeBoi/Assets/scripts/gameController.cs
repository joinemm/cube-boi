using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {

    public int timesDied = 0;
    bool transitioning = false;
    string elapsedTime;

    Vector2 spawnLocation;
    public GameObject player;
    public GameObject currentPlayer;
    public Text timesDiedText;
    public GameObject winScreen;
    public Text winText;
    public Text timer;
    public CinemachineVirtualCamera CMCamera;
    GameObject spawner;
    GameObject goal;

    Stopwatch stopwatch = new Stopwatch();

    private void Start() {
        goal = GameObject.FindWithTag("goal");
        spawner = GameObject.FindWithTag("spawner");
        currentPlayer = GameObject.FindWithTag("Player");
        spawnLocation = spawner.transform.position;
        stopwatch.Start();
    }

    private void FixedUpdate() {
        if (transitioning) {
            CMCamera.transform.position = Vector3.Lerp(new Vector3(CMCamera.transform.position.x, CMCamera.transform.position.y,-10), new Vector3(spawner.transform.position.x, spawner.transform.position.y, -10), 0.1f);
        }
    }

    private void Update() {
        TimeSpan ts = stopwatch.Elapsed;
        elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
        timer.text = elapsedTime;
        if (winScreen.activeInHierarchy == true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                NextLevel();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            currentPlayer.GetComponent<playerMovement>().Die();
            winScreen.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            UnityEngine.Debug.Log("Exit Game");
            Application.Quit();
        }
    }

    public void Respawn() {
        stopwatch.Stop();
        StartCoroutine(RespawnAfterWait(1f));
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
        yield return new WaitForSeconds(time/2);
        transitioning = true;
        yield return new WaitForSeconds(time / 2);
        ResetLevel();
        GameObject playerGO = Instantiate(player, spawnLocation, Quaternion.identity);
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
        winText.text = string.Format("STAGE CLEAR\n{0}\n{1} DEATHS",elapsedTime,timesDied);
        winScreen.gameObject.SetActive(true);
    }

    public void NextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
