using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwitchWeapons : MonoBehaviour
{
    Vector3 red = new Vector3(1f, 0f, 0f);
    Vector3 orange = new Vector3(1f, 140f / 255f, 0f);
    Vector3 nomal = new Vector3(1f, 1f, 1f);

    [Header("Guns")]
    //Current gun number
    public int currentGun = 0;
    //Get the current gun object
    public Transform currentGunObject;

    //Array of guns
    public List<Transform> guns = new List<Transform>();
    /* 0    Pistol     
     * 1    Rifle    
     * 2    Sniper
     * 3    ShotGun
     */

    [Header("Gun Text")]
    //Gun text
    public string gun1Text;
    public string gun2Text;
    public string gun3Text;
    public string gun4Text;
    public string gun5Text;

    [Header("UI Components")]
    //UI Text components
    public List<Text> totalAmmoText = new List<Text>();
    public List<Text> ammoLeftText = new List<Text>();
    public Text tutorialText;
    public List<Text> currentGunText = new List<Text>();
    public List<Slider> ammoLeftSlide = new List<Slider>();

    [Header("Customizable Options")]
    //How long the tutorial text will be visible
    public float tutorialTextTimer = 10.0f;
    //How slow the tutorial text will fade out
    public float tutorialTextFadeOutTime = 4.0f;

    [Header("Color")]
    public float chageWarning = 0.3f;


    [SerializeField]
    private int index;

    public static System.Action OnSetGunsArray;

    private int[] initlevels = { 0, 1, 2 };

    ShotSender sSender = null;




    private void OnLevelWasLoaded(int level)
    {
        OnDestroy();
        
        for(int num=0; num<initlevels.Length;num++)
        {
            if (level == initlevels[num])
            {
                Awake();
            }
        }

        
    }

    void Awake()
    {
        GameObject go1 = GameObject.Find("ShotImageTarget/ShotSender");

        if (go1 != null)
            sSender = go1.GetComponent<ShotSender>();

        OnSetGunsArray += sSender.Init;

        for (int count = 0; count < this.transform.childCount; count++)
        {
            if (this.transform.GetChild(count).gameObject.activeInHierarchy != false)
            {
                guns.Add(this.transform.GetChild(count));
            }

        }

        if (OnSetGunsArray != null)
            OnSetGunsArray();
        else
            print("ERROR~!!! OnSetGunsArray Action is Null~!!!!!!");


        TargetSender.OnGyroRotSet += changeGun;     //이동하면서 득득득 떨리는거 없애려고 넣음

        GameObject go = GameObject.Find("TutorialCanvas/TutorialText");
        if (go != null)
            tutorialText = go.GetComponent<Text>();
        else
            tutorialText = null;


        if(checkIndex(guns.Count))

        foreach (var item in guns)
        {
            if (item != null)
            {
                //guns[] 안에 게임오브젝트 속 텍스트 찾아오기
                totalAmmoText.Add(item.Find("Gun/Armature/weapon/AmmoCanvas/TotalAmmoText").GetComponent<Text>());
                currentGunText.Add(item.Find("Gun/Armature/weapon/CurrentGunCanvas/CurrentGunText").GetComponent<Text>());
                ammoLeftText.Add(item.Find("Gun/Armature/weapon/AmmoCanvas/AmmoLeftText").GetComponent<Text>());
                ammoLeftSlide.Add(item.Find("Gun/Armature/weapon/AmmoCanvas/Slider").GetComponent<Slider>());
            }
            else
                print("ERROR~!!!! guns List have Null~~!!!!!!");
        }

        index = 0;

        //Start with the first gun selected
        currentGunObject = guns[index];
        

    }

    private void OnDestroy()
    {
        OnSetGunsArray -= sSender.Init;

        guns.Clear();
        totalAmmoText.Clear();
        currentGunText.Clear();
        ammoLeftText.Clear();
        ammoLeftSlide.Clear();
    }

    void Start()
    {
        
        //Start the tutorial text timer
        StartCoroutine(TutorialTextTimer());

        guns[index].gameObject.SetActive(false);

        //Set the current gun text
        currentGunText[index].text = gun1Text;

        //Get the ammo values from the first guns script and show as text
        totalAmmoText[index].text = guns[index].GetComponentInChildren
            <ArmControllerScript>().ShootSettings.ammo.ToString();
        ammoLeftText[index].text = guns[index].GetComponentInChildren
            <ArmControllerScript>().currentAmmo.ToString();

        changeGun(index);
    }

    void Update()
    {
        if (checkIndex(index))
        {
            //Get the ammo left from the current gun
            //and show it as a text

            if (currentGunObject.GetComponentInChildren<ArmControllerScript>().nowReloading == false)
            {
                ammoLeftText[index].text = currentGunObject.GetComponentInChildren
                    <ArmControllerScript>().currentAmmo.ToString();
                ammoLeftSlide[index].value = (float)currentGunObject.GetComponentInChildren<ArmControllerScript>().currentAmmo /
                                        currentGunObject.GetComponentInChildren<ArmControllerScript>().ShootSettings.ammo;
            }
            else
            {
                ammoLeftText[index].text = "-";
                ammoLeftSlide[index].value = 0f;
            }

            //Chage Color (Orange, Red) for empty the mag
            if (currentGunObject.GetComponentInChildren<ArmControllerScript>().currentAmmo <= 0)
            {
                ammoLeftSlide[index].GetComponentInChildren<Image>().color = new Color(red.x, red.y, red.z);
                ammoLeftText[index].color = new Color(red.x, red.y, red.z);
            }
            else if (currentGunObject.GetComponentInChildren
                <ArmControllerScript>().currentAmmo <= currentGunObject.GetComponentInChildren
                <ArmControllerScript>().ShootSettings.ammo * chageWarning)
            {
                ammoLeftSlide[index].GetComponentInChildren<Image>().color = new Color(orange.x, orange.y, orange.z);
                ammoLeftText[index].color = new Color(orange.x, orange.y, orange.z);
            }
            else
            {
                ammoLeftSlide[index].GetComponentInChildren<Image>().color = new Color(nomal.x, nomal.y, nomal.z);
                ammoLeftText[index].color = new Color(nomal.x, nomal.y, nomal.z);
            }

            //If key 1 is pressed, and noSwitch is false in GunScript.cs
            if (guns[0].gameObject.GetComponent<GetFollower>().follower.wType == WeaponType.Pistol &&
               currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
            {
                index = 0;
                totalAmmoText[index].text = guns[index].GetComponentInChildren
                    <ArmControllerScript>().ShootSettings.ammo.ToString();
                //Set the currentGunObject to the current gun
                currentGunObject = guns[index];
                //Set the current gun text
                currentGunText[index].text = gun1Text;
                changeGun(index);
            }

            //If key 2 is pressed, and noSwitch is false in GunScript.cs
            if (guns[1].gameObject.GetComponent<GetFollower>().follower.wType == WeaponType.Rifle &&
               currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
            {
                index = 1;
                totalAmmoText[index].text = guns[index].GetComponentInChildren
                    <ArmControllerScript>().ShootSettings.ammo.ToString();
                //Set the currentGunObject to the current gun
                currentGunObject = guns[index];
                //Set the current gun text
                currentGunText[index].text = gun2Text;
                changeGun(index);
            }

            //If key 3 is pressed, and noSwitch is false in GunScript.cs
            if (guns[2].gameObject.GetComponent<GetFollower>().follower.wType == WeaponType.Sniper &&
               currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
            {
                index = 2;
                totalAmmoText[index].text = guns[index].GetComponentInChildren
                    <ArmControllerScript>().ShootSettings.ammo.ToString();
                //Set the currentGunObject to the current gun
                currentGunObject = guns[index];
                //Set the current gun text
                currentGunText[index].text = gun3Text;
                changeGun(index);
            }

            //If key 4 is pressed, and noSwitch is false in GunScript.cs
            if (guns[3].gameObject.GetComponent<GetFollower>().follower.wType == WeaponType.ShotGun &&      //여기 인덱스 문제 있음
               currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
            {
                index = 3;
                totalAmmoText[index].text = guns[index].GetComponentInChildren
                    <ArmControllerScript>().ShootSettings.ammo.ToString();
                //Set the currentGunObject to the current gun
                currentGunObject = guns[index];
                //Set the current gun text
                currentGunText[index].text = gun4Text;
                changeGun(index);
            }
        }
        else
        {
            print("ERROR~~~~!!!!!!! Guns List is wrong~~!!!!!!");
        }
        ////If key 5 is pressed, and noSwitch is false in GunScript.cs
        //if (Input.GetKeyDown(KeyCode.Alpha5) &&
        //   currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
        //{
        //    index = 4;
        //    changeGun(index);
        //    totalAmmoText[index].text = guns[index].GetComponentInChildren
        //        <ArmControllerScript>().ShootSettings.ammo.ToString();
        //    //Set the currentGunObject to the current gun
        //    currentGunObject = guns[index];
        //    //Set the current gun text
        //    currentGunText[index].text = gun5Text;
        //}
    }

    //Activates the current gun from the array
    public void changeGun(int num)
    {
        
        if (guns.Count == 0)
            return;
        else
        {
            for (int i = 0; i < guns.Count; i++)
            {
                if (guns[i].gameObject != null)
                {
                    if (i == num)
                        guns[i].gameObject.SetActive(true);
                    else
                        guns[i].gameObject.SetActive(false);
                }
            }
        }
    }

    bool checkIndex(int num)
    {
        if (num > guns.Count)
            return false;
        else if (num < 0 && guns.Count != 0)
            return false;
        else if (guns.Count == 0)
            return false;

        else
        {
            index = num;
            return true;
        }
    }

    //Timer for the tutorial text fade 
    IEnumerator TutorialTextTimer()
    {
        //Wait the set amount of time
        yield return new WaitForSeconds(tutorialTextTimer);
        if (tutorialText != null)
        {

            //Start fading out the tutorial text
            tutorialText.CrossFadeAlpha
                (0.0f, tutorialTextFadeOutTime, false);
        }
    }
}