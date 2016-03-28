namespace CludoEngine
{
    public class OnComponentRemovedEventArgs
    {
        public OnComponentRemovedEventArgs(IComponent component)
        {
            Removed = component;
        }

        public IComponent Removed { get; internal set; }
    }
}