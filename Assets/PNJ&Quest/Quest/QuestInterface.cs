using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class QuestInterface
{
    private static Dictionary<string, GameObject> questPrefabDictionary = new();  // Dictionnaire pour stocker les prefabs
    private static GameObject questTextPrefab;  // Référence au prefab du texte de quête dans le dossier Resources/QuestUi

    static QuestInterface()
    {
        // Chargement du prefab depuis les ressources
        if (questTextPrefab == null)
        {
            questTextPrefab = Resources.Load<GameObject>("QuestUi/TextQuest");
        }
    }

    public static void AddQuestToUI(Quest quest, Transform contentTransform)
    {
        if (questTextPrefab != null && contentTransform != null)
        {
            // Vérifier si la quête existe déjà dans le dictionnaire
            if (!questPrefabDictionary.ContainsKey(quest.Name))
            {
                // Instancier un nouvel objet texte pour la quête
                GameObject questTextObject = Object.Instantiate(questTextPrefab, contentTransform);

                // Configurer le texte de la quête
                TextMeshProUGUI questText = questTextObject.GetComponent<TextMeshProUGUI>();
                if (questText != null)
                {
                    questText.text = GetTextForQuest(quest);
                        //quest.Name;  // Mettre à jour le texte avec le nom de la quête
                }

                // Ajouter l'objet texte au dictionnaire
                questPrefabDictionary[quest.Name] = questTextObject;
            }
            else
            {
                Debug.LogWarning($"La quête {quest.Name} est déjà ajoutée à l'interface utilisateur.");
            }
        }
        else
        {
            Debug.LogError("Le prefab de texte de quête ou le transform du content n'est pas configuré.");
        }
    }


    private static string GetTextForQuest(Quest quest)
    {
        // Vérifie si la quête est de type Discution
        if (quest.ConditionQuest.typeOfTheQuest == typeOfQuest.Discution)
        {
            // Récupère les noms des conditions pour les quêtes de type Discution
            var conditionNames = quest.ConditionQuest.condition
                .Select(cond => cond.name)
                .ToList();

            // Retourne les noms des conditions comme une chaîne de caractères séparée par des virgules
            return string.Join(", ", conditionNames);
        }
        else
        {
            // Pour les autres types de quête, passe tout le contenu des conditions en texte
            var conditionsText = quest.ConditionQuest.condition
                .Select(cond => $"Tué: {cond.name},  {cond.quantiteInProgress} /  {cond.quantite}")
                .ToList();

            // Retourne le texte des conditions comme une chaîne de caractères séparée par des sauts de ligne
            return string.Join("\n", conditionsText);
        }
    }

    public static void QuestKillingUpdate(Quest quest)
    {
        // Vérifie si le dictionnaire contient une entrée pour la quête donnée
        if (questPrefabDictionary.TryGetValue(quest.Name, out GameObject questTextObject))
        {
            // Obtient le composant TextMeshProUGUI pour mettre à jour le texte
            TextMeshProUGUI questText = questTextObject.GetComponent<TextMeshProUGUI>();

            if (questText != null)
            {
                // Met à jour le texte de l'objet avec la chaîne retournée par GetTextForQuest
                questText.text = GetTextForQuest(quest);
                Debug.Log($"Le texte de la quête {quest.Name} a été mis à jour.");
            }
            else
            {
                Debug.LogError("Le composant TextMeshProUGUI n'a pas été trouvé sur le prefab.");
            }
        }
        else
        {
            Debug.LogError($"Aucun prefab trouvé dans le dictionnaire pour la quête {quest.Name}.");
        }
    }
    public static void QuestToEarnReward(Quest quest)
    {
        // Vérifier si le dictionnaire contient un prefab pour la quête donnée
        if (questPrefabDictionary.TryGetValue(quest.Name, out GameObject questTextObject))
        {
            // Obtenir le composant TextMeshProUGUI pour mettre à jour le texte
            TextMeshProUGUI questText = questTextObject.GetComponent<TextMeshProUGUI>();

            if (questText != null)
            {
                string modifText = $"<s>{questText.text}</s>";
                // Garder l'ancien texte et ajouter une nouvelle ligne pour indiquer le QuestConfirmer
                modifText += $"\nSe rendre à {quest.QuestConfirmer} pour obtenir la récompense.";
                questText.text = modifText;
                Debug.Log($"Le texte de la quête {quest.Name} a été mis à jour pour indiquer le QuestConfirmer.");
            }
            else
            {
                Debug.LogError("Le composant TextMeshProUGUI n'a pas été trouvé sur le prefab.");
            }
        }
        else
        {
            Debug.LogError($"Aucun prefab trouvé dans le dictionnaire pour la quête {quest.Name}.");
        }
    }
    public static void QuestToDelete(Quest quest)
    {
        // Vérifier si le dictionnaire contient un prefab pour la quête donnée
        if (questPrefabDictionary.TryGetValue(quest.Name, out GameObject questTextObject))
        {
            // Supprimer le GameObject du transform du content
            if (questTextObject != null)
            {
                Object.Destroy(questTextObject);
            }

            // Retirer l'entrée du dictionnaire
            questPrefabDictionary.Remove(quest.Name);

           // Debug.Log($"Le prefab de la quête {quest.Name} a été supprimé de l'interface utilisateur.");
        }
        else
        {
            Debug.LogError($"Aucun prefab trouvé dans le dictionnaire pour la quête {quest.Name}.");
        }
    }
}