using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesManager : Singleton<RessourcesManager>
{
    public int scraps = 100;

    [Header("Energy (WIP)")]
    [Min(0)]
    public int _actualEnergy;
    public int _energyMax;
    public int _energyConsumed;
    public int _energyProduced;

    private void Start()
    {
        TimeManager.Instance.tickEvent.AddListener(TickEnergy);
        CalculEnergyCost();
    }

    public void CalculEnergyCost()
    {
        // Calcul de l'énergie nécessaire
        EntityController[] entities = FindObjectsOfType<EntityController>();
        _energyConsumed = 0;
        foreach (EntityController entity in entities)
        {
            if (entity.Faction == Faction.Player)
            {
                _energyConsumed += entity.Datas.EnergyCost;
            }
        }

        // Calcul de l'énergie max diponible et energie produite par tick le jour
        SolarPanel[] panels = FindObjectsOfType<SolarPanel>();
        _energyMax = _energyProduced = 0;
        foreach(SolarPanel panel in panels)
        {
            if(panel._isActivated)
            {
                _energyMax += 300;
                _energyProduced += 50;
            }
        }
    }

    public void TickEnergy()
    {
        if (TimeManager.Instance._actualTimeOfDay == TimeOfDay.Day)
        {
            AddEnergie(_energyProduced);
        }
    }

    public void AddEnergie(int energy)
    {
        _actualEnergy += energy;
        if (_actualEnergy > _energyMax)
        {
            _actualEnergy = _energyMax;
        }
    }

    public bool RemoveEnergie(int energy)
    {
        _actualEnergy -= energy;
        return _actualEnergy <= 0;
    }

    public bool TryBuy(int cost)
    {
        if (cost <= scraps)
        {
            scraps -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }
}
