using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Default,
    Cac,
    Range,
    GlassCanon,
    Tower,
    Barricade
}

[CreateAssetMenu(fileName = "New Default Entity", menuName = "TowerAttack/Entity/New Entity")]
public class EntityData : ScriptableObject
{
    [Header("ENTITY PROPERTIES")]
    [SerializeField, Min(0)]
    private int _life = 0; public int Life { get { return _life; } }

    //[SerializeField]
    //private ActionData[] _actions = null;
    //public ActionData[] Actions { get { return _actions; } }

    [SerializeField]
    private ActionData _action = null;
    public ActionData Action { get { return _action; } }

    [SerializeField]
    private float _damageModifier = 1;
    public float DamageModifier { get { return _damageModifier; } }

    [SerializeField]
    private EntityType _type = EntityType.Default;
    public EntityType Type { get { return _type; } }

    [SerializeField, Min(0), Tooltip("Nombre de scraps rapporté")]
    private int scrapsValue = 5;
    public int ScrapsValue { get { return scrapsValue; } }

    [SerializeField, Min(0), Tooltip("Consommation d'énergie par tick\nUnités player uniquement")]
    private int energieCost = 0;
    public int EnergyCost { get { return energieCost; } }

    [SerializeField, Tooltip("Liste des potentiels levelUp")]
    private Tiers[] _tiers = null;
    public Tiers[] Tiers { get { return _tiers; } }
}

[System.Serializable]
public struct Tiers
{
    [Tooltip("Prix de l'amélioration")]
    public int cost;
    public int lifeIncrease;
    public int damageModifierIncrease;
    public int energyCostIncrease;
    public int scrapsValueIncrease;
}
