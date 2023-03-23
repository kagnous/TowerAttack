using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Casern : Entity
{
    [Tooltip("L'unit�e cr��e")]
    public GameObject unit;
    [Tooltip("Coordon�es d'apparition de l'unit�")]
    public Transform spawnPoint;
    [Tooltip("Cout de l'unit�"), Min(0)]
    public int cost;
    [Tooltip("Temps minimal entre deux achats d'unit�"), Min(0)]
    public float colldown = 0f;

    private float timer = 100f;

    public void BuyUnit()
    {
        if (unit)
        {
            GameObject newUnit = Instantiate(unit, spawnPoint.position, spawnPoint.rotation);
            newUnit.GetComponent<EntityMovableController>().globalTarget = spawnPoint.gameObject;
            timer = 0;
        }
        else
        {
            Debug.LogWarning("AUCUNE UNITE RENSEIGNEE DANS LA CASERNE");
        }
    }

    public override void OnClick()
    {
        if (timer >= colldown)
        {
            if (RessourcesManager.Instance.TryBuy(cost))
            {
                BuyUnit();
            }
        }
    }

    public override void OnAltClick() { }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}
