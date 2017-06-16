using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcode : MonoBehaviour {

    
    public Renderer[] render;

    private void Start()
    {
        foreach (Renderer item in render)
        {
            item.enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {
	    if(TargetSender.Singleton._Active)
        {
            render[1].enabled = false;
            render[0].enabled = true;
        }
        else
        {
            render[0].enabled = false;
            render[1].enabled = true;
        }
	}
}
