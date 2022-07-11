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
}
