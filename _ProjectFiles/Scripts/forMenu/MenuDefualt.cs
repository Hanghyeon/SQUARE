using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuType
{
    QuitGame=-1,
    None=0,
    StartPlay =1,

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
    public Transform Player;
    public MenuType mt = MenuType.None;

    List<Camera> cams = new List<Camera>();
    bool temp;

    private void Awake()
    {

        foreach (Camera item in Player.GetComponentsInChildren<Camera>())
        {
            if (item.name != "VufoCamera")
                cams.Add(item);
        }

    }


    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(Player);
        
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
            case MenuType.None:
                print("Did not setting this Menu. dosen't work");
                break;
            case MenuType.StartPlay:
                StartGame();
                break;
        }
    }

    void StartGame()
    {
        GameManager.Singleton.InitData();
        SceneManager.LoadScene("MainGame");
    }

    void QuitGame()
    {
        SceneManager.LoadScene("CheckQuitGame");
    }
}
