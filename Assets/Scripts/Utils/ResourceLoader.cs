using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
    public static class ResourceLoader
    {
        /// <summary>
        /// Récupère tous les ScriptableObjects d'un type T dans Resources/subPath
        /// </summary>
        /// <param name="subPath">Chemin RELATIF au dossier Resources (ex: "Maps" ou "Data/Items")</param>
        public static List<T> GetAllScriptables<T>(string subPath) where T : ScriptableObject
        {
            List<T> result = new List<T>();

            // LoadAll charge tout ce qui correspond au type T dans le dossier spécifié
            T[] assets = Resources.LoadAll<T>(subPath);

            if (assets == null || assets.Length == 0)
            {
                Debug.LogWarning($"[ResourceLoader] Aucun ScriptableObject de type {typeof(T).Name} trouvé dans : Resources/{subPath}");
                return result;
            }

            result.AddRange(assets);
            Debug.Log($"[ResourceLoader] {result.Count} éléments chargés depuis Resources/{subPath}");
            
            return result;
        }
    }
}