using System;
using UnityEngine;

namespace Script {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostProcessing : MonoBehaviour
    {
        public Material effectMaterial;

        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if (effectMaterial != null) {
                Graphics.Blit(src, dest, effectMaterial);
            } else {
                Graphics.Blit(src, dest);
            }
        }
    }
}
