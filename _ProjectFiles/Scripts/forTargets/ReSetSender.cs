using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSetSender : MonoBehaviour
{

    public static ReSetSender Singleton;
    private Renderer rendererComponent;

    public bool _Active { get { return isFix; } }
    private bool isActiveFromVuforia;

    bool isFix = true;
    bool flag = true;

    void Awake()
    {
        Singleton = this;
        rendererComponent = this.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
            flag = true;
        }

        if (flag && isActiveFromVuforia)
        {
            StartCoroutine(test());
        }

    }

    IEnumerator test()
    {
        if (flag)
            flag = false;

        isFix = true;
        yield return new WaitForEndOfFrame();
        isFix = false;
    }

}
