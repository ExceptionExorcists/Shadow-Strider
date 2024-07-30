using UnityEngine;

namespace Script {
    public class MetalDetector : MonoBehaviour {
        private bool _powered = true;
        private bool _detected;

        public AudioClip powerDown;
        public AudioClip powerUp;
        private AudioSource AS;

        private void Start()
        {
            AS = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other) {
            if (!_powered || _detected) return;

            _detected = true;

            GameManager.ListenerScript.EnterHuntingState( transform.position, GameManager.PlayerController.gameObject, true);
            AS.Play();
        }

        public void TogglePower() {
            _powered = !_powered;
            _detected = false;
            if (_powered)
            {
                AS.PlayOneShot(powerUp);
            }
            else
            {
                AS.PlayOneShot(powerDown);
            }
            
        }
    }
}
