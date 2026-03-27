using System.Collections.Generic;
using Player.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectionCharacter
{
    public class BlopSelectionPanel : MonoBehaviour
    {
        [Header("Configuration UI")]
        [SerializeField] private RectTransform _container;
        [SerializeField] private Button _buttonPrefab;

        [Header("Settings")]
        [SerializeField] private string _resourcesPath = "ScriptableObject"; 

        private List<Blop> _allBlops = new();

        private void Start()
        {
            foreach (Transform child in _container)
            {
                Destroy(child.gameObject);
            }

            _allBlops.AddRange(Resources.LoadAll<Blop>(_resourcesPath));

            Debug.Log($"[{gameObject.name}] {_allBlops.Count} Blops trouvés dans Resources/{_resourcesPath}");

            foreach (Blop blop in _allBlops)
            {
                CreateButtonForBlop(blop);
            }
        }

        private void CreateButtonForBlop(Blop blopData)
        {
            Button newButton = Instantiate(_buttonPrefab, _container);
            newButton.name = $"Button_{blopData.name}";

            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                // On utilise le nom du fichier ScriptableObject comme texte
                buttonText.text = blopData.name; 
            }
            else
            {
                // Repli si tu utilises le Text classique d'Unity
                Text legacyText = newButton.GetComponentInChildren<Text>();
                if (legacyText != null) legacyText.text = blopData.name;
            }

            // Configuration de l'action au clic
            newButton.onClick.AddListener(() => OnBlopButtonClicked(blopData));
        }

        private void OnBlopButtonClicked(Blop selectedBlop)
        {
            Debug.Log($"Tu as cliqué sur le Blop : {selectedBlop.name} !");
        
            // C'est ici que tu ferais ta logique de sélection.
            // Par exemple, en l'envoyant au GameManager pour l'attribuer à un joueur.
            // Managers.GameManager.Instance.Players[0].Blop = selectedBlop;
        }
    }
}