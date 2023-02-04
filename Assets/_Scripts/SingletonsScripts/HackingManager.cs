using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingManager : MonoBehaviour
{
    private void Start()
    {
        TimeManager.Instance.tickEvent.AddListener(TryHackAll);
    }

    public void TryHackAll()
    {
        EntityController[] entities = FindObjectsOfType<EntityController>();
        foreach (EntityController entity in entities)
        {
            if(entity.Faction == Faction.Player)
            {
                TryHack(entity);
            }
        }
    }

    private void TryHack(EntityController entity)
    {
        if(Random.Range(0,100) == 1)
        {
            entity.Faction = Faction.IA;
            Debug.Log(entity + " a été hackée");
        }
    }
}
