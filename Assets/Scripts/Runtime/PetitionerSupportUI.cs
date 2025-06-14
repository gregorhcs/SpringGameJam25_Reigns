using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class PetitionerSupportUI : MonoBehaviour {
        [SerializeField]
        Petitioner petitioner = default;

        [SerializeField]
        TMPro.TMP_Text royalFeedbackNumber = default;

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
                royalFeedbackNumber.text = petitioner.royalSupport.ToString("0");
                royalFeedbackNumber.color = Color.Lerp(Color.red, Color.green, petitioner.royalSupport / 100f);
            }
        }
    }
}
