using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionData : ScriptableObject
{
    [SerializeField, Min(0), Tooltip("Cooldawn de l'action")]
    private float _timeToDoAction = 0;  public float TimeToDoAction { get { return _timeToDoAction; } }
}
