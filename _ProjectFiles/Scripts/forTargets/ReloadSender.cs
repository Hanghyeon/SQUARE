using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReloadSender : MonoBehaviour {

    public static ReloadSender Singleton;
    public bool isReload = false;

    protected Renderer rendererComponent;
    protected bool isActiveFromVuforia = false;

    private void Awake()
    {
        Singleton = this;
        rendererComponent = this.gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        checkThisRenderer();
    }


    //렌더가 켜졌는지 검사하는 함수
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

        Reload();
    }

    //Reload 해줌
    public void Reload()
    {
        if (isActiveFromVuforia)
        {
            isReload = true;
        }
        else
        {
            isReload = false;
        }
    }
}
