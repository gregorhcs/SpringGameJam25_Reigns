using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Runtime {
    sealed class FactionBar : MonoBehaviour {
        [SerializeField, Expandable]
        FactionAsset asset;
        [SerializeField]
        SpriteRenderer bar;

        void Start() {
            bar.sprite = asset.banner;
            UpdateHeight(1000);
        }

        void LateUpdate() {
            UpdateHeight(Time.deltaTime);
        }

        [SerializeField]
        float smoothTime = 1;
        [SerializeField]
        float maxHeight = 6;

        [Header("Runtime")]
        [SerializeField]
        float height;
        [SerializeField]
        float speed;

        void UpdateHeight(float deltaTime) {
            if (bar && asset) {
                height = Mathf.SmoothDamp(height, asset.currentLoyalty, ref speed, smoothTime, float.PositiveInfinity, deltaTime);
                bar.size = bar.size.WithY(Mathf.Lerp(0, maxHeight, height / 100));
            }
        }
    }
}