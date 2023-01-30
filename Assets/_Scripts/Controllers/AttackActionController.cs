using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActionController : ActionController
{
    private AttackActionData _attackActionData; public AttackActionData AttackActionData { get { return _attackActionData; } }

    /// <summary>
    /// L'entité qui attaque
    /// </summary>
    private EntityController _currentEntity;

    /// <summary>
    /// Cible actuelle
    /// </summary>
    private EntityController _currentTarget;

    public GameObject prefabAttack;
    public Transform originAttack;

    public override void Awake()
    {
        _currentEntity = GetComponent<EntityController>();

        _attackActionData = (AttackActionData)_currentEntity.Datas.Actions[0];

        if (originAttack == null)
        {
            originAttack = transform;
            Debug.Log("No Origin Attack ", gameObject);
        }

        //Dans le script mère, on appelle GetData pour set le TimeToDoAction
        base.Awake();
    }

    public override ActionData GetData()
    {
        return _currentEntity.Datas.Actions[0];
    }

    public override void UpdateAction()
    {
        // S'il y a une target, on test si la target est toujours à bonne distance pour agir
        if (_currentTarget != null)
        {
            if (Vector3.Distance(originAttack.position, _currentTarget.transform.position) > _attackActionData.RangeDo)
            {
                _currentTarget = null;
            }
        }

        // Si pas de target on cherche une nouvelle target
        if (_currentTarget == null)
        {
            _currentTarget = DetectAroundEntity(_attackActionData.RangeDo);
        }

        // Si on a une target on update l'action
        if (_currentTarget != null)
        {
            base.UpdateAction();
        }
        // Sinon on réinitialise
        else
        {
            ResetAction();
        }
    }

    private EntityController DetectAroundEntity(float rangeDetect)
    {
        RaycastHit[] hits = Physics.CapsuleCastAll(originAttack.position, originAttack.position, rangeDetect, Vector3.up, 0);//, _maskLayer    <- ajouer à la fin pour test les types de cibles selon le layer

        EntityController newTarget = null;

        foreach (RaycastHit hit in hits)
        {
            // Test si c'est pas lui même qu'il capte
            if (hit.transform == transform)
            {
                continue;
            }

            EntityController toTestNewTarget = hit.transform.GetComponent<EntityController>();

            // Test si bien une entity
            if (toTestNewTarget == null)
            {
                continue;
            }

            // Test l'alignement
            if (_currentEntity.Faction == toTestNewTarget.Faction)
            {
                continue;
            }

            // Si c'est la 1ère target du raycast on l'assigne comme cible
            if (newTarget == null)
            {
                newTarget = toTestNewTarget;
                continue;
            }

            // Test si elle est plus proche qu'une potentielle autre cible
            if (Vector3.Distance(originAttack.position, toTestNewTarget.transform.position) 
                    < Vector3.Distance(originAttack.position, newTarget.transform.position))
            {
                newTarget = toTestNewTarget;
                continue;
            }
        }

        return newTarget;
    }

    protected override void DoAction()
    {
        // Si il a bien une cible (normalement c'est obligé pour arriver ici)
        if (_currentTarget != null)
        {
            transform.LookAt(_currentTarget.transform);
            // Si il y a une munition
            if (prefabAttack != null)
            {
                // On l'instancie
                GameObject newProjectile = Instantiate(prefabAttack);

                // On la positionne
                newProjectile.transform.position = originAttack.position;

                // On récupère le component Projectile
                Projectile projectileCompo = newProjectile.GetComponent<Projectile>();

                // Si il existe
                if (projectileCompo)
                {
                    // On lui donne une cible
                    projectileCompo.InitTarget(_currentTarget);
                    // On lui donne ces dégâts
                    projectileCompo.damage = _attackActionData.Damage;
                }
            }
            // Sinon juste il fait les dégâts (coup au càc notemment)
            else
            {
                _currentTarget.ApplyDamage(_attackActionData.Damage);
            }
        }

        ResetAction();
    }

    public GameObject DetectNewTarget()
    {
        EntityController detected = DetectAroundEntity(_attackActionData.RangeDetect);
        if (detected)
        {
            return detected.gameObject;
        }
        return null;
    }
}
