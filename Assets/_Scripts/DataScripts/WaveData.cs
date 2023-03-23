using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty Wave", menuName = "TowerAttack/Wave")]
public class WaveData : ScriptableObject
{
    public List<GameObject> army;
}
