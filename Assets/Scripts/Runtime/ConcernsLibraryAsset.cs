using System.Collections.Generic;
using UnityEngine;

namespace Runtime {
    public sealed class ConcernsLibraryAsset : ScriptableObject {
        public List<ConcernAsset> concerns = new();
    }
}
