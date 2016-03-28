namespace CludoEngine
{
    public class OnGameObjectRemovedEventArgs
    {
        public OnGameObjectRemovedEventArgs(GameObject obj)
        {
            GameObject = obj;
        }

        public GameObject GameObject { get; set; }
    }
}