using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text scrapsCounter;

    public Slider energySlider;

    private void Update()
    {
        scrapsCounter.text = RessourcesManager.Instance.scraps.ToString();

        energySlider.value = RessourcesManager.Instance._actualEnergy;
        energySlider.maxValue = RessourcesManager.Instance._energyMax;
    }
}
