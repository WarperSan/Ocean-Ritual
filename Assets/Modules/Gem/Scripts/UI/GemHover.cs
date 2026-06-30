using ControllerModule.Interfaces.UI;
using System.Collections.Generic;
using TMPro;
using UIModule.Components;
using UnityEngine;
using UnityEngine.UI;

namespace GemModule.UI
{
    public class GemHover : HoverItem<GemData>, ITabable
    {
        #region Fields

        [Header("Fields")]
        [SerializeField]
        private Image Icon;

        [SerializeField]
        private ShowGemShape Form;

        [SerializeField]
        private StarsManager Stars;

        [SerializeField]
        private TextMeshProUGUI Level;

        [SerializeField]
        private GameObject Gun;

        [SerializeField]
        private GameObject Boat;

        [SerializeField]
        private GameObject Net;

        #endregion

        #region Data

        private const string DATA_PATH = "Inventory";

        private static Dictionary<string, GameObject> data;

        // R�cup�rer les objets GameObject � partir du dictionnaire
        private static void FetchData() => data = DictionaryGenerator.DictionaryGameObjectGenerator(DATA_PATH);

        #endregion

        // Affiche les informations de la gemme (stats) sur le UI
        private void ChangeInfoStat(GemData Gem)
        {
            // Ajouter les stats pour les armes
            if (Gem.typeWeapon != null && Gem.typeWeapon.Count > 0)
                PopulateStatUI(Gem.typeWeapon, Gun, Gem.LVL);

            // Ajouter les stats pour les bateaux
            if (Gem.typeBoat != null && Gem.typeBoat.Count > 0)
                PopulateStatUI(Gem.typeBoat, Boat, Gem.LVL);

            // Ajouter les stats pour les filets
            if (Gem.typeNet != null && Gem.typeNet.Count > 0)
                PopulateStatUI(Gem.typeNet, Net, Gem.LVL);
        }

        // M�thode g�n�rique pour remplir les UI en fonction des types (arme, bateau, filet)
        private void PopulateStatUI<TEnum>(List<TypeQuantity<TEnum>> list, GameObject parent, int lvl)
        {
            // Efface les enfants pr�c�dents s'il y en a
            foreach (Transform child in parent.transform)
                Destroy(child.gameObject);

            // Instancie et remplit les �l�ments horizontaux
            for (int i = 0; i < list.Count; i += 1) // Pour chaque paire d'�l�ments
            {
                // R�cup�re le prefab de "horizontale" � partir du dictionnaire
                if (!data.TryGetValue("horizontale", out GameObject prefab))
                {
                    Debug.LogWarning("Prefab 'horizontale' introuvable dans le dictionnaire.");
                    continue;
                }

                // Instancie le prefab de "horizontale" sous le parent correspondant (Gun, Boat, Net)
                GameObject horizontalInstance = Instantiate(prefab, parent.transform);

                int index = i;

                if (index < list.Count)
                {
                    Transform child = horizontalInstance.transform.GetChild(0);            // R�cup�re l'enfant (0 ou 1)
                    TextMeshProUGUI txt = child.GetComponentInChildren<TextMeshProUGUI>(); // R�cup�re le composant Text

                    if (txt != null)
                    {
                        // D�finit le texte avec le nom de l'enum et la quantit�
                        txt.text = $"{list[index].Type} : {list[index].Quantite}";

                        // Applique la couleur du texte bas�e sur le ratio
                        txt.color = GetTextColorForRatio(list[index].Quantite / lvl);
                    }
                }
            }
        }

        // Sous-fonction pour r�cup�rer la couleur en fonction du ratio
        private static Color GetTextColorForRatio(float ratio)
        {
            // Blanc pour ratio = 1
            if (ratio == 1)
                return Color.black;

            // Vert pour ratio = 2
            if (ratio == 2)
                return Color.green;

            // Violet clair pour ratio = 3
            if (ratio == 3)
                return new Color(0.7f, 0.4f, 0.7f);

            // Orange pour ratio = 4
            if (ratio == 4)
                return new Color(1f, 0.647f, 0f);

            // Par d�faut, la couleur est blanche
            return Color.white;
        }

        #region HoverItem

        /// <inheritdoc/>
        protected override void SetData(GemData gem)
        {
            if (data == null)
                FetchData();

            //this.Icon.sprite = gem.sprite;
            //this.Level.text = $"Level of the gems :  {gem.LVL}";
            ChangeInfoStat(gem);
            Form.Show(gem, gem);
            Stars.CreateStars(gem);
            navBar.Select(0);
        }

        #endregion

        #region ITabable

        [Header("ITabable")]
        [SerializeField]
        private NavBar navBar;

        /// <inheritdoc/>
        public void OnTabNext() => navBar.Next();

        /// <inheritdoc/>
        public void OnTabPrevious() => navBar.Previous();

        #endregion
    }
}