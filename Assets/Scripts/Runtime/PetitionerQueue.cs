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
        float minSecToWaitBetweenSpawns = 0.1f;

        [SerializeField]
        float maxSecToWaitBetweenSpawns = 0.3f;

        [SerializeField]
        GameObject petitionerSpawnPoint = default;

        [SerializeField]
        GameObject queueStart = default;

        [SerializeField]
        GameObject queueEnd = default;

        List<PetitionerSlot> slots = new();

        protected void Start() {
            for (int slotIndex = 0; slotIndex < numberOfSlotsToGenerate; slotIndex++) {
                var slot = Instantiate(slotPrefab);
                slot.transform.position = Vector3.Lerp(queueStart.transform.position, queueEnd.transform.position, slotIndex / (numberOfSlotsToGenerate-1f));
                slots.Add(slot);
            }
            slots.Last().isThroneSlot = true;
            StartCoroutine(SpawnInitialPetitioners());
        }

        IEnumerator SpawnInitialPetitioners() {
            for (int i = 0; i < numberOfSlotsToGenerate; i++) {
                SpawnPetitioner();
                yield return new WaitForSeconds(Random.Range(minSecToWaitBetweenSpawns, maxSecToWaitBetweenSpawns));
            }
            yield return null;
        }

        public bool FindFreeSlotInFrontOf(GameObject instigator, out PetitionerSlot outSlot) {
            outSlot = null;
            foreach (var slot in slots) {
                if (IsInFrontOf(instigator, slot.gameObject)) {
                    if (!slot.IsFree()) {
                        // non-free slot ahead, can't jump lines so stop search!
                        return false;
                    }
                    outSlot = slot;
                    return true;
                }
            }
            return false;
        }

        bool IsInFrontOf(GameObject instigator, GameObject target) {
            return target.transform.position.x < instigator.transform.position.x;
        }

        // @TODO ghs: maybe move the code below into separate class?

        [SerializeField]
        Petitioner petitionerPrefab = default;

        [SerializeField]
        ConcernsLibraryAsset concernsLibrary = default;

        Petitioner SpawnPetitioner() {
            var petitioner = Instantiate(petitionerPrefab);

            var concern = concernsLibrary.concerns[Random.Range(0, concernsLibrary.concerns.Count)];
            petitioner.SetUp(petitionerSpawnPoint.transform, concern, this);

            return petitioner;
        }
    }
}
