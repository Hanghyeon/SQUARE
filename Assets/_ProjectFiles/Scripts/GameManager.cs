using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Singletaon = null;

    private static GameManager Singleton;
    private static GameObject container;
    public static GameManager GetInstance()
    {
        if (!Singleton)
        {
            container = new GameObject();
            container.name = "Logger";
            Singleton = container.AddComponent(typeof(GameManager)) as GameManager;
        }
        return Singleton;
    }

    public Transform screenTF = null;
    public GameObject pauseMenu = null;
    public GameObject pauseScreen = null;

    //http://unityindepth.tistory.com/38
}


