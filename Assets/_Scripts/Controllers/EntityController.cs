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

public class EntityController : MonoBehaviour
{
    [Header("Entity Properties")]
    private int _currentLife = 0; public int CurrentLife => _currentLife;

    private Slider _lifeBar;

    [SerializeField]
    private EntityData _datas; public EntityData Datas => _datas;

    [SerializeField]
    private Faction _faction;   public Faction Faction { get { return _faction; } set { _faction = value; } }

    [SerializeField, Tooltip("Si l'unité peut être hackée ou non")]
    private bool _canHacked = true; public bool CanHacked { get { return _canHacked; } set { _canHacked = value; } }

    protected ActionController[] actionControllers;

    public UnityEvent destroyEvent;

    public virtual void Awake()
    {
        _currentLife = _datas.Life;
        actionControllers = GetComponents<ActionController>();

        _lifeBar = GetComponentInChildren<Slider>();
        if(_lifeBar != null ) { _lifeBar.maxValue = _currentLife; _lifeBar.value = _currentLife; }
        
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

        if (_lifeBar != null)
            _lifeBar.value = _currentLife;

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

        if (_lifeBar != null)
            _lifeBar.value = _currentLife;
    }

    public bool IsValidEntity()
    {
        return gameObject != null && gameObject.activeSelf && _currentLife > 0;
    }
}
