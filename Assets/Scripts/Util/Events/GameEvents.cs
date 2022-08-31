namespace GameEvents
{
    public class BasicEvent : EventManager.BaseEvent
    {
        public string ID;

        public BasicEvent(string id)
        {
            ID = id;
        }
    }

    public class PlayerSpawnEvent : EventManager.BaseEvent
    {
        public UnityEngine.InputSystem.PlayerInput PlayerInput;

        public PlayerSpawnEvent(UnityEngine.InputSystem.PlayerInput playerInput)
        {
            PlayerInput = playerInput;
        }
    }

    public class PlayerDespawnEvent : EventManager.BaseEvent { }
}
