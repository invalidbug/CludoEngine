namespace Cludo_Engine.Debugging
{
    public class ErrorArgs
    {
        public ErrorArgs(string message, bool fatal)
        {
            Reason = message;
            Fatal = fatal;
        }

        public bool Fatal { get; set; }
        public string Reason { get; set; }
    }
}