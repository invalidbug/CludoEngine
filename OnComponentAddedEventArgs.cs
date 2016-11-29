namespace CludoEngine {

    public class OnComponentAddedEventArgs {

        public OnComponentAddedEventArgs(IComponent component) {
            Component = component;
        }

        public IComponent Component { get; internal set; }
    }
}