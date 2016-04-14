namespace CludoEngine.Components {
    public class OnCollisionEventArgs {
        public OnCollisionEventArgs(GameObject obj1, GameObject obj2) {
            Object1 = obj1;
            Object2 = obj2;
            Canceled = false;
        }

        public bool Canceled { get; set; }
        public GameObject Object1 { get; set; }
        public GameObject Object2 { get; set; }
    }
}