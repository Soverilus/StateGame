using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseController : MonoBehaviour {
    [SerializeField]
    GameObject backToMenuButton;
    AudioSource winMusic;
    AudioSource loseMusic;

    [SerializeField]
    GameObject victoryHolder;
    Image victory;
    [SerializeField]
    GameObject defeatHolder;
    Image defeat;

    void Start() {
        victory = victoryHolder.GetComponent<Image>();
        defeat = defeatHolder.GetComponent<Image>();
        winMusic = victoryHolder.GetComponent<AudioSource>();
        loseMusic = defeatHolder.GetComponent<AudioSource>();
        victory.enabled = false;
        defeat.enabled = false;
        backToMenuButton.SetActive(false);
    }

    public void WinLose(bool win) {
        victory.enabled = win;
        defeat.enabled = !win;
        winMusic.enabled = win;
        loseMusic.enabled = !win;
        backToMenuButton.SetActive(true);
    }
}