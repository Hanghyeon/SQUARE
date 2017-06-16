using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour {

    public GameObject head;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 5F;
    public float sensitivityY = 5F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

	// Update is called once per frame
	void FixedUpdate () {
        RotateMouse();
    }

    void RotateMouse()
    {
        // 카메라가 돌게 아니라 최상위 객체(Player)가 돌아야됨
        //camera.transform.rotation = Quaternion.Euler(Input.mousePosition.y * mouseSensiX * -1f, Input.mousePosition.x * mouseSensiY, 0f);

        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = head.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            head.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
        }
        else if (axes == RotationAxes.MouseX)
        {
            head.transform.Rotate(0f, Input.GetAxis("Mouse X") * sensitivityX, 0f);
        }
        else  //(axes == RotationAxes.MouseY)
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            head.transform.localEulerAngles = new Vector3(0f, head.transform.localEulerAngles.y, -rotationY);
        }
    }

}
