using UnityEditor;

namespace Utils.Editor
{
    [CustomPropertyDrawer(typeof(StringSpriteDictionary))]
    public class AnyDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}