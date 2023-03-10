using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : Entity
{
    public int _cost = 100;

    private GameObject _barricade;
    public GameObject barricadePrefab;

    [SerializeField, Tooltip ("Tour à laquelle la arricade est rattachée")]
    private StructureRuins towerRef;

    public bool _canBuild = false;

    private void Start()
    {
        if(towerRef)
        {
            GetComponent<MeshRenderer>().enabled = false;
            towerRef.spawnStructure.AddListener(CanBuild);
        }
    }

    public void BuildBarricade()
    {
        _barricade = Instantiate(barricadePrefab, transform.position, Quaternion.identity);
        _barricade.GetComponent<EntityController>().destroyEvent.AddListener(DestroyBarricade);
        CanBuild();
    }

    private void CanBuild()
    {
        if(_barricade == null && towerRef.Structure != null)
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

    // Intermédiaire nécessaire, car le script capte pas de suite que la barricade n'existe plus une fois détruite
    private void DestroyBarricade()
    {
        _barricade = null;
        CanBuild();
    }

    public override void OnClick()
    {
        if (_canBuild)
        {
            if (RessourcesManager.Instance.TryBuy(_cost))
            {
                BuildBarricade();
            }
        }
    }

    public override void OnAltClick() { }
}
