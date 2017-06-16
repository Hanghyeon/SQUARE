using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMover : MonoBehaviour
{
    public static CustomMover Singleton;

    public CustomRail rail = null;
    public PlayMode mode;
    public float speed = 3f;
    //  :WARNING: isReversed has a logical problem. So plz careful when u used.
    public bool isReversed;
    public bool isLooping;
    public bool pingPong;

    public bool getFreeze { get { return freezeTransition; } }
    public int getCurrentSeg { get { return currentSeg; } }

    [SerializeField]
    private bool freezeTransition = false;

    [SerializeField]
    private int currentSeg = 0;
    [SerializeField]
    private float transition;
    [SerializeField]
    private bool isCompleted;

    public bool canMove = false;

    public delegate void CompleteAction();
    public static event CompleteAction OnCompleted;

    public static System.Action OnSetRail;

    private void Awake()
    {
        Singleton = this;

        TargetSender.OnGyroRotSet += (int num) => 
        {
            canMove = true;
        };

        GameObject go = GameObject.Find("RailManager");
        if (go != null)
        {
            rail = go.GetComponent<CustomRail>();
            if (OnSetRail != null)
                OnSetRail();
        }
        else
        {
            print("ERROR~!! Rail is NULL~!!!!!");
        }

       
       

        OnCompleted += () =>
        {
            if (CustomRail.Singleton.nodes[currentSeg].setFreeze)
                freezeTransition = true;
            else freezeTransition = false;
        };


    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)   //일시정지와 같은 역할을 하게 됨, 이동이 멈춤
            return;

        if (!rail)
            return;

        if (!isCompleted)
            Play(!isReversed);

    }





    void Play(bool foward = true)
    {
        speed = rail.nodes[currentSeg].movSpeed;

        //lerp 함수 진행값(float)을 조절하는 부분, OnCompleted 이벤트에서 freeze를 통해 제어함
        if (!freezeTransition)
        {
            float m = (rail.nodes[currentSeg + 1].nodeTrans.position - rail.nodes[currentSeg].nodeTrans.position).magnitude;
            float s = (Time.deltaTime * 1 / m) * speed;
            transition += (foward) ? s : -s;
        }
        else
        {
            freezeTransition = FreezeManager.Singleton.getIsFreeze;
        }

        //인덱스값 변경하고 트렌스폼 주는 부분
        if (transition > 1)
        {
            transition = 0;
            currentSeg++;

            if (OnCompleted != null)
            {
                OnCompleted();
            }

            if (currentSeg == rail.nodes.Count - 1)
            {
                if (isLooping)
                {
                    if (pingPong)
                    {
                        isReversed = !isReversed;
                        transition = 1;
                        currentSeg = rail.nodes.Count - 2;
                    }
                    else
                    {
                        currentSeg = 0;
                    }
                }
                else
                {
                    isCompleted = true;
                    return;
                }
            }
        }
        else if (transition < 0)
        {
            transition = 1;
            currentSeg--;

            if (OnCompleted != null)
            {
                OnCompleted();
            }

            if (currentSeg == -1)
            {
                if (isLooping)
                {
                    if (pingPong)
                    {
                        isReversed = !isReversed;
                        transition = 0;
                        currentSeg = 0;
                    }
                    else
                    {
                        currentSeg = rail.nodes.Count - 2;
                    }
                }
                else
                {
                    isCompleted = true;
                    return;
                }
            }
        }
        
        this.gameObject.transform.position = rail.PositionOnRail(currentSeg, transition, mode);
        this.gameObject.transform.localRotation = rail.Orientation(currentSeg, transition);
        //print(rail.Orientation(currentSeg, transition).eulerAngles);
    }

}
