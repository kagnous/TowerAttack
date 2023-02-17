using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class PriorityController : MonoBehaviour
{
    private const int CAC_DEFAULT_PRIORITY = 5;
    private const int RANGE_DEFAULT_PRIORITY = 1;
    private const int BARRICADE_DEFAULT_PRIORITY = 10;
    private const int GLASSCANON_DEFAULT_PRIORITY = 7;
    private const int TOWER_DEFAULT_PRIORITY = 3;

    private Dictionary<EntityType, int> _priorities = new Dictionary<EntityType, int>();
    public Dictionary<EntityType, int> Priorities => _priorities;

    private void Awake()
    {
        LoadPriorities();
    }

    public void LoadPriorities()
    {
        _priorities.Clear();

        _priorities.Add(EntityType.Default, -1);
        _priorities.Add(EntityType.Cac,CAC_DEFAULT_PRIORITY);
        _priorities.Add(EntityType.Range,RANGE_DEFAULT_PRIORITY);
        _priorities.Add(EntityType.GlassCanon, GLASSCANON_DEFAULT_PRIORITY);
        _priorities.Add(EntityType.Barricade,BARRICADE_DEFAULT_PRIORITY);
        _priorities.Add(EntityType.Tower,TOWER_DEFAULT_PRIORITY);

        //Modifier les priorités selon le pattern

        // Trie par ordre le dico et range les types dans la liste de priorité
        foreach (KeyValuePair<EntityType, int> typeValue in _priorities.OrderBy(key => key.Value)) ;
    }
}
