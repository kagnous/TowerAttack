using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public enum Faction
{
    Player,
    IA,
    Neutral
}

public class EntityController : Entity
{
    [Header("Entity Properties")]
    private float _currentLife = 0; public float CurrentLife => _currentLife;

    [SerializeField, Min(1)]
    private int level = 1;  public int Level { get { return level; } }

    [SerializeField]
    private EntityData _datas; public EntityData Datas => _datas;

    [SerializeField]
    private Faction _faction;   public Faction Faction { get { return _faction; } set { _faction = value; } }

    [SerializeField, Tooltip("Si l'unité peut être hackée ou non")]
    private bool _canHacked = true; public bool CanHacked { get { return _canHacked; } set { _canHacked = value; } }

    protected ActionController[] actionControllers;

    public UnityEvent hackEvent;
    public UnityEvent damageEvent;
    public UnityEvent healEvent;
    public UnityEvent destroyEvent;

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

    public void ApplyDamage(float damage)
    {
        _currentLife -= damage;

        damageEvent?.Invoke();

        if (_currentLife <= 0)
        {
            EntityManager.Instance.DestroyEntity(gameObject);
            destroyEvent?.Invoke();
        }
    }

    public void Heal(int heal)
    {
        _currentLife += heal;
        if(_currentLife > _datas.Life)
        {
            _currentLife = _datas.Life;
        }

        healEvent?.Invoke();
    }

    public void Hacking()
    {
        if(_faction == Faction.Player)
        {
            _faction = Faction.IA;
        }
        else if (_faction == Faction.IA)
        {
            _faction = Faction.Player;
        }

        hackEvent?.Invoke();
    }

    public bool IsValidEntity()
    {
        return gameObject != null && gameObject.activeSelf && _currentLife > 0;
    }

    public override void OnClick() { }
    public override void OnAltClick() { }
}
