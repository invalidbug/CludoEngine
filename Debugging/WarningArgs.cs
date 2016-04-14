namespace CludoEngine.Debugging {
    public class WarningArgs {
        public WarningArgs(string message, int level) {
            Message = message;
            WarningLevel = level;
        }

        public string Message { get; set; }

        public int WarningLevel { get; set; }
    }
}