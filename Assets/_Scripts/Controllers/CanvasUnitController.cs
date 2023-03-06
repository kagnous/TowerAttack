using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CanvasUnitController : MonoBehaviour
{
    [SerializeField]
    private Slider _lifeBar;
    [SerializeField]
    private Image _fillLifeBar;

    [SerializeField]
    private Text _level;

    public Color PlayerColor;
    public Color AIColor;

    private EntityController entity;

    private void Start()
    {
        // Recup entity
        entity = GetComponentInParent<EntityController>();
        if (!entity)
        {
            Debug.LogWarning("Barre de vie sans EntityController");
            return;
        }

        // Setup barre de vie
        if (_lifeBar != null)
            _lifeBar = GetComponentInChildren<Slider>();

        _lifeBar.maxValue = entity.CurrentLife;
        _lifeBar.value = entity.CurrentLife;

        // Setup levelDisplay
        if (_level)
        {
            _level.text = $"Lv {entity.Level}";
        }


        SetUIColor();

        // Abonnement aux events du controller
        entity.damageEvent.AddListener(SetLifeValue);
        entity.healEvent.AddListener(SetLifeValue);
        entity.hackEvent.AddListener(SetUIColor);
    }

    /// <summary>
    /// Met à jours l'affichage des pvs
    /// </summary>
    private void SetLifeValue()
    {
        _lifeBar.value = entity.CurrentLife;
    }

    /// <summary>
    /// Set la couleur de l'UI selon la faction
    /// </summary>
    private void SetUIColor()
    {
        if (entity.Faction == Faction.Player)
        {
            _fillLifeBar.color = PlayerColor;
            _level.color = PlayerColor;
        }
        else if (entity.Faction == Faction.IA)
        {
            _fillLifeBar.color = AIColor;
            _level.color = AIColor;
        }
    }
}