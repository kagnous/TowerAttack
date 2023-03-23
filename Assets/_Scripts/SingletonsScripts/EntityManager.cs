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

    private int numberWave = 1;

    [Header("Wave parameters"), Min(0)]
    [Tooltip("Intervalle de temps entre le spawn de 2 unités")]
    public float cadenceSpawn = 1f;

    [Tooltip("Override des wave auto par des ensembles prédefinis\nDefault Wave si nul")]
    public List<WaveData> overridesWaves;

    [Tooltip("Amrée par défaut de la wave\nMode mirroir si nul")]
    public WaveData defaultWave;

    // Ajout mob progression
    [Header("Additionnals Units")]
    [Min(0)]
    public int unitMinAdded = 0;
    [Min(0)]
    public int unitMaxAdded = 5;
    public bool addOneUnitByWave = false;

    private void Start()
    {
        TimeManager.Instance.tickEvent.AddListener(TickConsumeEnergy);
    }

    public IEnumerator WaveAttack(List<GameObject> army)
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

            yield return new WaitForSeconds(cadenceSpawn);
        }
    }

    public void PrepareWave(bool isSuperNight)
    {


        List<GameObject> AIarmy = new List<GameObject>();

        // Si la liste des override existe à la wave en question
        if (overridesWaves.Count >= numberWave)
        {
            // Si il y a une wave renseigner
            // (le -1 c'est car numberWave commence à 1 et la liste à 0)
            if (overridesWaves[numberWave-1] != null)
            {
                AIarmy.AddRange(overridesWaves[numberWave-1].army);
            }
            // Sinon si il y a une default awave
            else if (defaultWave != null)
            {
                AIarmy.AddRange(defaultWave.army);
            }
            // Si rien n'est renseigner
            else
            {
                AIarmy.AddRange(MirrorArmy());
            }
        }
        // Sinon si on a quand même une wave par défaut
        else if (defaultWave != null)
        {
            AIarmy.AddRange(defaultWave.army);
        }
        // Si rien n'est renseigner
        else
        {
            AIarmy.AddRange(MirrorArmy());
        }

        #region Additionnal Units
        //Ajout d'unités en plus
        int tmp = Random.Range(unitMinAdded, unitMaxAdded + 1);

        // Ajout potentiel d'une unité par wave
        if (addOneUnitByWave) { tmp += numberWave; }

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

        // Appel de la coroutine qui cadence l'instanciation
        StartCoroutine(WaveAttack(AIarmy));

        numberWave++;
    }

    /// <summary>
    /// Retourne une armée constituée du double des unités alliée
    /// </summary>
    public List<GameObject> MirrorArmy()
    {
        PlayerProfile playerProfile = IdentifyPlayerProfile();
        List<GameObject> AIMirrorArmy = new List<GameObject>();

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
                AIMirrorArmy.Add(unitToBuild);
            }
        }
        return AIMirrorArmy;
    }

    public void DestroyEntity(GameObject toDestroy)
    {
        if (toDestroy.TryGetComponent(out EntityController entity))
        {
            RessourcesManager.Instance.scraps += entity.Datas.ScrapsValue;
            if (entity.Faction == Faction.Player)
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
        if (RessourcesManager.Instance.RemoveEnergie(RessourcesManager.Instance._energyConsumed))
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