using TMPro;
using UnityEngine;

namespace Script {
    public class DeathCounter : MonoBehaviour {
        private TextMeshProUGUI _textMeshProUGUI;
        
        private void Awake() {
            _textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Start() {
            UpdateText();
            GameManager.DeathCounter = this;
        }

        public void UpdateText() {
            _textMeshProUGUI.text = $"Attempts: {GameManager.DeathCount}";
        }
    }
}
