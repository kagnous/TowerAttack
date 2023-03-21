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

    [SerializeField]
    private GameObject mur1;
    [SerializeField]
    private GameObject mur2;
    [SerializeField]
    private GameObject mur3;
    [SerializeField]
    private GameObject mur4;
    [SerializeField]
    private GameObject mur5;
    [SerializeField]
    private GameObject mur6;
    [SerializeField]
    private GameObject mur7;
    [SerializeField]
    private Material wallsAllies;
    [SerializeField]
    private Material wallsEnnemies;
    // Trigger pour changer la couleur des murs pour le start 
    public void Start()
    {
        mur1.GetComponent<MeshRenderer> ().material = wallsEnnemies;
        mur2.GetComponent<MeshRenderer> ().material = wallsEnnemies;
        mur3.GetComponent<MeshRenderer> ().material = wallsEnnemies;
        mur4.GetComponent<MeshRenderer> ().material = wallsEnnemies;
        mur5.GetComponent<MeshRenderer> ().material = wallsEnnemies;
        mur6.GetComponent<MeshRenderer> ().material = wallsEnnemies;
        mur7.GetComponent<MeshRenderer> ().material = wallsEnnemies;
    }

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
            mur1.GetComponent<MeshRenderer> ().material = wallsAllies;
            mur2.GetComponent<MeshRenderer> ().material = wallsAllies;
            mur3.GetComponent<MeshRenderer> ().material = wallsAllies;
            mur4.GetComponent<MeshRenderer> ().material = wallsAllies;
            mur5.GetComponent<MeshRenderer> ().material = wallsAllies;
            mur6.GetComponent<MeshRenderer> ().material = wallsAllies;
            mur7.GetComponent<MeshRenderer> ().material = wallsAllies;
        }
    }
}
