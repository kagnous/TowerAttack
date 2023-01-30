using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// Instance du MB
    /// </summary>
    private static T _instance = null;

    /// <summary>
    /// Acesseur du MB
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject instanceGo = new GameObject(nameof(T));
                    _instance = instanceGo.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}
