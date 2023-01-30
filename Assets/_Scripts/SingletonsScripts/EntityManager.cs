using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    public Transform unitsSpawn;
    public GameObject unitsCac;
    public GameObject unitsRange;
    public GameObject destroyEffect;
    public GameObject globalAItarget;

    public IEnumerator WaveAttack(List<GameObject> army, float delay)
    {
        for (int i = 0; i < army.Count; i++)
        {
            GameObject unit = Instantiate(army[i], unitsSpawn.position, unitsSpawn.rotation);
            EntityMovableController controller = unit.GetComponent<EntityMovableController>();
            controller.Faction = Faction.IA;
            controller.globalTarget = globalAItarget;

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

    public void PrepareWave()
    {
        PlayerProfile playerProfile = IdentifyPlayerProfile();

        List<GameObject> AIarmy = new List<GameObject>();

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
                case EntityType.Tower:
                    continue;
                default:
                    Debug.Log("Une entité du player n'a pas de type assigné");
                    continue;
            }

            // On répète 3 fois car profil de base = 3x plus
            for (int j = 0; j < 3; j++)
            {
                AIarmy.Add(unitToBuild);
            }
        }

        if(AIarmy.Count <= 0) 
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
        }

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