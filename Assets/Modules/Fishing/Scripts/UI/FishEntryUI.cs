using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FishingModule
{
    public class FishEntryUI : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _percent;

        public void Show(FishSO fish, float percent)
        {
            _image.sprite = fish.Icon;
            _name.text = fish.GetColoredName();
            _percent.text = percent + "%";
        }


        public FishSO test;
        public float percent;
        private void Start() {
            this.Show(test, percent);
        }
    }
}
