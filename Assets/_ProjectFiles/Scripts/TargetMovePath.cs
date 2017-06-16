using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovePath : MonoBehaviour {

    public bool canMoveTarget = false;
    public List<Transform> path;
    public GameObject target;


    List<GameObject> temp = new List<GameObject>();
    bool once = true;
    int index = 0;

    // Use this for initialization
    void Start() {
        temp.Add(Instantiate(target, path[index].position, path[index].rotation, this.transform.parent));
        index++;
    }

    void FixedUpdate() {
        if (canMoveTarget)
        {
            foreach (GameObject item in temp)
            {
                if (item.GetComponentInChildren<TargetScript>().movSpeed > 1)
                {
                    item.GetComponentInChildren<TargetScript>().movSpeed = 0.015f;
                    print(item.gameObject.name + " movSpeed reset!");
                }

                if (item.GetComponentInChildren<TargetScript>().isHit == false)
                {
                    //path의 갯수는 언제든지 마뀔 수 있음,                   // index=1
                    //path[1]보다 작아지면(왼쪽으로 가면) path[0]으로 간다
                    if (path[index].position.x < item.transform.position.x)
                    {
                        item.transform.Translate(Vector3.left * item.GetComponentInChildren<TargetScript>().movSpeed);
                    }
                    else
                    {
                        
                        item.transform.position = path[0].position;
                    }

                }
                else
                {
                    if (once)
                    {
                        StartCoroutine(addTarget(item.GetComponentInChildren<TargetScript>().randomTime));
                    }
                }
            }
        }
        
    }

    IEnumerator addTarget(float waitingTime)
    {
        once = false;
        yield return new WaitForSeconds(waitingTime);
        if (temp.Count < 4)
        {
            temp.Add(Instantiate(target, path[index].position, path[index].rotation, this.transform.parent));
        }
        once = true;
    }
}
