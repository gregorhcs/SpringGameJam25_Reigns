using System.Collections.Generic;
using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    public sealed class HighscoresAsset : ScriptableObject {
        public List<HighscoreEntry> highscores = new();
        public HighscoreEntry currentRun = default;
    }
}
