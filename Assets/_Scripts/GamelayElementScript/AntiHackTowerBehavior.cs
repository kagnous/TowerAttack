using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiHackTowerBehavior : MonoBehaviour
{
    [SerializeField, Tooltip("Portée du rayon de protection")]
    private float _protectionRange = 1f;

    private void Start()
    {
        ParticleSystem.ShapeModule shape;
        shape = GetComponentInChildren<ParticleSystem>().shape; 
        shape.radius = _protectionRange;

        GetComponent<CapsuleCollider>().radius = _protectionRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<EntityController>(out EntityController entity) )
        {
            if(entity.Faction == Faction.Player)
            {
                entity.CanHacked = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EntityController>(out EntityController entity))
        {
            if (entity.Faction == Faction.Player)
            {
                entity.CanHacked = true;
            }
        }
    }
}
