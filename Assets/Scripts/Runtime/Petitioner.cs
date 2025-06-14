using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class Petitioner : MonoBehaviour {
        public PetitionerSlot slot = null;
        public bool isPhysicallyAtSlot = true;

        public PetitionerQueue queue = null;
        public float royalSupport = 0f;
        public ConcernAsset concern = default;

        bool isSetUp = false;

        public FactionAsset faction => concern.faction;

        [SerializeField]
        Animator animator = default;

        [SerializeField]
        float speed = 0.4f;

        public void SetUp(Transform spawnPoint, ConcernAsset inConcern, PetitionerQueue inQueue) {
            transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            animator.runtimeAnimatorController = inConcern.faction.animatorController;
            concern = inConcern;
            queue = inQueue;

            animator.Play("Idle", layer: 0, normalizedTime: 0f);

            isSetUp = true;
        }

        protected void Update() {
            if (!isSetUp) {
                return;
            }

            if (isPhysicallyAtSlot) {
                if (queue.FindFreeSlotInFrontOf(gameObject, out var newSlot)) {
                    if (slot) {
                        slot.petitioner = null;
                    }

                    slot = newSlot;
                    slot.petitioner = this;

                    isPhysicallyAtSlot = false;
                    animator.Play("Walk", layer: 0, normalizedTime: 0f);
                }
            }
            else {
                var delta = speed * Time.deltaTime * Vector3.left;
                float overshoot = slot.transform.position.x - (transform.position.x + delta.x);

                transform.position += delta;

                if (overshoot > 0) {
                    isPhysicallyAtSlot = true;
                    animator.Play("Idle", layer: 0, normalizedTime: 0f);
                }
            }
        }
    }
}
