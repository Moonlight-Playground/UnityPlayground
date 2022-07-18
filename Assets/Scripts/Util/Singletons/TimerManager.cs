using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class TimerManager : PersistentSingleton<TimerManager>
{
    [SerializeField, Required]
    private Timer _timerPrefab;
    private Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();

    protected override void OnDestroy()
    {
        base.OnDestroy();
        foreach (Timer timer in _timers.Values)
        {
            DestroyTimer(timer.ID);
        }
    }

    /// <summary>
    /// Creates a timer based on the various parameters
    /// </summary>
    /// <param name="id">Unique ID of the timer</param>
    /// <param name="duration">Time or value until complete</param>
    /// <param name="onComplete">Callback on timer complete</param>
    /// <param name="tickOnCreate">Auto-start timer?</param>
    /// <param name="loop">Does this timer loop?</param>
    /// <param name="loopFor">How many times to loop before destroying itself, default to loop forever</param>
    /// <param name="destroyOnComplete">Should this timer be destroyed on completion?</param>
    /// <param name="type">When does the timer tick?</param>
    /// <param name="tickValueType">How much does each tick subtract from the time left?</param>
    /// <param name="tickCallback">Delegate to invoke per tick</param>
    /// <param name="tickDelegate">External tick to use instead of Unity's build-in messages, takes precendence when determining timer type if not null</param>
    /// <param name="tickValue">Custom value to subtract from the time left per tick, takes precedence when determining timer tick value if not default</param>
    ///
    public Timer CreateTimer(string ID, float duration, Action onComplete,
                            bool tickOnCreate = true, bool loop = default, int loopFor = default, bool destroyOnComplete = false,
                            Timer.TimerType type = Timer.TimerType.Update, Timer.TimerTickValue tickValueType = Timer.TimerTickValue.DeltaTime,
                            Action tickCallback = null, Action tickDelegate = null, float tickValue = default)
    {
        if (_timers.ContainsKey(ID))
        {
            Debug.LogError($"Timer ID '{ID}' already exists", this);
            return null;
        }

        var timer = Instantiate(_timerPrefab, Vector3.zero, Quaternion.identity, transform) as Timer;
        timer.Initialize(ID, duration, onComplete, tickOnCreate, loop, loopFor, destroyOnComplete, type, tickValueType, tickCallback, tickDelegate, tickValue);
        timer.name = "Timer: " + ID;
        _timers.Add(ID, timer);
        return timer;
    }

    public bool DestroyTimer(string ID)
    {
        if (!_timers.Remove(ID, out Timer timer))
        {
            Debug.LogWarning($"Attempted to destroy timer '{ID}' when it doesn't exist", this);
            return false;
        }

        Destroy(timer.gameObject);
        return true;
    }

    public Timer GetTimer(string ID)
    {
        if (!_timers.TryGetValue(ID, out Timer timer))
        {
            Debug.LogError($"Attempted to get timer '{ID}' when it doesn't exist", this);
            return null;
        }

        return timer;
    }
}
