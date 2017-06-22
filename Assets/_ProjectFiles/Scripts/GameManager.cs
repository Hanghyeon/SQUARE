using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
    public static GameManager Singletaon = null;

    public Transform screenTF = null;
    public GameObject pauseMenu = null;
    public GameObject pauseScreen = null;

    public GameObject resScreen = null;
    public Text res01 = null;
    public Text res02 = null;
    public Text res03 = null;

    List<UserTargetScript> targets = new List<UserTargetScript>();

    [SerializeField]
    GameObject next = null;
    [SerializeField]
    GameObject quit2 = null;        //일단 급하니깐...

    [SerializeField]
    GameObject resume = null;
    [SerializeField]
    GameObject quit = null;

    Dictionary<GameObject, MenuLite> mLite = new Dictionary<GameObject, MenuLite>();

    private void Awake()
    {
        Singletaon = this;


        screenTF = GameObject.Find("CameraGroup_Done/Head/Menu/ScreenPOS").transform;

        pauseMenu = GameObject.Find("CameraGroup_Done/Head/Menu/PauseMenu");
        pauseScreen = GameObject.Find("CameraGroup_Done/PauseScreen");

        resScreen = GameObject.Find("RailManager/Node (50)/Arrived_Sign");

        res01 = GameObject.Find("RailManager/Node (50)/Arrived_Sign/ArrivedCanvas/res01").GetComponent<Text>();
        res02 = GameObject.Find("RailManager/Node (50)/Arrived_Sign/ArrivedCanvas/res02").GetComponent<Text>();
        res03 = GameObject.Find("RailManager/Node (50)/Arrived_Sign/ArrivedCanvas/res03").GetComponent<Text>();

        GameObject go1 = GameObject.Find("Targets");
        int cnt = go1.transform.GetChildCount();
        for (int idx = 0; idx < cnt; idx++)
        {
            targets.Add(go1.transform.GetChild(idx).GetComponentInChildren<UserTargetScript>());
        }

        next = GameObject.Find("RailManager/Node (50)/Arrived_Sign/Next");
        quit2 = GameObject.Find("RailManager/Node (50)/Arrived_Sign/Quit");


        if (screenTF == null)
            print("ERROR~!! screenTF is NULL~!!!!");
        if (pauseMenu == null)
            print("ERROR~!! pauseMenu is NULL~!!!!");
        if (pauseScreen == null)
            print("ERROR~!! pauseScreen is NULL~!!!!");

        resume = pauseScreen.transform.Find("Resume").gameObject;
        quit = pauseScreen.transform.Find("Quit").gameObject;

        


        //예외 처리가 더 필요할지도 모름
        mLite.Add(pauseMenu, pauseMenu.GetComponent<MenuLite>());
        mLite.Add(resume, resume.GetComponent<MenuLite>());
        mLite.Add(quit, quit.GetComponent<MenuLite>());
        mLite.Add(quit2, quit2.GetComponent<MenuLite>());
        mLite.Add(next, next.GetComponent<MenuLite>());

        if (pauseMenu.active)
            pauseMenu.SetActive(true);

        if (pauseScreen.active)
            pauseScreen.SetActive(false);

        if (resScreen.active)
            resScreen.SetActive(false);

        CustomMover.OnCompleted += arriveSemiDone;
    }

    //총 맞아야 1회 실행됨
    public void runMenu(GameObject go)
    {
        MenuLite ml = go.GetComponent<MenuLite>();
        if (ml.Hit)
        {
            switch (ml.mt)
            {
                case MenuType.GotoGall:
                    GotoGallery();
                    break;
                case MenuType.Pause:
                    PauseGame();
                    break;
                case MenuType.Resume:
                    ResumeGame();
                    break;
                case MenuType.Next:
                    NextGame();
                    break;
                default:
                    print("WARNING~~~~!!!!!!! non stored MenuType. plz check -> GameManager/runMenu <- function.");
                    break;
            }
        }
        else
        {
            print("WARNING~~~!!!!!!! " + go.name + " didn't hit.");
        }
    }


    //플레이어가 50번째 노드에 도달하면 1번 실행됨
    void arriveSemiDone()
    {
        if (pauseMenu.active)
            pauseMenu.SetActive(false);

        int died = 0;

        if (!resScreen.active)
            resScreen.SetActive(true);

        foreach (UserTargetScript item in targets)
        {
            if (item.isHit == true)
                died++;
        }


        res01.text = targets.Count.ToString();
        res02.text = died.ToString();
        res03.text = (targets.Count - died).ToString();
    }

    void NextGame()
    {
        //스테이지 2가 생기면 주석 풀자ㅠㅠ
        //SceneManager.LoadScene("MainGame2");
        SceneManager.LoadScene("Gallery");
    }

    void ResumeGame()
    {
        //일시정지 창을 닫는다

        if (!pauseMenu.active)
            pauseMenu.gameObject.SetActive(true);
        if (pauseScreen.active)
            pauseScreen.gameObject.SetActive(false);

        //이동을 재개
        CustomMover.Singleton.canMove = !CustomMover.Singleton.canMove;
    }

    void PauseGame()
    {
        //일시정지 창을 띄운다

        if (!pauseScreen.active)
        {
            pauseScreen.gameObject.SetActive(true);
            pauseScreen.transform.position = screenTF.position;
            pauseScreen.transform.rotation = screenTF.rotation;
        }

        if (pauseMenu.active)
            pauseMenu.gameObject.SetActive(false);

        //플레이어의 이동을 멈추고, 자이로와 총은 살아있어야 함 
        CustomMover.Singleton.canMove = !CustomMover.Singleton.canMove;
        
        //적을 멈추고 - 지금 적은 수정중임, 타겟으로 대체중

    }

    void GotoGallery()
    {
        SceneManager.LoadScene("Gallery");
    }



}


