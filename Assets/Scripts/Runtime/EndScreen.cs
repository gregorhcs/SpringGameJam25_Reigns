using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public sealed class EndScreen : MonoBehaviour
    {
        [SerializeField]
        HighscoresAsset highscores = default;

        [SerializeField]
        TMPro.TMP_Text age = default;

        [SerializeField]
        TMPro.TMP_Text handledConcerns = default;

        [SerializeField]
        TMPro.TMP_Text timePlayed = default;

        public void Open()
        {
            gameObject.SetActive(true);
            age.text = $"You reigned for {highscores.currentRun.year} year(s) and {highscores.currentRun.month} month(s).";
            handledConcerns.text = $"You handled {highscores.currentRun.handledConcerns} concern(s).";
            timePlayed.text = $"Playtime - {highscores.currentRun.playtime.Hours}h {highscores.currentRun.playtime.Minutes}m {highscores.currentRun.playtime.Seconds}s";
            PlayerAsset.onShortInteract += HandleInteract;
        }

        void HandleInteract() {
            PlayerAsset.onShortInteract -= HandleInteract;
            SceneManager.LoadScene("SampleScene");
        }
    }
}
