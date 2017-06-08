using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Died = -1,              
    None = 0,
    Steady = 1,
    Aiming = 2,
    Shot = 3
}

public class Enemy_Logic : MonoBehaviour {

    public EnemyState es = EnemyState.None;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
