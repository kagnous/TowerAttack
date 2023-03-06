using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerRuins : Entity
{
    public GameObject towerPrefab;

    private GameObject tower = null; public GameObject Tower => tower;

    public UnityEvent spawnTower;

    public override void OnClick()
    {
        if(RessourcesManager.Instance.TryBuy(100))
        {
            BuildTower();
        }
    }

    public override void OnAltClick() { }

    public void BuildTower()
    {
        if (towerPrefab && tower == null)
        {
            tower = Instantiate(towerPrefab, new Vector3(transform.position.x, transform.position.y +1.2f, transform.position.z), transform.rotation);
            spawnTower?.Invoke();
        }
    }
}
