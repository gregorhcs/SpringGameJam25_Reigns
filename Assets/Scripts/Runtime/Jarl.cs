using System.Collections;
using System.Collections.Concurrent;
using MyBox;
using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class Jarl : MonoBehaviour {
        public enum JarlState { Idle, Throw, Reject, Leave };

        public JarlState state = JarlState.Idle;

        [SerializeField]
        Animator animator = default;

        [SerializeField]
        PetitionerQueue queue = default;

        [SerializeField]
        float royalSupportPerThrow = 0.1f;

        [SerializeField]
        float royalSupportPerRejectSecond = -0.1f;

        [SerializeField]
        float maxRejectTime = 15f;

        [SerializeField]
        float quitTime = 30f;

        [SerializeField]
        float noInputTimeAtWhichToSendAwayPetitioner = 5f;

        Coroutine endThrowCoroutine = default;
        Coroutine noInputCoroutine = default;

        protected void Start()
        {
            PlayerAsset.onShortInteract += InputThrow;
            PlayerAsset.onLongInteractProgress += InputRejectProgress;
            PlayerAsset.onLongInteract += InputRejectEnd;

            UpdateState(JarlState.Idle);
        }

        void InputThrow() {
            if (queue.TryGetPetitionerInFrontOfThrone(out var petitioner)) {
                UpdateState(JarlState.Throw);

                petitioner.royalSupport += royalSupportPerThrow;

                RestartNoInputCoroutine();

                if (endThrowCoroutine != null) {
                    StopCoroutine(endThrowCoroutine);
                }
                endThrowCoroutine = StartCoroutine(EndThrowAnimation());
            }
        }

        IEnumerator EndThrowAnimation() {
            yield return new WaitForSeconds(0.1f);
            if (state  == JarlState.Throw) {
                UpdateState(JarlState.Idle);
            }
        }

        void InputRejectProgress(float timeSinceStart) {
            if (timeSinceStart > quitTime) {
                QuitGameHelper.Quit();
            }
            else if (timeSinceStart > maxRejectTime) {
                UpdateState(JarlState.Leave);
            }
            else if (queue.TryGetPetitionerInFrontOfThrone(out var petitioner)) {
                UpdateState(JarlState.Reject);
                petitioner.royalSupport += royalSupportPerRejectSecond * timeSinceStart;

                if (noInputCoroutine != null) {
                    StopCoroutine(noInputCoroutine);
                }
            }
            else {
                UpdateState(JarlState.Idle);
            }
        }

        void InputRejectEnd() {
            UpdateState(JarlState.Idle);
            RestartNoInputCoroutine();
        }

        void RestartNoInputCoroutine() {
            if (noInputCoroutine != null) {
                StopCoroutine(noInputCoroutine);
            }
            noInputCoroutine = StartCoroutine(SendAwayPetitionerIfNoInput());
        }

        IEnumerator SendAwayPetitionerIfNoInput() {
            yield return new WaitForSeconds(noInputTimeAtWhichToSendAwayPetitioner);
            if (queue.TryGetPetitionerInFrontOfThrone(out var petitioner)) {
                petitioner.Leave();
            }
        }

        void UpdateState(JarlState newState) {
            if (state == newState) {
                return;
            }
            state = newState;
            animator.Play(state.ToString(), layer: 0, normalizedTime: 0f);
        }
    }
}
