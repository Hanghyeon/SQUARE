using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Node : MonoBehaviour {

    public GameObject nodeOBJ;
    public Transform nodeTrans;
    public bool setFreeze = false;
    public float lessTime = 0.1f;
    public float movSpeed = 2.5f;

    private void Awake()
    {
        nodeOBJ = this.gameObject;
        nodeTrans = this.transform;
    }
}
