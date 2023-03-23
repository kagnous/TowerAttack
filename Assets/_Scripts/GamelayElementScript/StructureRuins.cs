using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StructureRuins : Entity
{
    [Header("CONSTRUCTION :")]
    [Tooltip("Scraps dépensés pour construire le batiment")]
    public int _cost = 100;

    [Tooltip("Prefab du batiment")]
    public GameObject structurePrefab;

    private GameObject structure = null; public GameObject Structure => structure;

    [Tooltip("Event lancé à la construction du batiment")]
    public UnityEvent spawnStructure;
    
    public override void OnClick()

    {
        if(RessourcesManager.Instance.TryBuy(_cost))
        {
            BuildStructure();
        }
    }

    public override void OnAltClick() { }
    // Trigger pour changer la couleur des murs et build la tour le sang
    public void BuildStructure()
    {
        if (structurePrefab && structure == null)
        {
            structure = Instantiate(structurePrefab, new Vector3(transform.position.x, transform.position.y +1.2f, transform.position.z), transform.rotation);
            spawnStructure?.Invoke();
        }
    }
}
