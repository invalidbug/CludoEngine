namespace Cludo_Engine
{
    public class OnComponentAddedEventArgs
    {
        public OnComponentAddedEventArgs(IComponent component)
        {
            Added = component;
        }

        public IComponent Added { get; internal set; }
    }
}