namespace Cludo_Engine
{
    public class OnGameObjectAddedEventArgs
    {
        public OnGameObjectAddedEventArgs(GameObject obj)
        {
            GameObject = obj;
        }

        public GameObject GameObject { get; set; }
    }
}