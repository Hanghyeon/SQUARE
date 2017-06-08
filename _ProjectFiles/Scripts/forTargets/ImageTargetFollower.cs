using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public enum WeaponType
{
    ThrowThing = -1,
    Melee = 0,
    Pistol = 1,
    Rifle = 2,
    Sinper = 3,

    Gun = 4,
    Rocket = 5,
}

public class ImageTargetFollower : MonoBehaviour
{
    public WeaponType wType;

    public bool isActiveFromVuforia = false;

    protected Renderer rendererComponent;

    private void Awake()
    {
        rendererComponent = this.gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        checkThisRenderer();
    }

    //렌더가 켜졌는지 검사하는 함수
    void checkThisRenderer()
    {
        if(rendererComponent.enabled)
        {
            isActiveFromVuforia = true;
        }
        else
        {
            isActiveFromVuforia = false;
        }
    }
    
    //트렌스폼을 보내주는데, 렌더가 안켜졌으면 Null반환
    public Transform GetFollowerTransform()
    {
        if(isActiveFromVuforia)
        {
            return this.gameObject.transform;
        }
        else
        {
            print(this.gameObject.name + "isn't active from Vuforia.");
            return null;
        }

    }
}
