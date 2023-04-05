using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    public Camera currentCamera;

    public GameObject _entitieSelected;

    private Inputs _inputsInstance = null;

    private void Awake()
    {
        _inputsInstance = new Inputs();
    }
    private void OnEnable()
    {
        // Assignation des fonctions aux Inputs
        _inputsInstance.Player.Enable();
        _inputsInstance.Player.Activate.performed += Activate;
    }

    private void Update()
    {
        UpdateInputPlayer();
    }

    /// <summary>
    /// Quand on clique gauche
    /// </summary>
    public void Activate(InputAction.CallbackContext context)
    {
        if(_entitieSelected)
        {
            if(_entitieSelected.TryGetComponent(out Entity entity))
            {
                if(entity.Interractable)
                    entity.OnClick();
            }
        }

        /*
        if (_entitieSelected)
        {
            if (_entitieSelected.TryGetComponent(out TowerRuins ruins))
            {
                if (ruins.Tower == null)
                {
                    if (RessourcesManager.Instance.TryBuy(100))
                    {
                        ruins.BuildTower();
                    }
                }
                return;
            }
            else if (_entitieSelected.TryGetComponent(out Casern casern))
            {
                if (RessourcesManager.Instance.TryBuy(casern.cost))
                {
                    casern.BuyUnit();
                }
                return;
            }
            else if (_entitieSelected.TryGetComponent(out Barricade barricade))
            {
                if (barricade._canBuild)
                {
                    if (RessourcesManager.Instance.TryBuy(100))
                    {
                        barricade.BuildBarricade();
                    }
                }
                return;
            }
            else if(_entitieSelected.TryGetComponent(out SolarPanel solarPanel))
            {
                if(!solarPanel._isActivated)
                {
                    if(RessourcesManager.Instance.TryBuy(50))
                    {
                        solarPanel.BuildSolarPanel();
                    }   
                }
                return;
            }
            else if (_entitieSelected.TryGetComponent(out EntityController entity))
            {
                if (entity.Datas.Type == EntityType.Tower)
                {
                    if (RessourcesManager.Instance.TryBuy(40))
                    {
                        entity.Heal(entity.Datas.Life);
                    }
                }
                return;
            }
            Debug.Log("Objet sélectionné non reconnu");
        }
        */
    }

    /// <summary>
    /// Quand on clique droit
    /// </summary>
    public void AltClick(InputAction.CallbackContext context)
    {
        if (_entitieSelected)
        {
            if (_entitieSelected.TryGetComponent(out Entity entity))
            {
                if (entity.Interractable)
                    entity.OnAltClick();
            }
        }
    }

    private void UpdateInputPlayer()
    {
        //Capte la position de la souris
        Vector3 mousePosition = Input.mousePosition;

        // Rayon de la souris par rapport à la caméra
        Ray ray = currentCamera.ScreenPointToRay(mousePosition);
        // Création d'une variable Raycast nécessaire pour le out de la fonction Raycast
        RaycastHit hit;

        //Si le raycast de la souris sur l'écran est en contact avec un objet de layer "Structure"
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Structure")))
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
            if (hit.collider.gameObject!=_entitieSelected)
            {
                if (_entitieSelected!=null) UnselectObject();
                SelectObject(hit.collider.gameObject);
            }
            
        }
        else
        {
            if (_entitieSelected!=null) UnselectObject();  
        }
    }

    void SelectObject(GameObject newlySelected)
    {
        _entitieSelected = newlySelected;
        _originalScale = _entitieSelected.transform.localScale;
        _originalMaterial = _entitieSelected.GetComponentInChildren<MeshRenderer>().material;
        _entitieSelected.GetComponentInChildren<MeshRenderer>().material = _selectedMaterial;
        _entitieSelected.transform.localScale = _entitieSelected.transform.localScale * 1.2f;
    
    }

    void UnselectObject()
    {
        _entitieSelected.transform.localScale = _originalScale;
        _entitieSelected.GetComponentInChildren<MeshRenderer>().material = _originalMaterial;
        _entitieSelected = null;
    }

    Vector3 _originalScale;
    Material _originalMaterial;
    [SerializeField]Material _selectedMaterial;

}
