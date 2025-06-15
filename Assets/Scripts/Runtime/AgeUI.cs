using UnityEngine;

namespace Runtime {
    public sealed class AgeUI : MonoBehaviour {
        [SerializeField]
        HighscoresAsset asset = default;

        [SerializeField]
        TMPro.TMP_Text age = default;

        void Update() {
            age.text = EndingConditionsObserver.isEnded ? "" : $"{asset.currentRun.year}y {asset.currentRun.month}m.";
        }
    }
}
