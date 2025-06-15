using System;

namespace Runtime {
    public sealed class HighscoreEntry {
        public int handledConcerns = 0;
        public DateTime startTime = default;
        public DateTime endTime = default;
        public bool died = false;
        public int month => handledConcerns % 12;
        public int year => handledConcerns / 12;
        public TimeSpan playtime => endTime - startTime;
    }
}
