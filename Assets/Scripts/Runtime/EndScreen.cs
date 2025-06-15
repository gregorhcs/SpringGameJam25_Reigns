using UnityEngine;

namespace Runtime
{
    public sealed class EndScreen : MonoBehaviour
    {
        [SerializeField]
        HighscoresAsset asset = default;

        [SerializeField]
        TMPro.TMP_Text age = default;

        [SerializeField]
        TMPro.TMP_Text handledConcerns = default;

        [SerializeField]
        TMPro.TMP_Text timePlayed = default;

        public void Open()
        {
            gameObject.SetActive(true);
            age.text = $"You lived {asset.currentRun.year} years and {asset.currentRun.month} months.";
            handledConcerns.text = $"You handled {asset.currentRun.handledConcerns} concerns.";
            timePlayed.text = $"Playtime - {asset.currentRun.playtime.Hours}h {asset.currentRun.playtime.Minutes}m {asset.currentRun.playtime.Seconds}s";
        }

        public void Close() {
            gameObject.SetActive(false);
        }
    }
}
