using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected bool _interractable = true;
    public bool Interractable => _interractable;

    public abstract void OnClick();

    public abstract void OnAltClick();
}
