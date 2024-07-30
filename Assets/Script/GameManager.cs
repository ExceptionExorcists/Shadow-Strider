using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; set; }
        public static PlayerController PlayerController { get; set; }
        public static WallhuggerController WallhuggerController1 { get; set; }
        public static WallhuggerController WallhuggerController2 { get; set; }
        public static ListenerScript ListenerScript{ get; set; }
        public static DeathCounter DeathCounter { get; set; }
        public static int DeathCount { get; set; }

        private GameManager() {
            Instance = this;
        }

        public static void Reset() {
            DeathCount++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            DeathCounter.UpdateText();
        }
    }
}
