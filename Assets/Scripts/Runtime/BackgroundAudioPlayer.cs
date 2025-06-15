using System.Collections;
using UnityEngine;

namespace GameJam {
    sealed class BackgroundAudioManager : MonoBehaviour {
        [SerializeField]
        AudioClip backgroundMusic = default;

        [SerializeField]
        AudioClip introCue = default;

        [SerializeField]
        AudioSource audioSource = default;

        [SerializeField]
        float delayafterIntroCue = 0.4f;

        IEnumerator Start()
        {
            audioSource.PlayOneShot(introCue);
            yield return new WaitForSeconds(introCue.length);
            yield return new WaitForSeconds(delayafterIntroCue);

            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();

            yield return null;
        }
    }
}
