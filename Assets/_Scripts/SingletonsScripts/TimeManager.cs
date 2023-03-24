using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public enum TimeOfDay
{
    Pause,
    Day,
    Night
}

public class TimeManager : Singleton<TimeManager>
{
    private Light sun;
    public GameObject sunlight;

    private const float _tick = 1f;
    private const float _hackTick = 5f;

    // CYCLE JOUR / NUIT AUTO
    /*[Header("Cycle jour/nuit")]
    [SerializeField, Tooltip("Durée du jour en s")]
    private float _dayDuration = 10;

    [SerializeField, Tooltip("Durée de la nuit en s")]
    private float _nightDuration = 10;*/

    //private float _daysSurvived = 0;

    //private float _timerDaytime = 0;

    private float _timerTick = 0;
    private float _timerHack = 0;

    [Header("Events")]
    public UnityEvent tickEvent;
    public UnityEvent hackEvent;

    public TimeOfDay _actualTimeOfDay;

    private void Start()
    {
        sun = sunlight.GetComponent<Light>();

        // Setup du cycle jour/nuit
        //_actualTimeOfDay = TimeOfDay.Night;
        //sun.color = Color.grey;
    }

    private void Update()
    {
        /*
        #region Journée
        _timerDaytime += Time.deltaTime;
        if (_actualTimeOfDay == TimeOfDay.Day && _timerDaytime >= _dayDuration)
        {
            sun.color = Color.grey;

            _timerDaytime = 0;
            _actualTimeOfDay = TimeOfDay.Night;
            _daysSurvived++;
        }
        else if (_actualTimeOfDay == TimeOfDay.Night && _timerDaytime >= _nightDuration)
        {
            sun.color = Color.white;

            _timerDaytime = 0;
            if (_daysSurvived > 3)
            {
                EntityManager.Instance.PrepareWave(true);
            }
            else
            {
                EntityManager.Instance.PrepareWave(false);
            }

            _actualTimeOfDay = TimeOfDay.Day;
        }
        //Debug.Log(_actualTimeOfDay);

        #endregion
        */


        // TIMERS
        _timerTick += Time.deltaTime;
        if (_timerTick > _tick)
        {
            tickEvent?.Invoke();
            _timerTick = 0;
        }

        _timerHack+= Time.deltaTime;
        if(_timerHack > _hackTick)
        {
            hackEvent?.Invoke();
            _timerHack = 0;
        }
    }

    /// <summary>
    /// Alternative au cycle jour/nuit
    /// </summary>
    public void NewDay()
    {
        EntityManager.Instance.PrepareWave(false);
    }
}
