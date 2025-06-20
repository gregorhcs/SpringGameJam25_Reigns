using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class EmojiCannon : MonoBehaviour {
        [SerializeField]
        float minDeltaToShoot = 1;
        [SerializeField]
        SerializableKeyValuePairs<ParticleSystem, float> cannons = new();

        internal void Shoot(float change) {
            change *= Random.value;
            if (Mathf.Abs(change) < minDeltaToShoot) {
                return;
            }

            var cannon = cannons
                .Where(c => Mathf.Sign(c.Value) == Mathf.Sign(change))
                .OrderBy(c => Mathf.Abs(change - c.Value))
                .Select(c => c.Key)
                .FirstOrDefault();

            if (!cannon) {
                return;
            }

            cannon.Emit(Mathf.CeilToInt(Mathf.Abs(change)));
        }
    }
}
