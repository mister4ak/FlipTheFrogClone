using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class Tools
    {
        [MenuItem("Tools/Clear Prefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
