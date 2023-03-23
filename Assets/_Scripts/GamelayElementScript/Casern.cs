using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Casern : Entity
{
    [Tooltip("L'unitée créée")]
    public GameObject unit;
    [Tooltip("Coordonées d'apparition de l'unité")]
    public Transform spawnPoint;
    [Tooltip("Cout de l'unité"), Min(0)]
    public int cost;
    [Tooltip("Temps minimal entre deux achats d'unité"), Min(0)]
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
