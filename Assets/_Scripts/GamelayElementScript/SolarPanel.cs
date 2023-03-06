using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : Entity
{
    public bool _isActivated = false;

    public Material day;
    public Material night;
    private MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void BuildSolarPanel()
    {
        RessourcesManager.Instance.CalculEnergyCost();
        _isActivated = true;
    }

    private void Update()
    {
        if(_isActivated)
        {
            if(TimeManager.Instance._actualTimeOfDay == TimeOfDay.Day)
            {
                mesh.material = day;
            }
            else
            {
                mesh.material = night;
            }
        }
    }

    public override void OnClick()
    {
        if (RessourcesManager.Instance.TryBuy(50))
            BuildSolarPanel();
    }

    public override void OnAltClick() { }
}
