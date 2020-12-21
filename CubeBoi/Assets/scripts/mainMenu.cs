using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour {

    public GameObject optionsUI;
    public GameObject mainMenuItems;
    public GameObject levelsUI;
    public AudioMixerGroup musicMixer;
    public GameObject cube;
    float PrefsVolume;

    private void Start() {
        PrefsVolume = PlayerPrefs.GetFloat("MusicVolume");
        optionsUI.GetComponentInChildren<Slider>().value = PrefsVolume;
    }

    private void FixedUpdate() {
        cube.transform.Rotate(Vector3.up, 0.1f);
    }

    public void StartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int levelIndex) {
        SceneManager.LoadScene(levelIndex);
    }

    public void ToggleLevelSelect() {
        if (levelsUI.activeInHierarchy) {
            levelsUI.SetActive(false);
            mainMenuItems.SetActive(true);
        } else {
            levelsUI.SetActive(true);
            mainMenuItems.SetActive(false);
        }
    }

    public void ToggleSettings() {
        if (optionsUI.activeInHierarchy) {
            optionsUI.SetActive(false);
            mainMenuItems.SetActive(true);
            PlayerPrefs.SetFloat("MusicVolume", PrefsVolume);
        } else {
            optionsUI.SetActive(true);
            mainMenuItems.SetActive(false);
        }
    }

    public void SetVolume(float volume) {
        musicMixer.audioMixer.SetFloat("Volume", volume);
        PrefsVolume = volume;
    }

    public void Quit() {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
