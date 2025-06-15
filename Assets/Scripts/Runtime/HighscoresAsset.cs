using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    public sealed class HighscoresAsset : ScriptableObject {
        public List<HighscoreEntry> highscores = new();
        public HighscoreEntry currentRun = default;

        public static event Action onConcernHandled;

        public void ModifyHandledConcerns(int amount) {
            currentRun.handledConcerns += amount * 12;
            onConcernHandled?.Invoke();
        }
    }
}
