// First created by Bl@ke on July 1, 2025.
// Version 1.0.0 on July 1, 2025.

using System.IO;
using UnityEngine;
using UnityEditor;

namespace Blatke.General.PathHepler
{
    public class ScriptFinder
    {
        public string GetScriptPath(ScriptableObject obj)
        {
            MonoScript script = MonoScript.FromScriptableObject(obj);
            return AssetDatabase.GetAssetPath(script);
        }
        public string GetScriptPath(MonoBehaviour behaviour)
        {
            MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
            return AssetDatabase.GetAssetPath(script);
        }
        public string GetParentFolder(string path)
        {
            string parent = Path.GetDirectoryName(path);
            return parent?.Replace('\\', '/');
        }
    }
}