using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TimeOfDay
{
    Pause,
    Day,
    Night
}

public class TimeManager : Singleton<TimeManager>
{
    private const float _tick = 1f;

    [SerializeField, Tooltip("Durée du jour en s")]
    private float _dayDuration = 10;

    [SerializeField, Tooltip("Durée de la nuit en s")]
    private float _nightDuration = 10;

    private float _daysSurvived = 0;

    public float _timerDaytime = 0;
    public float _timerTick = 0;

    public UnityEvent tickEvent;

    public TimeOfDay _actualTimeOfDay;

    private void Start()
    {
        _actualTimeOfDay = TimeOfDay.Night;
    }

    private void Update()
    {
        _timerDaytime += Time.deltaTime;

        if (_actualTimeOfDay == TimeOfDay.Day)
        {
            if (_timerDaytime >= _dayDuration)
            {
                _timerDaytime = 0;
                _actualTimeOfDay = TimeOfDay.Night;
                _daysSurvived++;
            }
        }
        else if (_actualTimeOfDay == TimeOfDay.Night && _timerDaytime >= _nightDuration)
        {
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

        _timerTick += Time.deltaTime;
        if (_timerTick > _tick)
        {
            tickEvent?.Invoke();
            _timerTick = 0;
        }
    }
}
