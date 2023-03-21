using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HackingManager : MonoBehaviour
{
    [SerializeField, Tooltip("Valeur moyenne de chance de hack\n0 = automatique, 20 = 1 chance sur 20 ect...")]
    private int _hackingChance = 999999;

    public GameObject hackingEffect;

    private void Start()
    {
        TimeManager.Instance.hackEvent.AddListener(TryHackAll);
    }

    public void TryHackAll()
    {
        EntityController[] entities = FindObjectsOfType<EntityController>();
        foreach (EntityController entity in entities)
        {
            if (entity.Faction == Faction.Player && entity.CanHacked && entity.Datas.Type != EntityType.Tower && entity.Datas.Type != EntityType.Barricade)
            {
                TryHack(entity);
            }
        }
    }

    private void TryHack(EntityController entity)
    {
        if (Random.Range(0, _hackingChance) == 0)
        {
            entity.Hacking();

            // Particule
            if (hackingEffect)
                Instantiate(hackingEffect, entity.transform.position, entity.transform.rotation);

            Debug.Log(entity + " a été hackée");
        }
    }
}
