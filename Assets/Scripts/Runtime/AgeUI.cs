using Assets.Scripts.Runtime;
using UnityEngine;

namespace Runtime {
    public sealed class AgeUI : MonoBehaviour {
        [SerializeField]
        HighscoresAsset asset = default;

        [SerializeField]
        TMPro.TMP_Text age = default;

        void Update() {
            age.text = GameState.hasEnded ? "" : $"{asset.currentRun.year}y {asset.currentRun.month}m.";
        }
    }
}
