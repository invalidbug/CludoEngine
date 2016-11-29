using System.Data.Odbc;
using System.Security.AccessControl;

namespace CludoEngine.Debugging {

    public static class Debug {

        public delegate void OnDebug(object sender, DebugArgs args);

        public delegate void OnError(object sender, ErrorArgs args);

        public delegate void OnWarning(object sender, WarningArgs args);

        public delegate void OnMessage(object sender, MessageArgs args);

        public static event OnDebug OnDebugEvent;

        public static event OnError OnErrorEvent;

        public static event OnWarning OnWarningEvent;

        public static event OnMessage OnMessageEvent;

        public static void DoDebug(string message) {
            if (OnDebugEvent != null) {
                OnDebugEvent(null, new DebugArgs(message));
            }
        }

        public static void BroadcastMessage(string message) {
            BroadcastMessage("New Message", message);
        }

        public static void BroadcastMessage(string title, object message) {
            if (OnMessageEvent != null) {
                OnMessageEvent(null, new MessageArgs() {Title=title, Message = message });
            }
        }

        public static void DoError(string reason, bool fatal) {
            if (OnErrorEvent != null) {
                OnErrorEvent(null, new ErrorArgs(reason, fatal));
            }
        }

        public static void DoWarning(string message, int level) {
            if (OnWarningEvent != null) {
                string levelstring;
                switch (level) {
                    case 0:
                    levelstring = "Safe To ignore";
                    break;

                    case 1:
                    levelstring = "Should be fixed.";
                    break;

                    case 2:
                    levelstring = "Should be fixed, could cause fatal error";
                    break;

                    case 3:
                    levelstring = "Must be fixed, will cause fatal error";
                    break;

                    default:
                    levelstring = "Needs attention now!";
                    break;
                }
                OnWarningEvent(null, new WarningArgs(message + "\n Level:" + levelstring, level));
            }
        }

        public static void Write(string text) {
            System.Diagnostics.Debug.Write(text);
        }

        public static void Write(object text) {
            System.Diagnostics.Debug.Write(text.ToString());
        }

        public static void WriteLine(string text) {
            System.Diagnostics.Debug.Write("\n" + text);
        }

        public static void WriteLine(object text) {
            System.Diagnostics.Debug.Write("\n" + text);
        }
    }

}