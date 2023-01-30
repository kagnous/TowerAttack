using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesManager : Singleton<RessourcesManager>
{
    public int scraps = 100;

    public bool TryBuy(int cost)
    {
        if(cost <= scraps)
        {
            scraps -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }
}
