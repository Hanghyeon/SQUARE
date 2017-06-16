using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeManager : MonoBehaviour {
    public static FreezeManager Singleton;
    public CustomMover Mover = null;
    //public Slider freezeTimeBar;
    //public Button killedEnemyButton;
    public float freezeTime;        //  값은 0f~1f 사이

    public bool getIsFreeze { get { return isFreeze; } }

    private float lessSize = 0.01f;  //  게이지를 초당 얼마나 잃을건지

    private bool isFreeze = false;

    private float maxValue = 1f;

    private void Awake()
    {
        Singleton = this;

        GameObject go = GameObject.Find("CameraGroup_Done");

        if (go != null)
            Mover = go.GetComponent<CustomMover>();
        else
            print("ERROR~!!! Mover is Null~!!!!!!");

        CustomMover.OnCompleted += Init;
    }

    private void Start()
    {
        CustomMover.OnCompleted += startFreeze;
    }

    //// Update is called once per frame
    //void Update () {
    //       freezeTimeBar.value = freezeTime;
    //}

    private void LateUpdate()
    {
        lessTime(lessSize);
    }

    void startFreeze()  ///////////////////////////////////////
    {
        lessSize = CustomRail.Singleton.nodes[CustomMover.Singleton.getCurrentSeg].lessTime;
        isFreeze = CustomMover.Singleton.getFreeze;
    }                   ///////////////////////////////////////

    void lessTime(float value)
    {
        if (isFreeze)
        {

            if (freezeTime <= 0f)                               //0f 이하로는 내려가지 않음
            {
                freezeTime = 0f;
                isFreeze = false;
            }
            else
            {
                freezeTime -= Time.deltaTime * value;           //초당 value만큼 줄어듬, value는 lessSize
            }
        }
    }


    void Init()
    {
        freezeTime = maxValue;

        ////타이머 준비값으로 초기화
        //freezeTime = freezeTimeBar.maxValue;
        //
        ////빨강 게이지 맥스로 초기화
        //freezeTimeBar.value = freezeTime;

    }
}
