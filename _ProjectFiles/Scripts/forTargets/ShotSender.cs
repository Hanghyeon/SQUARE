using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShotSender : MonoBehaviour {

    public static ShotSender Singleton;
    public bool isFire = false;

    protected Renderer rendererComponent;
    protected bool isActiveFromVuforia = false;

    public List<GameObject> callOnFire;

    System.Action OnFire;

    

    void Awake () {

        foreach (GameObject item in callOnFire)
        {
            if(item.GetComponent<ShootAtMenu>()!=null)
            {
                ShootAtMenu sam = item.GetComponent<ShootAtMenu>();
                OnFire += sam.shoot;
            }
        }

        Singleton = this;
        rendererComponent = this.gameObject.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        checkThisRenderer();
    }

    protected void checkThisRenderer()
    {
        if (rendererComponent.enabled)
        {
            isActiveFromVuforia = true;
        }
        else
        {
            isActiveFromVuforia = false;
        }


        Fire();
    }

    //Reload 해줌
    public void Fire()
    {
        if (isActiveFromVuforia)
        {
            isFire = true;
            OnFire();
        }
        else
        {
            isFire = false;
        }
    }
}
