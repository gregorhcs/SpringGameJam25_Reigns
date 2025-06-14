using System.Collections;
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

        Coroutine endThrowCoroutine = default;

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
            }
            else {
                UpdateState(JarlState.Idle);
            }
        }

        void InputRejectEnd() {
            UpdateState(JarlState.Idle);
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
