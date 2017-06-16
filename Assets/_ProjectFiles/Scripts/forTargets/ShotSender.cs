using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShotSender : MonoBehaviour {

    public bool isFire = false;

    protected Renderer rendererComponent;
    protected bool isActiveFromVuforia = false;

    [SerializeField]
    public List<GameObject> callOnFire = new List<GameObject>();

    static System.Action OnFire;
    [SerializeField]
    GameObject go;
    [SerializeField]
    SwitchWeapons sWeapons = null;

    void Awake() {

        rendererComponent = this.gameObject.GetComponent<Renderer>();

    }


    // Update is called once per frame
    void Update() {
        checkThisRenderer();
    }

    protected void checkThisRenderer()
    {
        if (rendererComponent.enabled)
        {
            isActiveFromVuforia = true;
        }
        else
        {
            isActiveFromVuforia = false;
        }


        Fire();
    }

    public void Fire()
    {
        if (isActiveFromVuforia)
        {

            if (OnFire != null)
                OnFire();
            else
                print("ERROR~!!! ShotSender OnFire Action is Null~!!!!");
        }
        else
        {
            isFire = false;
        }
    }

    public void Init()
    {
        go = GameObject.Find("Weapons");

        OnFire += () => { isFire = true; };

        if (go != null)
            sWeapons = go.GetComponent<SwitchWeapons>();
        else
            print("ERROR~!!! init sWeapons is Null~!!!!");

        if (sWeapons.guns.Count != 0)
        {
            callOnFire.Clear();

            foreach (Transform item in sWeapons.guns)
            {
                callOnFire.Add(item.Find("Gun").gameObject);
            }

            foreach (GameObject item in callOnFire)
            {
                if (item != null)
                {
                    if (item.GetComponent<ShootAtMenu>() != null)
                    {
                        //OnFire += item.GetComponent<ShootAtMenu>().shoot;
                    }
                }
                else
                {
                    print("ERROR~!!! init callOnFire is Null~!!!!");
                }
            }
        }

    }

}    
