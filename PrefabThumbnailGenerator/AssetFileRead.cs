// First created by Bl@ke on July 1, 2025.
// Version 1.0.0 on July 1, 2025.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Blatke.General.Asset
{
    public class AssetFileRead
    {
        public Dict d;
        private string _path;
        public bool isChanged = false;
        public AssetFileRead(string path)
        {
            _path = path;
            Open();
            if (d == null)
            {
                Create();
                Open();
            }
        }
        public string Read(string _key, bool doUpdate = false)
        {
            if (d.dict.ContainsKey(_key))
            {
                return d.dict[_key];
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
        public void Write(string str1, string str2 = "")
        {
            if (!d.dict.ContainsKey(str1))
            {
                d.dict.Add(str1, str2);
                d.MarkDirty();
                isChanged = true;
            }
            else
            {
                if (d.dict[str1] != str2){
                    d.dict[str1] = str2;
                    d.MarkDirty();
                    isChanged = true;
                }
            }
        }

        public void Update()
        {
            if (d == null) return;
            if (d.dict.Count == 0) return;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            isChanged = false;
            // Debug.Log("Asset saved! ");
        }
        
        private void Open()
        {
            d = AssetDatabase.LoadAssetAtPath<Dict>(_path);
        }

        private void Create()
        {
            d = ScriptableObject.CreateInstance<Dict>();
            AssetDatabase.CreateAsset(d, _path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            // Debug.Log("Asset created! ");
        }
    }

    [System.Serializable]
    public class Dict : ScriptableObject
    {
        public SerializableDictionary dict = new SerializableDictionary();

        public void MarkDirty()
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }

        [System.Serializable]
        public class SerializableDictionary : SerializableDictionary<string, string> {}
    }

    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < keys.Count; i++)
            {
                this.Add(keys[i], values[i]);
            }
        }
    }
}