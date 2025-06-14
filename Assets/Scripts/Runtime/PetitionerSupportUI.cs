using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class PetitionerSupportUI : MonoBehaviour {
        [SerializeField]
        Petitioner petitioner = default;

        [SerializeField]
        TMPro.TMP_Text royalFeedbackNumber = default;

        [SerializeField]
        TMPro.TMP_Text colorLerpTarget = default;

        [SerializeField]
        TMPro.TMP_Text speechText = default;

        [SerializeField]
        Canvas rootPanel = default;

        protected void Start()
        {
            rootPanel.enabled = false;
        }

        protected void Update()
        {
            if (petitioner.isAtThrone) {
                rootPanel.enabled = true;
                speechText.text = petitioner.concern.speech;
                royalFeedbackNumber.text = petitioner.GetRoyalSupport().ToString("0");
                colorLerpTarget.color = Color.Lerp(Color.red, Color.green, petitioner.GetRoyalSupport() / 100f);
            }
        }
    }
}
