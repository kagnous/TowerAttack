using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    [Header("AI Navigation")]
    [Tooltip("Point d'apparition des ennemis")]
    public Transform unitsSpawn;
    [Tooltip("Destination par defaut des ennemis")]
    public GameObject globalAITarget;

    [Header("Prefab AI")]
    public GameObject unitsCac;
    public GameObject unitsRange;
    public GameObject unitsGlassCanon;
    public GameObject unitsBoss;

    [Header("Effects")]
    [Tooltip("Effet visuel lors de la destruction d'unité")]
    public GameObject destroyEffect;

    // Ajout mob progression
    private int numberWave = 1;
    [Header("Tweak random spawn by wave")]
    [Min(0)]
    public int unitMinAdded = 0;
    [Min(0)]
    public int unitMaxAdded = 5;
    public bool addOneUnitByWave = false;

    private void Start()
    {
        TimeManager.Instance.tickEvent.AddListener(TickConsumeEnergy);
    }

    public IEnumerator WaveAttack(List<GameObject> army, float delay)
    {
        for (int i = 0; i < army.Count; i++)
        {
            GameObject unit = Instantiate(army[i], unitsSpawn.position, unitsSpawn.rotation);
            EntityMovableController controller = unit.GetComponent<EntityMovableController>();
            controller.Faction = Faction.IA;
            controller.globalTarget = globalAITarget;

            // Coloriage
            unit.GetComponent<MeshRenderer>().material.color = Color.red;
            MeshRenderer[] meshes = unit.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.material.color = Color.red;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    public void PrepareWave(bool isSuperNight)
    {
        PlayerProfile playerProfile = IdentifyPlayerProfile();

        List<GameObject> AIarmy = new List<GameObject>();

        #region Mirror AI Army
        for (int i = 0; i < playerProfile.units.Count; i++)
        {
            GameObject unitToBuild;

            switch (playerProfile.units[i].Datas.Type)
            {
                case EntityType.Default:
                    Debug.Log("Une entité du player n'a pas de type assigné");
                    continue;
                case EntityType.Cac:
                    unitToBuild = unitsCac;
                    break;
                case EntityType.Range:
                    unitToBuild = unitsRange;
                    break;
                case EntityType.GlassCanon:
                    unitToBuild = unitsGlassCanon;
                    break;
                case EntityType.Tower:
                    continue;
                default:
                    Debug.Log("Une entité du player n'a pas de type assigné");
                    continue;
            }

            // On répète 2 fois car profil de base = 2x plus
            for (int j = 0; j < 2; j++)
            {
                AIarmy.Add(unitToBuild);
            }
        }
        #endregion

        #region Additionnal Units
        //Ajout d'unités en plus
        int tmp = Random.Range(unitMinAdded, unitMaxAdded+1);

        // Ajout potentiel d'une unité par wave
        if(addOneUnitByWave) { tmp += numberWave; }

        //Ajout des unités random supplémentaires
        for (int k = 0; k < tmp; k++)
        {
            int tmp2 = Random.Range(0, 2);
            switch (tmp2)
            {
                case 0:
                    AIarmy.Add(unitsCac);
                    break;
                case 1:
                    AIarmy.Add(unitsRange);
                    break;
                case 2:
                    AIarmy.Add(unitsGlassCanon);
                    break;
                default:
                    break;
            }
        }

        numberWave++;

        // Ajout du boss
        if (isSuperNight)
        {
            AIarmy.Add(unitsBoss);
        }

        #endregion

        // Au cas ou la wave est vide
        if (AIarmy.Count <= 0) 
        {
            AIarmy.Add(unitsCac);
        }

        // Une seconde de delai car baseProfil
        StartCoroutine(WaveAttack(AIarmy, 1f));
    }

    public void DestroyEntity(GameObject toDestroy)
    {
        if (toDestroy.TryGetComponent(out EntityController entity))
        {
            RessourcesManager.Instance.scraps += entity.Datas.ScrapsValue;
            if(entity.Faction == Faction.Player)
            {
                RessourcesManager.Instance.CalculEnergyCost();
            }
        }

            //Debug.Log("Destroy " + toDestroy.name);
        Destroy(toDestroy);
        Instantiate(destroyEffect, toDestroy.transform.position, toDestroy.transform.rotation);
    }

    public PlayerProfile IdentifyPlayerProfile()
    {
        EntityMovableController[] allUnits = FindObjectsOfType<EntityMovableController>();

        List<EntityMovableController> units = new List<EntityMovableController>();
        for (int i = 0; i < allUnits.Length; i++)
        {
            if (allUnits[i].Faction == Faction.Player)
                units.Add(allUnits[i]);
        }
        return new PlayerProfile(PlayerType.baseProfile, units);
    }

    public void TickConsumeEnergy()
    {
        //Appelle la fonction de réduction d'énergie, qui retourne vrai si l'énergie est à 0 après réduction
        if(RessourcesManager.Instance.RemoveEnergie(RessourcesManager.Instance._energyConsumed))
        {
                //Debug.Log("Manque d'energie");
        }

        // TMP A ENLEVER !!!!!!!!!!!!!!!
        //RessourcesManager.Instance.CalculEnergyCost();
    }
}

public struct PlayerProfile
{
    public PlayerType playerType;
    public List<EntityMovableController> units;

    public PlayerProfile(PlayerType playerType, List<EntityMovableController> units)
    {
        this.playerType = playerType;
        this.units = units;
    }
}

public enum PlayerType
{
    baseProfile,
    greedy
}