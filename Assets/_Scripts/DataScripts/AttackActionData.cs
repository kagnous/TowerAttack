using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "TowerAttack/Action/New Attack")]
public class AttackActionData : ActionData
{
    [SerializeField, Tooltip("D�g�ts inflig�s")]
    private float _damage = 0;    public float Damage { get { return _damage; } }

    [SerializeField, Min(0), Tooltip("Port�e � laquelle l'attaque � lieu")]
    private float _rangeDo = 0; public float RangeDo { get { return _rangeDo; } }

    [SerializeField, Min(0), Tooltip("Port�e � partir de laquelle l'unit� capte une cible")]
    private float _rangeDetect = 0; public float RangeDetect { get { return _rangeDetect; } }
}
