using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text scrapsCounter;

    private void Update()
    {
        scrapsCounter.text = RessourcesManager.Instance.scraps.ToString();
    }
}
