
using System;

namespace Assets.Scripts.Runtime {
    public class GameState {
        public static bool hasEnded = false;
        public static event Action<bool> onHasEndedChanged = default;
        public static void ChangeHasEnded(bool newHasEnded) {
            hasEnded = newHasEnded;
            onHasEndedChanged?.Invoke(newHasEnded);
        }
    }
}
