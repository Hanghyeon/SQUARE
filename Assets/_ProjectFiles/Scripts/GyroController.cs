using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GyroController : MonoBehaviour {

    private Quaternion initialRotation = Quaternion.identity;
    public static Quaternion rot;

    Quaternion temp = Quaternion.identity;

	// Update is called once per frame
	void Update () {
        
        Input.gyro.enabled = true;
        var att = Input.gyro.attitude * initialRotation;
        att = new Quaternion(att.x, att.y, -att.z, -att.w);         // att+att=360      -> 위에 att랑 아래 att를 더하면 360 or 0 나옴
        rot = Quaternion.Euler(90, 0, 0) * att;
        //보고있는 방향을 보정해 주려면 rot와 무언가를 더하거나 곱해줘야 됨
        
        this.gameObject.transform.rotation = FixGyro(rot);
    }


    Quaternion FixGyro(Quaternion nonfixed)
    {
        Quaternion nonfix = nonfixed;

        if (TargetSender.Singleton._Active)
        {
            temp = Quaternion.Euler((Vector3.zero - this.gameObject.transform.rotation.eulerAngles) + temp.eulerAngles);
        }

        return Quaternion.Euler(nonfix.eulerAngles + temp.eulerAngles);
    }



}


