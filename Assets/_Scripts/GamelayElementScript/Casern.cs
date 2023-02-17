using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casern : MonoBehaviour
{
    public GameObject unit;
    public Transform spawnPoint;
    public int cost;

    public void BuyUnit()
    {
        if(unit)
        {
            GameObject newUnit = Instantiate(unit, spawnPoint.position, spawnPoint.rotation);
            newUnit.GetComponent<EntityMovableController>().globalTarget = spawnPoint.gameObject;
        }
    }
}
