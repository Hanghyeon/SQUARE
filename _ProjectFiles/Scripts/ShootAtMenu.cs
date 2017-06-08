using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtMenu : MonoBehaviour {

    Ray ray;
    int layerMask = 1 << 8;

    public Transform rayOrigin;

    ArmControllerScript weaponScript = null;
    float wDistance = 0f;        //무기의 사거리

    private void Awake()
    {
        if (weaponScript == null)
        {
            if (this.gameObject.GetComponent<ArmControllerScript>() == null)        //내 객체 검사
            {
                if (this.gameObject.GetComponentInChildren<ArmControllerScript>() != null)      //자식 객체 검사
                {
                    weaponScript = this.gameObject.GetComponentInChildren<ArmControllerScript>();
                    wDistance = weaponScript.ShootSettings.bulletDistance;
                }
                else                                                                            //그래도 없으면 프린트 및 0f로 셋팅
                {
                    weaponScript = null;
                    print("ShootAtMenu에서 ArmControllerScript를 가지고 오지 못했습니다. -> 부모 객체에 있을 것으로 보임, weaponScript가 Null임");
                }
            }
            else
            {
                weaponScript = this.gameObject.GetComponent<ArmControllerScript>();
                wDistance = weaponScript.ShootSettings.bulletDistance;
            }
        }
    }

    // Use this for initialization
    void Start() {
        ray = new Ray(this.transform.position, this.transform.forward);
    }


    //ShotSender에서 OnFire 델리게이트(Action)이 불러다 씀
    public void shoot()
    {
        RaycastHit hit;
        //트루일 경우 발사되어서 머쓸브레이크가 막 나오려는 중임
        if(ShotSender.Singleton.isFire)
        {
            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, wDistance, layerMask))
            {
                if (hit.transform.tag == "Menu")
                {
                    //hit.transform.gameObject.GetComponent<>();        //메뉴 관련 컴포넌트 안에 있는 함수를 실행함
                    hit.transform.gameObject.GetComponent<MenuDefualt>().workMenu();
                }
            }
        }

    }



}
    