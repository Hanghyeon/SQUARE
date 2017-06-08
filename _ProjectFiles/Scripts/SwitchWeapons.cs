using UnityEngine;
using System.Collections;
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
    public Transform[] guns;

    [Header("Gun Text")]
    //Gun text
    public string gun1Text;
    public string gun2Text;
    public string gun3Text;
    public string gun4Text;
    public string gun5Text;

    [Header("UI Components")]
    //UI Text components
    public Text[] totalAmmoText;
    public Text[] ammoLeftText;
    public Text tutorialText;
    public Text[] currentGunText;
    public Slider[] ammoLeftSlide;

    [Header("Customizable Options")]
    //How long the tutorial text will be visible
    public float tutorialTextTimer = 10.0f;
    //How slow the tutorial text will fade out
    public float tutorialTextFadeOutTime = 4.0f;

    [Header("Color")]
    public float chageWarning = 0.3f;

    private int index;

    void Start()
    {
        index = 0;

        //Start with the first gun selected
        currentGunObject = guns[index];
        changeGun(index);
        //Set the current gun text
        currentGunText[index].text = gun1Text;

        //Get the ammo values from the first guns script and show as text
        totalAmmoText[index].text = guns[index].GetComponentInChildren
            <ArmControllerScript>().ShootSettings.ammo.ToString();
        ammoLeftText[index].text = guns[index].GetComponentInChildren
            <ArmControllerScript>().currentAmmo.ToString();

        //Start the tutorial text timer
        StartCoroutine(TutorialTextTimer());
    }

    void Update()
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
        if (Input.GetKeyDown(KeyCode.Alpha1) &&
           currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
        {
            index = 0;
            changeGun(index);
            totalAmmoText[index].text = guns[index].GetComponentInChildren
                <ArmControllerScript>().ShootSettings.ammo.ToString();
            //Set the currentGunObject to the current gun
            currentGunObject = guns[index];
            //Set the current gun text
            currentGunText[index].text = gun1Text;
        }

        //If key 2 is pressed, and noSwitch is false in GunScript.cs
        if (Input.GetKeyDown(KeyCode.Alpha2) &&
           currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
        {
            index = 1;
            changeGun(index);
            totalAmmoText[index].text = guns[index].GetComponentInChildren
                <ArmControllerScript>().ShootSettings.ammo.ToString();
            //Set the currentGunObject to the current gun
            currentGunObject = guns[index];
            //Set the current gun text
            currentGunText[index].text = gun2Text;
        }

        //If key 3 is pressed, and noSwitch is false in GunScript.cs
        if (Input.GetKeyDown(KeyCode.Alpha3) &&
           currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
        {
            index = 2;
            changeGun(index);
            totalAmmoText[index].text = guns[index].GetComponentInChildren
                <ArmControllerScript>().ShootSettings.ammo.ToString();
            //Set the currentGunObject to the current gun
            currentGunObject = guns[index];
            //Set the current gun text
            currentGunText[index].text = gun3Text;
        }

        //If key 4 is pressed, and noSwitch is false in GunScript.cs
        if (Input.GetKeyDown(KeyCode.Alpha4) &&
           currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
        {
            index = 3;
            changeGun(index);
            totalAmmoText[index].text = guns[index].GetComponentInChildren
                <ArmControllerScript>().ShootSettings.ammo.ToString();
            //Set the currentGunObject to the current gun
            currentGunObject = guns[index];
            //Set the current gun text
            currentGunText[index].text = gun4Text;
        }

        //If key 5 is pressed, and noSwitch is false in GunScript.cs
        if (Input.GetKeyDown(KeyCode.Alpha5) &&
           currentGunObject.GetComponentInChildren<ArmControllerScript>().noSwitch == false)
        {
            index = 4;
            changeGun(index);
            totalAmmoText[index].text = guns[index].GetComponentInChildren
                <ArmControllerScript>().ShootSettings.ammo.ToString();
            //Set the currentGunObject to the current gun
            currentGunObject = guns[index];
            //Set the current gun text
            currentGunText[index].text = gun5Text;
        }
    }

    //Activates the current gun from the array
    void changeGun(int num)
    {
        currentGun = num;
        for (int i = 0; i < guns.Length; i++)
        {
            if (i == num)
                guns[i].gameObject.SetActive(true);
            else
                guns[i].gameObject.SetActive(false);
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