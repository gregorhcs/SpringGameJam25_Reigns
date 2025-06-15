using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runtime {
    public sealed class EndScreen : MonoBehaviour {
        [SerializeField]
        HighscoresAsset highscores = default;

        [SerializeField]
        TMPro.TMP_Text age = default;

        [SerializeField]
        TMPro.TMP_Text handledConcerns = default;

        [SerializeField]
        TMPro.TMP_Text timePlayed = default;

        [SerializeField]
        Image factionImage = default;

        [SerializeField]
        GameObject winnerFactionObject = default;

        [SerializeField]
        GameObject winnerJarlObject = default;

        [SerializeField]
        FactionAsset[] factions = default;

        public void Open() {
            gameObject.SetActive(true);
            age.text = $"You reigned for {highscores.currentRun.year} year(s) and {highscores.currentRun.month} month(s).";
            handledConcerns.text = $"You handled {highscores.currentRun.handledConcerns} concern(s).";
            timePlayed.text = $"Playtime - {highscores.currentRun.playtime.Hours}h {highscores.currentRun.playtime.Minutes}m {highscores.currentRun.playtime.Seconds}s";

            if (highscores.currentRun.died) {
                winnerJarlObject.SetActive(true);
            } else {
                FactionAsset winnerFaction = null;
                foreach (var faction in factions) {
                    if (winnerFaction == null || winnerFaction.GetLoyalty() > faction.GetLoyalty()) {
                        winnerFaction = faction;
                    }
                }
                factionImage.sprite = winnerFaction.sprite;
                winnerFactionObject.SetActive(true);
            }

            PlayerAsset.onShortInteract += HandleInteract;
        }

        void HandleInteract() {
            PlayerAsset.onShortInteract -= HandleInteract;
            SceneManager.LoadScene("SampleScene");
        }
    }
}
