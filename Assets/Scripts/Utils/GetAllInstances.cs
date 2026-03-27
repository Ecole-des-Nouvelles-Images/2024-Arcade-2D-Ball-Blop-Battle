using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class AssetUtils
    {
        public static List<T> GetAllInstances<T>(string path) where T : ScriptableObject
        {
            List<T> assets = new List<T>();
        
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { path });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets;
        }
    }
}