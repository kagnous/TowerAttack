using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    Player,
    IA,
    Neutral
}

public class EntityController : MonoBehaviour
{
    [Header("Entity Properties")]
    private int _currentLife = 0; public int CurrentLife => _currentLife;

    [SerializeField]
    private EntityData _datas; public EntityData Datas => _datas;

    [SerializeField]
    private Faction _faction;   public Faction Faction { get { return _faction; } set { _faction = value; } }

    protected ActionController[] actionControllers;

    public virtual void Awake()
    {
        _currentLife = _datas.Life;
        actionControllers = GetComponents<ActionController>();
    }

    public virtual void Update()
    {
        if (actionControllers != null)
        {
            foreach (ActionController actionController in actionControllers)
            {
                actionController.UpdateAction();
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        _currentLife -= damage;

        if (_currentLife <= 0)
        {
            //Destroy(gameObject);
            EntityManager.Instance.DestroyEntity(gameObject);
        }
    }

    public bool IsValidEntity()
    {
        return gameObject != null && gameObject.activeSelf && _currentLife > 0;
    }
}
