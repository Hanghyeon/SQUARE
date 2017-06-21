using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuType
{
    QuitGame = -3,
    GotoGall = -2,
    GotoQuit = -1,
    None = 0,
    GotoPlay = 1,
    PauseGame = 2,
    Resume = 3
}

public enum camSide
{
    left = 0,
    right = 1,
    up = 2,
    down = 3
}

public class MenuDefualt : MonoBehaviour {

    public float switchAngle = 50f;
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    public Transform Player=null;
    public MenuType mt = MenuType.None;
    public bool isLookAt = false;

    public Transform screenTF = null;
    public GameObject pauseMenu = null;
    public GameObject pauseScreen = null;
    List<Camera> cams = new List<Camera>();
    bool temp;

    private void Awake()
    {
        GameObject go = GameObject.Find("Head");
        if (go != null)
            Player = go.transform;
        else
            print("ERROR~!! Player Transform is NULL~!!!!");

        GameObject go2 = GameObject.Find("PauseScreen");
        if (go2 != null)
            pauseScreen = go2;
        else
            print("ERROR~!! PauseScreen is NULL~!!!!");

        GameObject go3 = GameObject.Find("ScreenPOS");
        if (go3 != null)
            screenTF = go3.transform;
        else
            print("ERROR~!! screenTF is NULL~!!!!");

        GameObject go4 = GameObject.Find("CameraGroup_Done/Head/Menu/PauseGame");
        if (go4 != null)
            pauseMenu = go4;
        else
            print("ERROR~!! screenTF is NULL~!!!!");

        foreach (Camera item in Player.GetComponentsInChildren<Camera>())
        {
            if (item.name != "VufoCamera")
                cams.Add(item);
        }

    }

    private void Start()
    {
        pauseScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLookAt)
            this.transform.LookAt(Player);

        if (sprites.Count > 0)
        {
            if (temp = checkIsVisible())
            {
                foreach (SpriteRenderer item in sprites)
                {
                    item.enabled = true;
                }
            }
            else
            {
                foreach (SpriteRenderer item in sprites)
                {
                    item.enabled = false;
                }
            }
        }
    }


    bool checkIsVisible()
    {

        Quaternion lookAtMe = Quaternion.identity;    // Querternion 함수 선언
        Vector3 lookatVec = (this.transform.position - Player.transform.position).normalized;
        lookAtMe.SetLookRotation(lookatVec);  // 쿼터니언의 SetLookRotaion 함수 적용,  player가 이 오브젝트를 정면으로 바라볼 때 쿼터니온은 lookat이 됨

        if ((Player.transform.rotation.eulerAngles.x > lookAtMe.eulerAngles.x - switchAngle &&
            Player.transform.rotation.eulerAngles.x < lookAtMe.eulerAngles.x + switchAngle) ||
            (Player.transform.rotation.eulerAngles.y > lookAtMe.eulerAngles.y - switchAngle * 1.5 &&
            Player.transform.rotation.eulerAngles.y < lookAtMe.eulerAngles.y + switchAngle * 1.5 )&&
            (Player.transform.rotation.eulerAngles.z > lookAtMe.eulerAngles.z - switchAngle &&
            Player.transform.rotation.eulerAngles.z < lookAtMe.eulerAngles.z + switchAngle))
        {
            return true;
        }

        return false;
    }

    public void workMenu()
    {
        switch (mt)
        {
            case MenuType.QuitGame:
                QuitGame();
                break;
            case MenuType.GotoQuit:
                GotoQuit();
                break;
            case MenuType.None:
                print("Did not setting this Menu. dosen't work");
                break;
            case MenuType.GotoPlay:
                GotoPlayGame();
                break;
            case MenuType.GotoGall:
                GotoGallery();
                break;
            case MenuType.PauseGame:
                PauseGame();
                break;
            case MenuType.Resume:
                ResumeGame();
                break;
        }
    }

    void GotoPlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    void GotoQuit()
    {
        SceneManager.LoadScene("CheckQuitGame");
    }

    void ResumeGame()
    {
        CustomMover.Singleton.canMove = !CustomMover.Singleton.canMove;
        pauseMenu.SetActive(true);
        pauseScreen.SetActive(false);
    }

    void PauseGame()
    {
        //일시정지 창을 띄운다
        pauseScreen.gameObject.SetActive(true);
        pauseScreen.transform.position = screenTF.position;
        pauseScreen.transform.rotation = screenTF.rotation;
        pauseMenu.gameObject.SetActive(false);


        CustomMover.Singleton.canMove = !CustomMover.Singleton.canMove;         //플레이어의 이동을 멈추고, 자이로와 총은 살아있어야 함 
        //적을 멈추고

    }

    void QuitGame()
    {
        Application.Quit();
    }

    void GotoGallery()
    {
        SceneManager.LoadScene("Gallery");
    }


}
