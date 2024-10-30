using DhafinFawwaz.AnimationUILib;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIModule.Components
{
    /// <summary>
    /// Component that determines a tab in a nav bar
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class NavTab : UIComponent, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Tooltip("Content that will be toggled by this tab")]
        private GameObject Content;

        [SerializeField]
        private UnityEvent OnOpen;

        [SerializeField]
        private UnityEvent OnClose;

        #region Select

        [Header("Select")]
        [SerializeField, Tooltip("Animation played when this tab is selected or hovered")]
        private AnimationUI selectAnimation;

        [SerializeField, Tooltip("Animation played when this tab is unselected")]
        private AnimationUI unselectAnimation;

        private bool isSelected = false;

        private void Select()
        {
            if (this.gameObject.activeInHierarchy && this.selectAnimation != null)
                this.selectAnimation.Play();

            this.isSelected = true;
        }

        private void UnSelect()
        {
            if (this.gameObject.activeInHierarchy && this.unselectAnimation != null)
                this.unselectAnimation.Play();

            this.isSelected = false;
        }

        #endregion

        /// <summary>
        /// Opens this tab
        /// </summary>
        public void Open()
        {
            this.Content.SetActive(true);

            if (!this.isSelected)
                this.Select();

            this.OnOpen?.Invoke();
        }

        /// <summary>
        /// Closes this tab
        /// </summary>
        public void Close()
        {
            this.Content.SetActive(false);

            if (this.isSelected)
                this.UnSelect();

            this.OnClose?.Invoke();
        }

        #region Hover

        /// <inheritdoc/>
        public void OnPointerEnter(PointerEventData eventData) => this.Select();

        /// <inheritdoc/>
        public void OnPointerExit(PointerEventData eventData) => this.UnSelect();

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            if (this.Content == null)
            {
                this.enabled = false;
                Debug.LogError($"Please add a content for the '{nameof(NavTab)}' named '{this.name}'.");
                return;
            }

            NavBar navBar = this.GetComponentInParent<NavBar>();
            if (navBar == null)
            {
                this.enabled = false;
                Debug.LogWarning($"Tried to add a '{nameof(NavTab)}' without parenting a '{nameof(NavBar)}'.");
                return;
            }

            Button button = this.GetComponent<Button>();
            button.onClick.AddListener(() => navBar.SelectTab(this));
        }

        #endregion
    }
}