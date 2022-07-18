using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField, Tooltip("The ID of the event you want to listen to")]
    private string _eventID = "";

    public UnityEvent OnInvoke;

    private void OnEnable()
    {
        EventManager.AddListener<GameEvents.BasicEvent>(OnEventRaised);
    }

    private void OnEventRaised(GameEvents.BasicEvent e, object t)
    {
        if (e.ID == _eventID)
        {
            OnInvoke?.Invoke();
        }
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<GameEvents.BasicEvent>(OnEventRaised);
    }
}
