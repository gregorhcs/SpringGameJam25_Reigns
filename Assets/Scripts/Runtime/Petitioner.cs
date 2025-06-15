using System;
using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class Petitioner : MonoBehaviour {

        public static event Action<Petitioner> onPetitionerLeaves;

        float royalSupport = 0f;
        public float GetRoyalSupport() {  return royalSupport; }
        public void AddToRoyalSupport(float amount) { royalSupport = Mathf.Clamp(royalSupport + amount, 0f, 100f); }

        public ConcernAsset concern = default;
        public FactionAsset faction => concern.faction;
        public bool isAtThrone => slot != null && slot.isThroneSlot;

        PetitionerQueue queue = null;
        PetitionerSlot slot = null;
        public bool isPhysicallyAtSlot = true;

        public bool isLeaving = false;
        Transform leavePoint = default;

        bool isSetUp = false;

        Vector3 targetPosition = default;

        [SerializeField]
        Animator animator = default;

        [SerializeField]
        float speed = 0.4f;

        [SerializeField]
        Vector2 leaveDirection = new(-1.7f, -1f);

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
            slot.petitioner = null;
            slot = null;

            if (BalancingLogs.enabled) {
                Debug.Log($"Concern: {concern}");
            }
            concern.ExecuteOnConcern(royalSupport);
            onPetitionerLeaves.Invoke(this);
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
            var delta = speed * Time.deltaTime * Vector3.Normalize(new(leaveDirection.x, leaveDirection.y, 0f));
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
                    targetPosition = slot.transform.position + (Vector3.left * UnityEngine.Random.Range(0f, queue.spaceBetweenSlots));

                    isPhysicallyAtSlot = false;
                    animator.Play("Walk", layer: 0, normalizedTime: 0f);
                }
            } else {
                var delta = speed * Time.deltaTime * Vector3.left;
                float overshoot = targetPosition.x - (transform.position.x + delta.x);

                transform.position += delta;

                if (overshoot > 0) {
                    isPhysicallyAtSlot = true;
                    animator.Play("Idle", layer: 0, normalizedTime: 0f);
                }
            }
        }
    }
}
