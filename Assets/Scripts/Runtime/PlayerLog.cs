using UnityEngine;

namespace Runtime {
    sealed class PlayerLog : MonoBehaviour {
        void OnEnable() {
            PlayerAsset.onShortInteract += OnShortInteract;
            PlayerAsset.onLongInteract += OnLongInteract;
        }

        void OnDisable() {
            PlayerAsset.onShortInteract -= OnShortInteract;
            PlayerAsset.onLongInteract -= OnLongInteract;
        }

        void OnShortInteract() => Debug.Log(nameof(OnShortInteract));
        void OnLongInteract() => Debug.Log(nameof(OnLongInteract));
    }
}
