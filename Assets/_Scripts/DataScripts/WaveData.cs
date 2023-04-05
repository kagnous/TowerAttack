using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty Wave", menuName = "TowerAttack/Wave")]
public class WaveData : ScriptableObject
{
    [Tooltip("Points d'apparitions des ennemis")]
    public List<Vector3> unitsSpawns;

    public List<GameObject> army;
}
