using System;
using UnityEngine;

namespace Script {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostProcessing : MonoBehaviour
    {
        public static PostProcessing Instance { get; set; }
        public Material effectMaterial;

        private PostProcessing() {
            Instance = this;
        }

        private void Start() {
            effectMaterial.SetFloat("_PulseEffect", 0.0f);
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if (effectMaterial != null) {
                Graphics.Blit(src, dest, effectMaterial);
            } else {
                Graphics.Blit(src, dest);
            }
        }
    }
}
