using Storages.Interfaces;
using UnityEngine;

namespace Storages
{
    [CreateAssetMenu(menuName = "Storages/Sprite", fileName = "SpriteStorage")]
    public class ScriptableSpriteStorage : ScriptableObject, IStorage<string, Sprite>
    {
        [NonReorderable]
        [SerializeField] private StringSpriteDictionary dictionary;

        public Sprite GetValue(string key)
        {
            var value = dictionary.ContainsKey(key) ? dictionary[key] : null;
            return value;
        }
    }
}