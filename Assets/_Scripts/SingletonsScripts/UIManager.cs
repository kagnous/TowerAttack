using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text scrapsCounter;
    public Text waveCounter;
    public Slider energySlider;

    private void Update()
    {
        scrapsCounter.text = RessourcesManager.Instance.scraps.ToString();
        if (waveCounter != null)
            waveCounter.text = $"Wave {EntityManager.Instance.NumberWave-1}";
        energySlider.value = RessourcesManager.Instance._actualEnergy;
        energySlider.maxValue = RessourcesManager.Instance._energyMax;
    }
}
