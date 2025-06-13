using System;
using UnityEngine;

namespace Runtime {
    [CreateAssetMenu]
    sealed class PlayerAsset : ScriptableObject {

        internal static event Action onShortInteract;
        internal static event Action<float> onLongInteractProgress;
        internal static event Action onLongInteract;

        internal void SetUp() {
            isInteracting = false;
            longInteractionDone = false;
        }

        [SerializeField]
        float shortInteractionDuration = 0.2f;
        [SerializeField]
        float longInteractionDuration = 1f;

        [Header("Runtime")]
        [SerializeField]
        internal bool isInteracting;
        [SerializeField]
        internal float interactionTime;
        [SerializeField]
        bool longInteractionDone;

        internal float interactionDuration => Time.time - interactionTime;

        internal void OnInteract(bool willBeInteracting) {
            bool wasInteracting = isInteracting;
            isInteracting = willBeInteracting;

            if (isInteracting) {
                if (!wasInteracting) {
                    interactionTime = Time.time;
                }

                if (!longInteractionDone) {
                    if (interactionDuration >= shortInteractionDuration) {
                        float duration = (interactionDuration - shortInteractionDuration) / longInteractionDuration;
                        if (duration > 1) {
                            longInteractionDone = true;
                            onLongInteract?.Invoke();
                        } else {
                            onLongInteractProgress?.Invoke(duration);
                        }
                    }
                }
            } else {
                longInteractionDone = false;

                if (wasInteracting) {
                    if (interactionDuration < shortInteractionDuration) {
                        onShortInteract?.Invoke();
                    }
                }
            }
        }
    }
}