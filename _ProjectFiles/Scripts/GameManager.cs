using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayState
{
    Died=-1,

    Start=3,
    None =0,
    Done=4,

    Move=1,
    Fight=2,

}

public class GameManager : MonoBehaviour {

    public static GameManager Singleton;
    protected bool canHeadTrack = false;

    static int playerHP = 0;
    static WeaponType wType = WeaponType.Melee;
    static PlayState pState = PlayState.None;


    private void Awake()
    {
        Singleton = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitData()
    {
        playerHP = 10;
        wType = WeaponType.Rifle;
        pState = PlayState.Start;

        print("Init Data~ function is empty");
    }
}
