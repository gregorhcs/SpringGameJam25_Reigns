using UnityEngine;

namespace GameJam {
    [RequireComponent(typeof(AudioSource))]
    sealed class AudioSourceRandomPitch : MonoBehaviour {
        [SerializeField]
        float minPitch = 0.9f;
        [SerializeField]
        float maxPitch = 1.1f;

        void Awake() {
            GetComponent<AudioSource>().pitch = Random.Range(minPitch, maxPitch);
        }
    }
}
