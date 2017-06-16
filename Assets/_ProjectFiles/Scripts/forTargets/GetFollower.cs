using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFollower : MonoBehaviour {

    public float speed = 10f;
    bool isStabilDone = true;
    public ImageTargetFollower follower;
    bool isTargetFollow = false;
    float time = 0f;
    #region etc

    //[Header("Stabilization Options")]
    //public float StabilSensiRot = 1f;     
    //public float StabilSensiPos = 0.1f;   // 유니티에서 1유닛은 1m
    //public float StabilRate = 0.02f;      // 1 sec = 0.01x60
    //public float StabilSpeed = 1f;        // 1초 안에 보간
    //[SerializeField]
    //bool isStabilDone = true;

    //Vector3 NonStabilizedPos;        // 이 두 변수는 매프레임 변함
    //Quaternion NonStabilizedRot;     // 이 두 변수는 매프레임 변함
    //Vector3 StabilizedPos;        // 이 두 변수는 보정 코루틴이 끝난 다음에 변함
    //Quaternion StabilizedRot;     // 이 두 변수는 보정 코루틴이 끝난 다음에 변함


    //public ImageTargetFollower follower;
    //bool isTargetFollow = false;

    //private void Awake()
    //{
    //    StabilizedPos = this.gameObject.transform.position;
    //    StabilizedRot = this.gameObject.transform.rotation;
    //}


    //// Update is called once per frame
    //void Update () {
    //    isTargetFollow = follower.isActiveFromVuforia;
    //    GetFollowerTransform();
    //}

    //private void FixedUpdate()
    //{
    //GetFollowerTransform();
    //}

    //IEnumerator StabilizationHand()
    //{
    //    isStabilDone = false;
    //    Vector3 tempPos = NonStabilizedPos;
    //    Vector3 tempRot = NonStabilizedRot.eulerAngles;

    //    yield return new WaitForSecondsRealtime(StabilRate);
    //    if (StabilSensiPos < Vector3.Distance(tempPos, NonStabilizedPos))
    //    {
    //        StabilizedPos = NonStabilizedPos;
    //    }
    //    if (StabilSensiRot < Vector3.Distance(tempRot, NonStabilizedRot.eulerAngles))
    //    {
    //        StabilizedRot = NonStabilizedRot;
    //    }

    //    isStabilDone = true;
    //}


    //void GetFollowerTransform()
    //{
    //    if (isTargetFollow)
    //    {
    //        NonStabilizedPos = follower.GetFollowerTransform().position;
    //        NonStabilizedRot = follower.GetFollowerTransform().rotation;

    //        //if (isStabilDone)
    //        //{
    //        //    StartCoroutine(StabilizationHand());
    //        //}

    //        StabilizeTransform(StabilizedPos, StabilizedRot);
    //    }
    //    else
    //    {
    //        print("isTargetFollower is false!!!" + " in " + this.gameObject.name);
    //    }
    //}

    #endregion

    private void Awake()
    {
        follower = GameObject.Find("FollowImageTarget/FollowPosSender").GetComponent<ImageTargetFollower>();
    }

    private void Update()
    {
        if (follower.isActiveFromVuforia)
        {
            StabilizeTransform(follower.GetFollowerTransform().position, follower.GetFollowerTransform().rotation);
        }
    }

    void StabilizeTransform(Vector3 pos, Quaternion rot)
    {
        float m = (pos - this.transform.position).magnitude;
        time += (Time.smoothDeltaTime * 1 / m) * speed;
        if (time > 1)
        {
            this.transform.position = pos;
            this.transform.rotation = rot;
            time = 0;
        }
        this.transform.position = Vector3.Lerp(this.transform.position, pos, time);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rot, time);
    }
}
