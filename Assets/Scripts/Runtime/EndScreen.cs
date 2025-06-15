using UnityEngine;

namespace Runtime
{
    public sealed class EndScreen : MonoBehaviour
    {
        [SerializeField]
        HighscoresAsset asset = default;

        [SerializeField]
        Canvas root = default;

        [SerializeField]
        TMPro.TMP_Text age = default;

        [SerializeField]
        TMPro.TMP_Text handledConcerns = default;

        [SerializeField]
        TMPro.TMP_Text timePlayed = default;

        void OnEnable()
        {
            root.enabled = true;
            age.text = $"You lived {asset.currentRun.year} years and {asset.currentRun.month} months.";
            handledConcerns.text = $"You handled {asset.currentRun.handledConcerns} concerns.";
            timePlayed.text = $"Playtime - {asset.currentRun.playtime}";
        }

        void OnDisable()
        {
            root.enabled = false;
        }
    }
}
