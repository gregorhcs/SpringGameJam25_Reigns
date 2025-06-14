using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime;
using UnityEngine;

namespace Assets.Scripts.Runtime {
    public class PetitionerQueue : MonoBehaviour {
        [SerializeField]
        PetitionerSlot slotPrefab = default;

        [SerializeField]
        int numberOfSlotsToGenerate = 10;

        [SerializeField]
        int numberOfPetitionersToSpawn = 10;

        [SerializeField]
        float minSecToWaitBetweenSpawns = 0.1f;

        [SerializeField]
        float maxSecToWaitBetweenSpawns = 0.3f;

        [SerializeField]
        GameObject petitionerSpawnPoint = default;

        [SerializeField]
        GameObject petitionerLeavePoint = default;

        [SerializeField]
        GameObject queueStart = default;

        [SerializeField]
        GameObject queueEnd = default;

        [SerializeField]
        Petitioner petitionerPrefab = default;

        [SerializeField]
        ConcernsLibraryAsset concernsLibrary = default;

        public float spaceBetweenSlots => (queueStart.transform.position.x - queueEnd.transform.position.x) / (numberOfSlotsToGenerate - 1);

        List<PetitionerSlot> slots = new();

        protected void Start() {
            for (int slotIndex = 0; slotIndex < numberOfSlotsToGenerate; slotIndex++) {
                var slot = Instantiate(slotPrefab);
                slot.transform.position = Vector3.Lerp(queueStart.transform.position, queueEnd.transform.position, slotIndex / (numberOfSlotsToGenerate-1f));
                slots.Add(slot);
            }

            slots.Last().isThroneSlot = true;

            StartCoroutine(SpawnInitialPetitioners());

            Petitioner.onPetitionerLeaves += HandlePetitionerLeaves;
        }

        public bool TryGetPetitionerInFrontOfThrone(out Petitioner outPetitioner) {
            outPetitioner = null;
            foreach (var slot in slots) {
                if (slot.isThroneSlot && slot.petitioner != null && slot.petitioner.isPhysicallyAtSlot) {
                    outPetitioner = slot.petitioner;
                    return slot.petitioner;
                }
            }
            return false;
        }

        IEnumerator SpawnInitialPetitioners() {
            SpawnPetitioner(slots.SkipLast(2).Last().transform);
            for (int i = 0; i < numberOfPetitionersToSpawn - 1; i++) {
                SpawnPetitioner();
                float delay = Random.Range(minSecToWaitBetweenSpawns, maxSecToWaitBetweenSpawns);
                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }

        public bool FindFreeSlotInFrontOf(GameObject instigator, out PetitionerSlot outSlot) {
            outSlot = null;
            // slots is sorted from nearest to farest from petitioner spawn point
            foreach (var slot in slots) {
                if (IsInFrontOf(instigator, slot.gameObject)) {
                    if (!slot.IsFree()) {
                        // non-free slot ahead, can't jump lines so stop search!
                        return outSlot != null;
                    }
                    else {
                        // found a slot that is farer away and free, target that one
                        outSlot = slot;
                    }
                }
            }
            return outSlot != null;
        }

        bool IsInFrontOf(GameObject instigator, GameObject target) {
            return target.transform.position.x < instigator.transform.position.x;
        }

        void HandlePetitionerLeaves() {
            SpawnPetitioner();
        }

        Petitioner SpawnPetitioner(Transform overrideTransform = default) {
            var petitioner = Instantiate(petitionerPrefab);
            var concern = concernsLibrary.concerns[Random.Range(0, concernsLibrary.concerns.Count)];
            petitioner.SetUp(overrideTransform ? overrideTransform : petitionerSpawnPoint.transform, petitionerLeavePoint.transform, concern, this);
            return petitioner;
        }
    }
}
