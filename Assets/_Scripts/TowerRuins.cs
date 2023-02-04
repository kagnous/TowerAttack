using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerRuins : MonoBehaviour
{
    public GameObject towerPrefab;

    private GameObject tower = null; public GameObject Tower => tower;

    public UnityEvent spawnTower;

    public void BuildTower()
    {
        if (towerPrefab && tower == null)
        {
            tower = Instantiate(towerPrefab, new Vector3(transform.position.x, transform.position.y +1.2f, transform.position.z), transform.rotation);
            spawnTower.Invoke();
        }
    }
}
