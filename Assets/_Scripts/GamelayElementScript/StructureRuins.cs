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

    [Tooltip("MeshRenderers des élements à colorer à la prise de la tour")]
    public List<MeshRenderer> ownedAreaMeshes;

    [Tooltip("Material à appliquer sur les éléments de la prise de tour")]
    public Material ownedAreaMaterial;

    public override void OnClick()
    {
        if (structurePrefab && structure == null)
        {
            if (RessourcesManager.Instance.TryBuy(_cost))
            {
                BuildStructure();
            }
        }
    }

    public override void OnAltClick() { }
    // Trigger pour changer la couleur des murs et build la tour le sang
    public void BuildStructure()
    {
        structure = Instantiate(structurePrefab, new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z), transform.rotation);
        spawnStructure?.Invoke();

        if (ownedAreaMaterial != null)
        {
            for (int i = 0; i < ownedAreaMeshes.Count; i++)
            {
                ownedAreaMeshes[i].material = ownedAreaMaterial;
            }
        }
    }
}
