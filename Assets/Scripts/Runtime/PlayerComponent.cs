using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime {
    sealed class PlayerComponent : MonoBehaviour {
        [SerializeField]
        InputActionReference interactAction;
        [SerializeField]
        PlayerAsset player;

        InputAction interact;

        void OnEnable() {
            interact = interactAction.ToInputAction();
            interact.Enable();
        }

        void Update() {
            player.OnInteract(interact.IsPressed());
        }

        void OnDisable() {
            interact.Disable();
            interact.Dispose();
        }
    }
}
