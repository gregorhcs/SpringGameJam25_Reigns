using System.Collections;
using UnityEngine;

namespace Runtime {
    sealed class GameLoop : MonoBehaviour {
        IEnumerator Start() {
            yield return null;
        }
    }
}
