using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TargetSender : MonoBehaviour {

    public static TargetSender Singleton;
    private Renderer rendererComponent;

    public bool _Active { get { return isFix; } }
    private bool isActiveFromVuforia;

    bool isFix = false;
    bool flag = true;

    public delegate void SetGyroAction(int num = 0);
    public static event SetGyroAction OnGyroRotSet = null;

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
        else
        {
            isFix = false;
        }
    }

    IEnumerator test()
    {
        if (flag)
            flag = false;

        isFix = true;
        if (OnGyroRotSet != null)
            OnGyroRotSet();
        else
            print("ERROR~!! OnGyroRotSet Event have NULL~!!!!!");
        yield return new WaitForEndOfFrame();
    }

}
