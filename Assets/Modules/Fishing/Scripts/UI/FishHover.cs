using TMPro;
using UIModule.Components;
using UnityEngine;
using UnityEngine.UI;

namespace FishingModule.UI
{
    /// <summary>
    /// Defines a hover item for a fish
    /// </summary>
    public class FishHover : HoverItem<FishData>
    {
        #region Fields

        [Header("Fields")]
        [SerializeField]
        private Image Icon;

        [SerializeField]
        private TextMeshProUGUI Name;

        [SerializeField]
        private TextMeshProUGUI Rarity;

        [SerializeField]
        private TextMeshProUGUI Description;

        #endregion

        /// <inheritdoc/>
        protected override void SetData(FishData data)
        {
            FishSO fish = data.fish;
            this.Icon.sprite = fish.Icon;
            this.Name.text = fish.GetColoredName();
            this.Rarity.text = fish.Rarity.ToString();
            this.Description.text = fish.Description;
        }
    }
}