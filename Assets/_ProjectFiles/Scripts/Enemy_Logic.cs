using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Died = -1,
    None = 0,
    Hide = 1,
    Appear = 2,
    Idle = 3,
    Aiming = 4,
    Shot = 5
}



public class Enemy_Logic : MonoBehaviour {

    public enum SearchNodeMod
    {
        Near = 0,
        Set = 1
    }

    [SerializeField]
    EnemyState es = EnemyState.None;
    [SerializeField]
    Enemy_Gun e_gun = null;
    [SerializeField]
    CustomMover Mover = null;
    Animator e_anim;

    public SearchNodeMod searchNmod = SearchNodeMod.Set;
    [SerializeField]
    int nodeNum;

    public bool Hit { get { return isHit; } set { isHit = value; } }
    [SerializeField]
    private bool isHit = false;

    Transform spine = null;

    bool flag = true;
    //bool isAiming = false;
    //bool isAppear = false;
    //bool isShoot = false;

    private void Awake()
    {
        //경로
        //Enemy/SimplePeople_Police_Black/
        //Hips_jnt/Spine_jnt/Spine_jnt 1/Chest_jnt/Shoulder_Right_jnt/Arm_Right_jnt/Forearm_Right_jnt/Hand_Right_jnt/
        //Pistol /Gun
        Transform tf = this.transform.Find("Hips_jnt/Spine_jnt/Spine_jnt 1/Chest_jnt/Shoulder_Right_jnt/Arm_Right_jnt/Forearm_Right_jnt/Hand_Right_jnt/Pistol/Gun");
        if (tf != null)
            e_gun = tf.gameObject.GetComponent<Enemy_Gun>();
        else
            print("ERROR~!!!!!, e_gun is NULL~!!!!");

        e_anim = GetComponent<Animator>();

        spine = this.transform.Find("Hips_jnt/Spine_jnt/Spine_jnt 1");

        //Awake에서 Rail객체가 설정되면 한번만 불러줌
        CustomMover.OnSetRail += initMover;

        //노드에 도착할 때, state 바꿀거 있는지 검사
        CustomMover.OnArrived += ChageState;
    }


    // Use this for initialization
    void Start () {
        es = EnemyState.Hide;
        if (Mover == null)
            initMover();
    }
	
	// Update is called once per frame
	void Update () {
        ChageState();
    }



    void ChageState()
    {
        if(isHit)
        {
            Died();
        }

        //플레이어가 주변에 오면
        if (Mover.getCurrentSeg == nodeNum)
        {
            Appear();
        }

        if (e_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && e_gun.currentAmmo != 0)
        {
            Aiming();
        }

        if (e_anim.GetCurrentAnimatorStateInfo(0).IsName("Aim") && e_gun.currentAmmo != 0)
        {
            e_gun.ShootPistol = true;
            Shot();
        }

        if (e_anim.GetCurrentAnimatorStateInfo(0).IsName("Aim") && e_gun.currentAmmo == 0)
        {
            e_gun.ShootPistol = false;
            Idle();
        }

        if (e_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && e_gun.currentAmmo == 0)
        {
            Hide();
        }
    }

    void Died()
    {
        releaseAnim();
        e_anim.Play("Died");
    }

    void Appear()
    {
        releaseAnim();
        e_anim.SetBool("Appear", true);
    }

    void Hide()
    {
        releaseAnim();
        e_anim.SetBool("Hiding", true);
    }

    void Idle()
    {
        releaseAnim();
        e_anim.SetBool("Idle", true);
    }

    void Aiming()
    {
        releaseAnim();
        e_anim.SetBool("Aim", true);
    }

    void Shot()
    {
        if (e_gun._Shooting)
            e_anim.SetBool("Shoot", true);
        else
            releaseAnim();
    }

    void releaseAnim()
    {
        e_anim.SetBool("Aim", false);
        e_anim.SetBool("Idle", false);
        e_anim.SetBool("Appear", false);
        e_anim.SetBool("Shoot", false);
        e_anim.SetBool("Idle", false);
    }


    void initMover()
    {
        GameObject go = GameObject.Find("CameraGroup_Done");

        if (go != null)
            Mover = go.GetComponent<CustomMover>();
        else
            print("ERROR~!!!!!, Player is NULL~!!!!");

        switch (searchNmod)
        {
            case SearchNodeMod.Near:
                nodeNum = SearchNearNode();
                break;
            case SearchNodeMod.Set:
                nodeNum = SetNode();
                break;
        }
    }

    //Enemy의 부모로 있는 Node의 인덱스를 전달해줌
    int SetNode()
    {
        if (Mover != null)
        {
            //Enemy객체 위에 있는 객체에 접근
            Transform tf = this.transform.Find("../../");
            if (tf != null)
            {
                return Mover.rail.nodes.IndexOf(tf.GetComponent<Node>());
            }
            else
            {
                //Enemy객체 위에 객체가 없으면 검색 모드를 Near로 변경 후 해당 함수 실행
                print("ERROR~!!!!!, this gobj don't have parents/parents~!!!!");
                searchNmod = SearchNodeMod.Near;
                print("WARNING~!!!!!, SetNode() change to SearchNmod~!!!!");
                print("WARNING~!!!!!, SetNode() replace to work SearchNearNode()~!!!!");
                return SearchNearNode();

            }
        }
        else
        {
            print("ERROR~!!!!!, SetNode method get Player(=null)~!!!!");
            return -1;
        }



    }

    //에너미의 주변에서 제일 가까운 노드의 인덱스를 받기 위해서 사용
    int SearchNearNode()
    {
        if (Mover != null)
        {
            int temp = 0;
            List<Node> nodeGroup = new List<Node>();
            nodeGroup.AddRange(Mover.rail.nodes);
            for (int index = 0; index < nodeGroup.Count; index++)
            {
                if (index < nodeGroup.Count - 1)
                {
                    //  A - A` = A`에서 A까지의 거리
                    float distanceA = (nodeGroup[index].transform.position - this.transform.position).magnitude;
                    float distanceB = (nodeGroup[index + 1].transform.position - this.transform.position).magnitude;
                    float minDistance = (nodeGroup[temp].transform.position - this.transform.position).magnitude;
                    if (minDistance > distanceA || minDistance > distanceB)
                    {
                        // 값이 작은놈을 저장
                        if (distanceA > distanceB)
                        {
                            temp = index + 1;
                        }
                        else if (distanceA < distanceB)
                        {
                            temp = index;
                        }
                        else
                        {
                            temp = index;
                        }
                    }
                }
            }
            print(nodeGroup[temp].gameObject.name);
            return temp;
        }
        else
        {
            print("ERROR~!!!!!, SearchNearNode method get Player(=null)~!!!!");
            return -1;
        }
    }

}
