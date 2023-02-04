using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    private GameObject _barricade;
    public GameObject barricadePrefab;

    [SerializeField, Tooltip ("Tour à laquelle la arricade est rattachée")]
    private TowerRuins towerRef;

    public bool _canBuild = false;

    private void Start()
    {
        if(towerRef)
        {
            GetComponent<MeshRenderer>().enabled = false;
            towerRef.spawnTower.AddListener(CanBuild);
        }
    }

    public void BuildBarricade()
    {
        _barricade = Instantiate(barricadePrefab, transform.position, Quaternion.identity);
        _barricade.GetComponent<EntityController>().destroyEvent.AddListener(CanBuild);
        CanBuild();
    }

    private void CanBuild()
    {
        if(_barricade == null && towerRef.Tower != null)
        {
            _canBuild = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            _canBuild = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
