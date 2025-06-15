using System.Collections;
using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    sealed class Jarl : MonoBehaviour {
        public enum JarlState { Idle, Throw, Reject, Leave };

        public JarlState state = JarlState.Idle;

        [SerializeField]
        Animator animator = default;

        [SerializeField]
        PetitionerQueue queue = default;

        [SerializeField]
        float royalSupportPerThrow = 0.1f;

        //[SerializeField]
        //float maxSendAwayTime = 15f;

        //[SerializeField]
        //float quitTime = 30f;

        [SerializeField]
        float throwAnimActiveTime = 0.3f;

        Coroutine endThrowCoroutine = default;

        void Start() {
            PlayerAsset.onShortInteract += InputThrow;
            PlayerAsset.onLongInteractProgress += InputSendAwayProgress;
            PlayerAsset.onLongInteract += InputSendAway;
            PlayerAsset.onLongInteractCanceled += InputSendAwayCanceled;

            UpdateState(JarlState.Idle);
        }

        [SerializeField]
        CoinCannon coinCannon;

        void InputThrow() {
            UpdateState(JarlState.Throw);

            if (coinCannon) {
                coinCannon.Shoot(() => {
                    if (queue.TryGetPetitionerInFrontOfThrone(out var petitioner)) {
                        petitioner.AddToRoyalSupport(royalSupportPerThrow);
                    }
                });
            }

            if (endThrowCoroutine != null) {
                StopCoroutine(endThrowCoroutine);
            }

            endThrowCoroutine = StartCoroutine(EndThrowAnimation());
        }

        IEnumerator EndThrowAnimation() {
            yield return new WaitForSeconds(throwAnimActiveTime);
            if (state == JarlState.Throw) {
                UpdateState(JarlState.Idle);
            }
        }

        void InputSendAwayProgress(float timeSinceStart) {
            //if (timeSinceStart > quitTime) {
            //    QuitGameHelper.Quit();
            //}
            //else if (timeSinceStart > maxSendAwayTime) {
            //    UpdateState(JarlState.Leave);
            //}
            //else {
            //    UpdateState(JarlState.Reject);
            //}
            UpdateState(JarlState.Reject);
        }

        void InputSendAway() {
            if (state == JarlState.Reject) {
                UpdateState(JarlState.Idle);
                if (queue.TryGetPetitionerInFrontOfThrone(out var petitioner)) {
                    petitioner.Leave();
                }
            }
        }

        void InputSendAwayCanceled() {
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
