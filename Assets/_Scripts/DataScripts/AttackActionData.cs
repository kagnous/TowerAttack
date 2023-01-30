using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "TowerAttack/Action/New Attack")]
public class AttackActionData : ActionData
{
    [SerializeField, Tooltip("Dégâts infligés")]
    private int _damage = 0;    public int Damage { get { return _damage; } }

    [SerializeField, Min(0), Tooltip("Portée à laquelle l'attaque à lieu")]
    private float _rangeDo = 0; public float RangeDo { get { return _rangeDo; } }

    [SerializeField, Min(0), Tooltip("Portée à partir de laquelle l'unité capte une cible")]
    private float _rangeDetect = 0; public float RangeDetect { get { return _rangeDetect; } }
}
