using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UtilsModule;

public class UiBSGBN : Singleton<UiBSGBN>
{
   
    [SerializeField] private GameObject content;  // Parent pour les objets instanciés
    [SerializeField] private GameObject Horizontal;  // Préfab contenant un layout horizontal pour 2 StatConteneur
    [SerializeField] private GameObject StatConteneur;  // Préfab pour afficher les stats

    public void CreateUiGBNUpgrade(List<UpgradeStats> ListStat)
    {
        // Nettoyer le contenu précédent
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // Compteur pour vérifier si deux StatConteneur doivent être placés dans le même Horizontal
        GameObject currentHorizontalInstance = null;
        int counter = 0;

        // Parcourir chaque élément de la liste ListStat
        foreach (UpgradeStats upgradeStat in ListStat)
        {
            // Si le compteur est à 0 ou est un multiple de 2, on instancie un nouvel Horizontal
            if (counter % 2 == 0)
            {
                currentHorizontalInstance = Instantiate(Horizontal, content.transform);
            }

            // Instancier un nouveau StatConteneur et l'ajouter à l'Horizontal
            GameObject statConteneurInstance = Instantiate(StatConteneur, currentHorizontalInstance.transform);

            // Accéder au premier composant TextMeshPro dans les enfants de StatConteneur
            TextMeshProUGUI textComponent = statConteneurInstance.GetComponentInChildren<TextMeshProUGUI>();

            // Construire la chaîne de caractères pour l'affichage
            string statsText = "";
            statsText += upgradeStat.name + "  Cost: " + upgradeStat.upgradeCost + "\n";
            for (int i = 0; i < upgradeStat.baseStats.Count; i++)
            {
                string baseStatText = upgradeStat.baseStats[i].name + " : " + upgradeStat.baseStats[i].quantity;
                string previewStatText = " -> " + upgradeStat.previewStats[i].quantity;

                // Ajout d'un saut de ligne après chaque stat, sauf pour la dernière ligne
                statsText += baseStatText + previewStatText + "\n";
            }

            // Supprimer le dernier saut de ligne
            statsText = statsText.TrimEnd('\n');

            // Affecter le texte complet au TextMeshPro du StatConteneur
            textComponent.text = statsText;

            // Incrémenter le compteur
            counter++;
        }
    }
}