using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitData
{
    

    public static class PistolArmInfo
    {
        public static int Ammo = 15;
        public static float FireRate = 0.1f;
        public static float BulletDistance = 100f;
        public static float BulletForce = 50f;
    }
}


public class GameManager : MonoBehaviour {

    public static GameManager Singleton;
    public static int levels = (1 << 1) | (1 << 2);

    private void OnLevelWasLoaded(int level)
    {
        if (level == levels)
        {

        }
    }



    void Awake()
    {
        
        Singleton = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addData()
    {

    }
}
