using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class Timer : MonoBehaviour
{
    // When/where to tick
    public enum TimerType
    {
        Update,
        FixedUpdate,
        LateUpdate,
        Manual
    }

    // For each tick, what is the value?
    public enum TimerTickValue
    {
        DeltaTime,
        FixedDeltaTime,
        UnscaledDeltaTime,
        FixedUnscaledDeltaTime,
        CustomValue
    }

    [ShowInInspector, ReadOnly] public bool IsTicking { get; set; }
    [ShowInInspector, ReadOnly] public string ID { get; private set; }
    [ShowInInspector, ReadOnly] public float Duration { get; private set; }
    [ShowInInspector, ReadOnly] public bool Loop { get; set; }
    [ShowIf("Loop"), ShowInInspector, ReadOnly] public int LoopFor { get; private set; }
    [ShowIf("Loop"), ShowInInspector, ReadOnly] public int LoopCount { get; private set; }
    [ShowInInspector, ReadOnly] public float TimeLeft { get; private set; }
    [ShowInInspector, ReadOnly] public float TickValue { get; private set; }
    [ShowInInspector, ReadOnly] public bool DestroyOnComplete { get; private set; }
    [ShowInInspector, ReadOnly] public TimerType Type { get; private set; }
    [ShowInInspector, ReadOnly] public TimerTickValue TickType { get; private set; }

    private Action _tickCallback;   // Callback to external for per-tick execution
    private Action _tickDelegate;   // Custom tick interval from external invoke
    private Action _onComplete;     // On timer complete callback

    private bool IsTimerComplete => TimeLeft <= 0;
    private bool _sameFrameRestart;

    public void Initialize(string id, float duration, Action onComplete, bool tickOnCreate, bool loop, int loopFor, bool destroyOnComplete, TimerType type, TimerTickValue tickValueType, Action tickCallback, Action tickDelegate, float tickValue)
    {
        // Required
        ID = id;
        Duration = duration;
        _onComplete = onComplete;

        // Optional
        Loop = loop;
        DestroyOnComplete = destroyOnComplete;
        _tickCallback = tickCallback;
        _tickDelegate = tickDelegate;
        TickValue = tickValue;

        if (_tickDelegate != null)
        {
            Type = TimerType.Manual;
            tickDelegate += OnTickDelegate;
        }
        else
        {
            Type = type;
        }

        if (tickValue != default)
        {
            TickType = TimerTickValue.CustomValue;
        }
        else
        {
            TickType = tickValueType;
        }

        TimeLeft = Duration;
        IsTicking = tickOnCreate;
    }

    protected void FixedUpdate()
    {
        if (Type == TimerType.FixedUpdate && IsTicking)
        {
            Tick();
        }
    }

    protected void Update()
    {
        if (Type == TimerType.Update && IsTicking)
        {
            Tick();
        }
    }

    protected void LateUpdate()
    {
        if (Type == TimerType.LateUpdate && IsTicking)
        {
            Tick();
        }
    }

    protected void OnEnable()
    {
        if (_tickDelegate != null)
        {
            _tickDelegate += OnTickDelegate;
        }
    }

    protected void OnDisable()
    {
        if (_tickDelegate != null)
        {
            _tickDelegate -= OnTickDelegate;
        }
    }

    protected void OnDestroy()
    {
        TimerManager.Instance?.DestroyTimer(ID);
    }

    public void UpdateDuration(float newDuration) => Duration = newDuration;
    public void ResetTimer() => TimeLeft = Duration;
    public void SetLoopFor(int newLoopFor) => LoopFor = newLoopFor;

    private void OnTickDelegate() => Tick();

    /// <param name="sameFrameRestart">Should timer restart if it was completed on the same frame?</param>
    public void RestartTimer(bool sameFrameRestart = true)
    {
        _sameFrameRestart = sameFrameRestart;

        ResetTimer();
        IsTicking = true;
    }

    private void Tick()
    {
        switch (TickType)
        {
            case TimerTickValue.DeltaTime:
                TimeLeft -= Time.deltaTime;
                break;
            case TimerTickValue.UnscaledDeltaTime:
                TimeLeft -= Time.unscaledDeltaTime;
                break;
            case TimerTickValue.FixedDeltaTime:
                TimeLeft -= Time.fixedDeltaTime;
                break;
            case TimerTickValue.FixedUnscaledDeltaTime:
                TimeLeft -= Time.fixedUnscaledDeltaTime;
                break;
            case TimerTickValue.CustomValue:
                TimeLeft -= TickValue;
                break;
        }

        _tickCallback?.Invoke();
        TryTimerComplete();
    }

    private void TryTimerComplete()
    {
        if (!IsTimerComplete)
        {
            return;
        }

        _onComplete?.Invoke();

        if (Loop)
        {
            LoopCount++;
            if (LoopFor != default && LoopCount >= LoopFor)
            {
                TimerComplete();
            }

            ResetTimer();
        }
        else
        {
            TimerComplete();
        }
    }

    private void TimerComplete()
    {
        if (DestroyOnComplete)
        {
            TimerManager.Instance.DestroyTimer(ID);
        }
        else
        {
            if (_sameFrameRestart)
            {
                IsTicking = true;
                _sameFrameRestart = false;
            }
            else
            {
                IsTicking = false;
            }
        }
    }
}
