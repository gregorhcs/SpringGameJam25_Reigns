using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class Petitioner : MonoBehaviour {
        public float royalSupport = 0f;
        public ConcernAsset concern = default;
        public FactionAsset faction => concern.faction;

        PetitionerQueue queue = null;
        PetitionerSlot slot = null;
        public bool isPhysicallyAtSlot = true;

        bool isLeaving = false;
        Transform leavePoint = default;

        bool isSetUp = false;


        [SerializeField]
        Animator animator = default;

        [SerializeField]
        float speed = 0.4f;

        public void SetUp(Transform spawnPoint, Transform inLeavePoint, ConcernAsset inConcern, PetitionerQueue inQueue) {
            transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            animator.runtimeAnimatorController = inConcern.faction.animatorController;

            leavePoint = inLeavePoint;
            concern = inConcern;
            queue = inQueue;

            animator.Play("Idle", layer: 0, normalizedTime: 0f);

            isSetUp = true;
        }

        public void Leave() {
            isLeaving = true;
            animator.Play("Walk", layer: 0, normalizedTime: 0f);
        }

        protected void Update() {
            if (!isSetUp) {
                return;
            }

            if (isLeaving) {
                UpdateLeaving();
            } else {
                UpdatePositionInQueue();
            }
        }

        void UpdateLeaving() {
            var delta = speed * Time.deltaTime * Vector3.down;
            float overshoot = leavePoint.position.y - (transform.position.y + delta.y);

            transform.position += delta;

            if (overshoot > 0) {
                Destroy(gameObject);
            }
        }

        void UpdatePositionInQueue() {
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
            } else {
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
