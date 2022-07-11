namespace GameEvents
{
    public class ExampleEvent : EventManager.BaseEvent
    {
        public string ID;

        public ExampleEvent(string id)
        {
            ID = id;
        }
    }
}
