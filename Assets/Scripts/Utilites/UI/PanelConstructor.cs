using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utilites.UI {

    public class PanelConstructor : MonoBehaviour {

        public TextAsset InputFile;

        public ImageWBullet[] ImageWBullets;

        private void Awake() {

            JsonUtility.FromJsonOverwrite(InputFile.text, ImageWBullets);
            Debug.Log(JsonUtility.FromJson<string>(InputFile.text).ToString());
        }
    }
}
