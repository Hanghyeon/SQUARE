using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Singletaon = null;

    public Transform screenTF = null;
    public GameObject pauseMenu = null;
    public GameObject pauseScreen = null;

    private void Awake()
    {
        Singletaon = this;

        screenTF = GameObject.Find("CameraGroup_Done/Head/Menu/ScreenPOS").transform;

        pauseMenu = GameObject.Find("CameraGroup_Done/Head/Menu/PauseMenu"); 
        pauseScreen = GameObject.Find("CameraGroup_Done/PauseScreen");

        if (screenTF == null)
            print("ERROR~!! screenTF is NULL~!!!!");
        if (pauseMenu == null)
            print("ERROR~!! pauseMenu is NULL~!!!!");
        if (pauseScreen== null)
            print("ERROR~!! pauseScreen is NULL~!!!!");

    }



}


