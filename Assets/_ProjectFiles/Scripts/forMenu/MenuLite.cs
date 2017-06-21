using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLite : MonoBehaviour {

    public MenuType mt = MenuType.None;
    public bool Hit { get { return isHit; } }
    bool isHit = false;

    public void ChangeHit()
    {
        isHit = !isHit;
        print(this.gameObject.name + " is Change " + isHit);
    }
}
