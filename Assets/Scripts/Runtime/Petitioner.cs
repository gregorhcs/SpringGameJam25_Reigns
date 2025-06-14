using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class Petitioner : MonoBehaviour {
        public float receivedPositiveFeedback = 0f;
        public float receivedNegativeFeedback = 0f;

        public ConcernAsset concern = default;
    }
}
