using System.Collections.Generic;
using UnityEngine;

namespace Runtime {
    public sealed class ConcernsLibraryAsset : ScriptableObject {
        public List<ConcernAsset> concerns = new();
        List<ConcernAsset> remainingConcerns = new();

        public ConcernAsset NextConcern() {
            if (remainingConcerns.Count == 0) {
                RefillRemainingConcerns();
            }
            var selected = remainingConcerns[Random.Range(0, remainingConcerns.Count)];
            remainingConcerns.Remove(selected);
            return selected;
        }

        public void SetUp() {
            RefillRemainingConcerns();
        }

        void RefillRemainingConcerns() {
            remainingConcerns = new();
            foreach (var concern in concerns) {
                remainingConcerns.Add(concern);
            }
        }
    }
}
