namespace CludoEngine {

    public class OnComponentRemovedEventArgs {

        public OnComponentRemovedEventArgs(IComponent component) {
            Component = component;
        }

        public IComponent Component { get; internal set; }
    }
}