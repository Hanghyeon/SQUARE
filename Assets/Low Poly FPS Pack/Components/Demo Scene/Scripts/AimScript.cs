using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AimScript : MonoBehaviour {

    //---------------------------------Custom Code----------------------------------
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;

    [Header("Mouse Options")]
    public GameObject centerPart;

    public float ZoomSensiX = 1F;
    public float ZoomSensiY = 1F;
    public float NomalSensiX = 5F;
    public float NomalSensiY = 5F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    Rigidbody rb;

    float rotationY = 0F;
    //---------------------------------------------------------------------------------

    float mouseX;
	float mouseY;
	Quaternion rotationSpeed;

	[Header("Gun Options")]
	//How fast the gun moves on the x and y
	//axis when aiming
	public float aimSpeed = 6.5f;
	//How fast the gun moves to the new position
	public float moveSpeed = 15.0f;

	[Header("Gun Positions")]
	//Default gun position
	public Vector3 defaultPosition;
	//Aim down the sight position
	public Vector3 zoomPosition;

	[Header("Camera")]
	//Main gun camera
	public List<Camera> gunCamera;

	[Header("Camera Options")]
	//How fast the camera field of view changes
	public float fovSpeed = 15.0f;
	//Camera FOV when zoomed in
	public float zoomFov = 30.0f;
	//Default camera FOV
	public float defaultFov = 60.0f;

	[Header("Audio")]
	public AudioSource aimSound;
	//Used to check if the audio has played
	bool soundHasPlayed = false;

    void Start () {

		//Hide the cursor at start
		Cursor.visible = false;
	}

	void Update () {

		//When right click is held down
		if(Input.GetButton("Fire2")) {
			//Move the gun to the zoom position
			transform.localPosition = Vector3.Lerp(transform.localPosition, 
			                                       zoomPosition, Time.deltaTime * moveSpeed);
            //Change the camera field of view
            foreach (Camera item in gunCamera)
            {
                item.fieldOfView = Mathf.Lerp(item.fieldOfView,
                                                   zoomFov, fovSpeed * Time.deltaTime);
            }

            //If the aim sound has not played, play it
            if (!soundHasPlayed) {
				aimSound.Play();
				//The sound has played
				soundHasPlayed = true;
			}

		} else {
			//When right click is released
			//Move the gun back to the default position
			transform.localPosition = Vector3.Lerp(transform.localPosition, 
			                                       defaultPosition, Time.deltaTime * moveSpeed);
            //Change back the camera field of view
            foreach (Camera item in gunCamera)
            {
                item.fieldOfView = Mathf.Lerp(item.fieldOfView,
                                               defaultFov, fovSpeed * Time.deltaTime);
            }
            soundHasPlayed = false;
		}

        //      //Rotate the gun based on the mouse input
        //      mouseX = Input.GetAxis("Mouse X") * 360f * Time.deltaTime;
        //      mouseY = Input.GetAxis("Mouse Y") * 360f * Time.deltaTime;

        ////Rotate the gun on the x and y axis
        //rotationSpeed = Quaternion.Euler (-mouseY, mouseX, 0);

        //      //transform.localRotation = rotationSpeed;
        //      transform.localRotation = Quaternion.Slerp
        //          (transform.localRotation, rotationSpeed, aimSpeed * Time.deltaTime);
        RotateMouse();
    }

    void RotateMouse()
    {
        // 카메라가 돌게 아니라 최상위 객체(Player)가 돌아야됨
        //camera.transform.rotation = Quaternion.Euler(Input.mousePosition.y * mouseSensiX * -1f, Input.mousePosition.x * mouseSensiY, 0f);

        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = 0;

            if (Input.GetButton("Fire2"))
            {
                rotationX = centerPart.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * ZoomSensiX;

                rotationY += Input.GetAxis("Mouse Y") * ZoomSensiY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            }
            else
            {
                rotationX = centerPart.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * NomalSensiX;

                rotationY += Input.GetAxis("Mouse Y") * NomalSensiY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            }

            centerPart.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
        }
        else if (axes == RotationAxes.MouseX)
        {
            if (Input.GetButton("Fire2"))
            {
                centerPart.transform.Rotate(0f, Input.GetAxis("Mouse X") * ZoomSensiX, 0f);
            }
            else
            {
                centerPart.transform.Rotate(0f, Input.GetAxis("Mouse X") * NomalSensiX, 0f);
                //head.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
            }
        }
        else  //(axes == RotationAxes.MouseY)
        {
            if (Input.GetButton("Fire2"))
            {

                rotationY += Input.GetAxis("Mouse Y") * ZoomSensiY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            }
            else
            {

                rotationY += Input.GetAxis("Mouse Y") * NomalSensiY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            }
            centerPart.transform.localEulerAngles = new Vector3(-rotationY, centerPart.transform.localEulerAngles.y, 0f);
        }
    }
}