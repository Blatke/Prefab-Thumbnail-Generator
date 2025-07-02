// First created by Bl@ke on July 2, 2025.
// Version 1.0.1 on July 3, 2025.
/*
It requires Newtonsoft.Json to run this script, please download it at https://github.com/JamesNK/Newtonsoft.Json, if you don't have it in Unity.
*/

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Blatke.General.Json
{
    public class JsonRead
    {
        private readonly string JsonPath;
        public Dictionary<string, string> dict;
        public bool isChanged = false;
        public JsonRead(string _JsonPath)
        {
            dict = new Dictionary<string, string>() { };
            JsonPath = _JsonPath;
            Open();
        }
        public void Create()
        {
            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);

            string directory = Path.GetDirectoryName(JsonPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(JsonPath, json);
            // Debug.Log("File for settings created at: " + JsonPath + ".");
            AssetDatabase.Refresh();
        }
        public void Open()
        {
            if (!File.Exists(JsonPath))
            {
                // Debug.Log("No current file for settings found, need to create one. ");
                Create();
                return;
            }
            string json = File.ReadAllText(JsonPath);
            dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
        public void Update()
        {
            if (dict == null) return;
            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            File.WriteAllText(JsonPath, json);
            // Debug.Log("Settings updated. "); 
            AssetDatabase.Refresh();
            isChanged = false;
        }
        public void Write(string str1, string str2 = "")
        {
            if (!dict.ContainsKey(str1))
            {
                dict.Add(str1, str2);
                isChanged = true;
            }
            else
            {
                if (dict[str1] != str2)
                {
                    dict[str1] = str2;
                    isChanged = true;
                }
            }
        }
        public string Read(string _key, bool doUpdate = false)
        {
            if (dict.ContainsKey(_key))
            {
                return dict[_key];
            }
            else
            {
                if (doUpdate)
                {
                    Write(_key, "");
                }
                return "";
            }
        }
        public void SetRead(string _key, ref string variable, bool doUpdate = false)
        {
            string _read = Read(_key, doUpdate);
            if (!string.IsNullOrEmpty(_read))
            {
                variable = _read;
            }
        }
        public void SetRead(string _key, ref bool variable, bool doUpdate = false)
        {
            string _read = Read(_key, doUpdate);
            if (_read.ToLower() == "true")
            {
                variable = true;
            }
        }
        public void SetRead(string _key, ref int variable, bool doUpdate = false)
        {
            string _read = Read(_key, doUpdate);
            if (!string.IsNullOrEmpty(_read))
            {
                if (int.TryParse(_read, out int i))
                {
                    if (i != 0)
                    {
                        variable = i;
                    }
                }
            }
        }
        public void SetRead(string _key, ref Vector4 variable, bool doUpdate = false)
        {
            string _read = Read(_key, doUpdate);
            if (!string.IsNullOrEmpty(_read))
            {
                variable = ParseVector4(_read);
            }
        }
        public Vector4 ParseVector4(string vectorString)
        {
            string cleaned = vectorString
                .Replace("(", "")
                .Replace(")", "")
                .Replace(" ", "");
            string[] components = cleaned.Split(',');
            if (components.Length != 4)
            {
                return Vector4.zero;
            }
            if (float.TryParse(components[0], out float x) &&
                float.TryParse(components[1], out float y) &&
                float.TryParse(components[2], out float z) &&
                float.TryParse(components[3], out float w))
            {
                return new Vector4(x, y, z, w);
            }
            return Vector4.zero;
        }
    }
}