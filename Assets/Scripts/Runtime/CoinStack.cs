using System;
using Slothsoft.Aseprite;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    [ExecuteAlways]
    sealed class CoinStack : MonoBehaviour {
        [SerializeField]
        internal int totalCoins = 0;

        [SerializeField]
        SpriteRenderer[] renderers = Array.Empty<SpriteRenderer>();

        [SerializeField, Expandable]
        AsepriteFile spriteSource = default;

        int coinsPerStack => spriteSource.info.frames.Length - 1;

        void Update() {
            if (!spriteSource) {
                return;
            }

            int coins = totalCoins;
            foreach (var renderer in renderers) {
                int stackCoins = Math.Clamp(coins, 0, coinsPerStack);
                if (spriteSource.TryGetSprite(stackCoins, out var sprite)) {
                    renderer.sprite = sprite;
                }

                coins -= stackCoins;
            }
        }
    }
}