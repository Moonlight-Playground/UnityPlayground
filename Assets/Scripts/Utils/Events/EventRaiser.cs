using UnityEngine;

public class EventRaiser : MonoBehaviour
{
    [SerializeField, Tooltip("The ID of this event")]
    private string ID = "";

    public void RaiseEvent()
    {
        EventManager.Raise(new GameEvents.BasicEvent(ID));
    }
}
