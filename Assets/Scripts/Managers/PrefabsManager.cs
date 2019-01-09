using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public static class PrefabsManager
    {
        private static Dictionary<PrefabsList, string> PrefabsStorage = new Dictionary<PrefabsList, string>();

        public enum PrefabsList
        {
           DefaultRoom,
           EmptyRoom,
           Player,
           DefaultArmor, 
           DefaultHeal,
           DefaultEnemy,
           FollowEnemy,
           DestroyEnemyAbility
        }

        public static GameObject LoadPrefab(PrefabsList feature)
        {
            try
            {
                AddPrefabToDictionary(feature);
                string path = PrefabsStorage[feature];
                return (GameObject)Resources.Load(path, typeof(GameObject));;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void AddPrefabToDictionary(PrefabsList feature)
        {
            if (!PrefabsStorage.ContainsKey(feature))
            {
                GetPrefabPath(feature);
            }
        }

        private static void GetPrefabPath(PrefabsList feature)
        {
            string path = string.Empty;
            
            switch (feature)
            {
                case PrefabsList.DefaultRoom:
                    path = "Prefabs/Environment/DefaultRoom";
                    break;     
                case PrefabsList.EmptyRoom:
                    path = "Prefabs/Environment/EmptyRoom";
                    break;               
                case PrefabsList.Player:
                    path = "Prefabs/Player";
                    break;  
                case PrefabsList.DefaultArmor:
                    path = "Prefabs/Environment/Buffs/Armor";
                    break;                
                case PrefabsList.DefaultEnemy:
                    path = "Prefabs/Environment/Enemy/DefaultEnemy";
                    break;               
                case PrefabsList.FollowEnemy:
                    path = "Prefabs/Environment/Enemy/FollowEnemy";
                    break;                
                case PrefabsList.DefaultHeal:
                    path = "Prefabs/Environment/Buffs/Heal";
                    break;               
                case PrefabsList.DestroyEnemyAbility:
                    path = "Prefabs/Ability/DestroyEnemyAbilityView";
                    break;
                default:
                    Debug.LogError("Prefab " + feature + " not declared");
                    break;
            }
            
            if (!string.IsNullOrEmpty(path))
            {
                PrefabsStorage.Add(feature, path);
            }
        }
    }
}