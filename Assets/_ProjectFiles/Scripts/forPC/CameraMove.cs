using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public float Zspeed = 1.3f;
    public float Xspeed = 1.3f;
    public float JumpPow = 1.3f;

    Rigidbody rb = null;
    protected bool canJump = false;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = false;
        }
    }


    // Update is called once per frame
    void FixedUpdate () {
        move();
        jump();
	}

    void move()
    {
        this.gameObject.transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * Zspeed);  //forward & backward
        this.gameObject.transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * Xspeed);  //leftside & rightside
    }

    void jump()
    {
        if(canJump)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * Time.deltaTime * JumpPow * 10000f, ForceMode.Impulse);
            }
        }
    }
}
