using System.Collections;
using UnityEngine;

namespace GameJam {
    sealed class BackgroundAudioManager : MonoBehaviour {
        [SerializeField]
        AudioSource intro = default;

        [SerializeField]
        float delayafterIntroCue = 0.4f;

        [SerializeField]
        AudioSource bgm = default;

        IEnumerator Start() {

            intro.Play();

            yield return new WaitForSeconds(intro.clip.length + delayafterIntroCue);

            bgm.Play();
        }
    }
}
