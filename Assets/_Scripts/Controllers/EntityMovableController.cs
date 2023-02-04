using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EntityMovableController : EntityController
{
    private EntityMoveableData _moveableData;

    private NavMeshAgent _navMeshAgent;

    [Header("Move Properties")]
    public GameObject globalTarget;

    [SerializeField]
    private GameObject _currentTargetToMove;

    private List<AttackActionController> _attackActionControllers;

    public override void Awake()
    {
        base.Awake();

        if (Datas is EntityMoveableData)
        {
            _moveableData = (EntityMoveableData)Datas;
        }
        else
        {
            Debug.LogError("Not Valid Entity Data - Need Moveable", gameObject);
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _moveableData.Speed;

        _attackActionControllers = new List<AttackActionController>();
        foreach (ActionController actionController in actionControllers)
        {
            if (actionController is AttackActionController)
            {
                _attackActionControllers.Add((AttackActionController)actionController);
            }
        }
    }

    public override void Update()
    {
        // Recuperation d'une destination
        _currentTargetToMove = GetCurrentTarget();

            //Debug.Log(_navMeshAgent.path.status);


        // Update de la destination
        if (_currentTargetToMove != null)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.stoppingDistance = GetMaxDistanceStop();
            _navMeshAgent.SetDestination(_currentTargetToMove.transform.position);
        }
        else
        {
            _navMeshAgent.stoppingDistance = 1f;
            
            _navMeshAgent.SetDestination(globalTarget.transform.position);
            
            if(_navMeshAgent.remainingDistance < 1)
            {
                Heal(Datas.Life);
            }
        }

        base.Update();
    }

    private float GetMaxDistanceStop()
    {
        float maxDistance = -1;
        foreach (AttackActionController attackActionController in _attackActionControllers)
        {
            if (maxDistance == -1 || maxDistance < attackActionController.AttackActionData.RangeDo)
            {
                maxDistance = attackActionController.AttackActionData.RangeDo;
            }
        }

        if (maxDistance == -1)
        {
            maxDistance = 1;
        }
        return maxDistance;
    }

    private GameObject GetCurrentTarget()
    {
        //Pour chaque attaque (on en a qu'une pour l'instant)
        foreach (AttackActionController attackActionController in _attackActionControllers)
        {
            // On tente de d�tecter une nouvelle cible
            GameObject newTarget = attackActionController.DetectNewTarget();
            if (newTarget)
            {
                // On cr�e un chemin avec la cible
                NavMeshPath path= new NavMeshPath();
                _navMeshAgent.CalculatePath(newTarget.transform.position, path);

                // Si ce chemin n'est pas complet
                if(path.status != NavMeshPathStatus.PathComplete)
                {
                    // On r�cup�re le type de la target
                    EntityType type = newTarget.GetComponent<EntityController>().Datas.Type;

                    // Si c'est une tour ou une barricade, alors c'est bon et on peut garder
                    if (type == EntityType.Tower | type == EntityType.Barricade)
                    {
                        return newTarget;
                    }
                }
                //Et si le chemin est complet alors c'est bon
                else
                {
                    return newTarget;
                }
            }
        }

        return null;
    }
}
