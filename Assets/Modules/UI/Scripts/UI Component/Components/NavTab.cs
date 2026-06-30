using DhafinFawwaz.AnimationUILib;
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
        [SerializeField]
        [Tooltip("Content that will be toggled by this tab")]
        private GameObject Content;

        [SerializeField]
        private UnityEvent OnOpen;

        [SerializeField]
        private UnityEvent OnClose;

        #region Select

        [Header("Select")]
        [SerializeField]
        [Tooltip("Animation played when this tab is selected or hovered")]
        private AnimationUI selectAnimation;

        [SerializeField]
        [Tooltip("Animation played when this tab is unselected")]
        private AnimationUI unselectAnimation;

        private bool isSelected;

        private void Select()
        {
            if (gameObject.activeInHierarchy && selectAnimation != null)
                selectAnimation.Play();

            isSelected = true;
        }

        private void UnSelect()
        {
            if (gameObject.activeInHierarchy && unselectAnimation != null)
                unselectAnimation.Play();

            isSelected = false;
        }

        #endregion

        /// <summary>
        /// Opens this tab
        /// </summary>
        public void Open()
        {
            Content.SetActive(true);

            if (!isSelected)
                Select();

            OnOpen?.Invoke();
        }

        /// <summary>
        /// Closes this tab
        /// </summary>
        public void Close()
        {
            Content.SetActive(false);

            if (isSelected)
                UnSelect();

            OnClose?.Invoke();
        }

        #region Hover

        /// <inheritdoc/>
        public void OnPointerEnter(PointerEventData eventData) => Select();

        /// <inheritdoc/>
        public void OnPointerExit(PointerEventData eventData) => UnSelect();

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            if (Content == null)
            {
                enabled = false;
                Debug.LogError($"Please add a content for the '{nameof(NavTab)}' named '{name}'.");
                return;
            }

            NavBar navBar = GetComponentInParent<NavBar>();

            if (navBar == null)
            {
                enabled = false;
                Debug.LogWarning($"Tried to add a '{nameof(NavTab)}' without parenting a '{nameof(NavBar)}'.");
                return;
            }

            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => navBar.SelectTab(this));
        }

        #endregion
    }
}