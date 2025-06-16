// First created by Bl@ke on June 14, 2025.
// Version 1.0.2 on June 16, 2025.
/*
Click on the top menu Window -> Prefab Thumbnail Generator to show the window
*/

#if UNITY_2020 || UNITY_2021 || UNITY_2022 || UNITY_2023 || UNITY_2024 || UNITY_6 || UNITY_7 || UNITY_2018 || UNITY_2019
#define UNITY_2018_OR_NEWER
#endif

using UnityEngine;
using UnityEditor;
using S = System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

#if UNITY_2018
using Blatke.General.XML;
#endif

namespace Blatke.General.Texture
{
    public class PrefabThumbnailGenerator : EditorWindow
    {
        private static string windowTitle = "Prefab Thumbnail Generator v1.0.2";
        private int targetWidth = 128;
        private int targetHeight = 128;
        private string targetPrefix = "";
        private string targetSuffix = "";
        private int targetCompression = 0;
        private int targetType = 0;
        private bool targetMipMap = false;
// #if UNITY_2018
        private bool targetReferenceMod = false;
        private bool targetSaveInThumbsFolder = false;
        private string _modXmlPath = "";
        private ModXmlRead m = null;
// #endif
        private List<string> savePath = new List<string>();
        private List<string> _filePath = new List<string>();
        private List<Object> prefabsToProcess = new List<Object>();
        private int _messageType = 0;
        private string _messageText = "";
        private bool _doMessageBubble = false;
        // private bool _isModXmlUse = false;
        private bool isProcessing = false;

        [MenuItem("Window/Bl@ke/Prefab Thumbnail Generator")]
        public static void ShowWindow()
        {
            GetWindow<PrefabThumbnailGenerator>(windowTitle);
        }
        void OnGUI()
        {
            GUILayout.Label("Prefab Thumbnail Settings", EditorStyles.boldLabel);
            targetWidth = EditorGUILayout.IntField("Width", targetWidth);
            targetHeight = EditorGUILayout.IntField("Height", targetHeight);
#if UNITY_2018
if (!targetReferenceMod){
#endif
            targetPrefix = EditorGUILayout.TextField("FileName Prefix", targetPrefix);
            targetSuffix = EditorGUILayout.TextField("FileName Suffix", targetSuffix);
#if UNITY_2018
}
#endif

#if UNITY_2018_OR_NEWER
            ModifyTextureImportSettings onPop = new ModifyTextureImportSettings();
            onPop.OnMenu();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Texture Type");
                targetType = EditorGUILayout.IntPopup(
                    targetType, 
                    onPop.typeOnMenu.Values.ToArray(),
                    onPop.typeOnMenu.Keys.ToArray(),                 
                    GUILayout.Width(130)
                );
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Compression");
                targetCompression = EditorGUILayout.IntPopup(
                    targetCompression, 
                    onPop.compressionOnMenu.Values.ToArray(),
                    onPop.compressionOnMenu.Keys.ToArray(),                 
                    GUILayout.Width(130)
                );
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Generate MipMap");
                GUILayout.FlexibleSpace();
                targetMipMap = GUILayout.Toggle(targetMipMap,"");
            }            
            GUILayout.EndHorizontal();
#endif

#if UNITY_2018
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent("Name by 'mod.xml' for StuioItem", "Will read mod.xml/mod.sxml outside current folder. If no corresponding tags found there, it will instead use prefab name."));
                GUILayout.FlexibleSpace();
                targetReferenceMod = GUILayout.Toggle(targetReferenceMod,"");
            }            
            GUILayout.EndHorizontal();

            if (targetReferenceMod){
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(new GUIContent("Save in 'thumbs' Folder", "Will save thumbnails in 'thumbs' folder outside current folder. If no such this folder found, it will create one."));
                    GUILayout.FlexibleSpace();
                    targetSaveInThumbsFolder = GUILayout.Toggle(targetSaveInThumbsFolder,"");
                }
                GUILayout.EndHorizontal();
            }
#endif

            if (GUILayout.Button("Generate from Selected Prefabs") && !isProcessing)
            {
                _doMessageBubble = false;
                ProcessPrefabs();
            }

            if (_doMessageBubble)
            {
                EditorGUILayout.BeginHorizontal();
                // EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    switch (_messageType)
                    {
                        default:
                        case 0:
                            EditorGUILayout.HelpBox(_messageText, MessageType.Info);
                            break;
                        case 1:
                            EditorGUILayout.HelpBox(_messageText, MessageType.Warning);
                            break;
                        case 2:
                            EditorGUILayout.HelpBox(_messageText, MessageType.Error);
                            break;
                        case 3:
                            EditorGUILayout.HelpBox(_messageText, MessageType.None);
                            break;
                    }
                    if (GUILayout.Button("×", GUILayout.Width(20)))
                    {
                        _doMessageBubble = false;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (isProcessing)
            {
                EditorGUILayout.HelpBox("Processing prefabs...", MessageType.Info);
                Repaint();
            }
        }
        void Message(bool doMessageBubble = false, int messageType = 0, string messageText = "")
        {
            if (doMessageBubble)
            {
                _doMessageBubble = true;
                _messageType = messageType;
                _messageText = messageText;
            }
            else
            {
                _doMessageBubble = false;
            }
        }
        void ProcessPrefabs()
        {
            prefabsToProcess.Clear();
            foreach (Object obj in Selection.objects)
            {
                if (obj is GameObject)
                {
                    prefabsToProcess.Add(obj);
                    savePath.Add(Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj)));
                }
            }
            if (prefabsToProcess.Count > 0)
            {
                isProcessing = true;
                // Processing the first prefab.
                EditorApplication.update += ProcessNextPrefab;
            }
            else
            {
                // Debug.LogWarning("No valid prefabs selected.");
                Message(true, 1, "[" + S.DateTime.Now.ToString("HH:mm:ss") + "] No valid prefabs selected.");
            }
        }
        void ProcessNextPrefab()
        {
            if (prefabsToProcess.Count == 0)
            {
                // Exit processing.
                isProcessing = false;
                EditorApplication.update -= ProcessNextPrefab;
                AssetDatabase.Refresh();

                ModifyTextureImportSettings textureSet = new ModifyTextureImportSettings()
                {
                    type = targetType,
                    compression = targetCompression,
                    mipmap = targetMipMap
                };
                textureSet.SettingChangeProcess(_filePath);

                // Debug.Log("All thumbnails saved!");
                Message(true, 0, "[" + S.DateTime.Now.ToString("HH:mm:ss") + "] All thumbnails saved!");
                return;
            }

            Object prefab = prefabsToProcess[0];
            prefabsToProcess.RemoveAt(0);
            string _savePath = savePath[0];
            savePath.RemoveAt(0);

            SaveThumbnail(prefab, _savePath);
        }
        void SaveThumbnail(Object prefab, string _savePath)
        {
            // Request for the thumbnail.
            Texture2D thumbnail = AssetPreview.GetAssetPreview(prefab);

            // If not ready, wait for a retry.
            if (thumbnail == null || AssetPreview.IsLoadingAssetPreview(prefab.GetInstanceID()))
            {
                prefabsToProcess.Add(prefab);
                savePath.Add(Path.GetDirectoryName(AssetDatabase.GetAssetPath(prefab)));
                return;
            }

            // Rescale the texture.
            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
            Graphics.Blit(thumbnail, rt);
            Texture2D resized = new Texture2D(targetWidth, targetHeight);
            RenderTexture.active = rt;
            resized.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resized.Apply();
            RenderTexture.ReleaseTemporary(rt);
            RenderTexture.active = null;

            // Save it as PNG.
            byte[] bytes = resized.EncodeToPNG();

            string image_fileName = "";

#if UNITY_2018
            // Reference mod.xml to name images.
            if (targetReferenceMod)
            {
                if (m == null || string.IsNullOrEmpty(_modXmlPath))
                {
                    m = new ModXmlRead(_savePath);
                    _modXmlPath = _savePath;
                }
                image_fileName = ReferenceModXML(_modXmlPath, prefab.name);
            }
#endif
            if (string.IsNullOrEmpty(image_fileName))
            {
                image_fileName = targetPrefix + prefab.name + targetSuffix;
            }

#if UNITY_2018
            if (targetReferenceMod && targetSaveInThumbsFolder)
            {
                DirectoryInfo _parentOfCurrentFolder = Directory.GetParent(_savePath);
                _savePath = Path.Combine(_parentOfCurrentFolder.ToString(), "thumbs");
            }
#endif

            SavePathCheck(_savePath);
            string filePath = Path.Combine(_savePath, image_fileName + ".png");
            _filePath.Add(filePath);
            File.WriteAllBytes(filePath, bytes);

            Debug.Log($"Successfully saved thumbnail to: {filePath}");
        }
        void SavePathCheck(string _savePath)
        {
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
        }

        private string ReferenceModXML(string modXmlPath, string prefabName)
        {
            if (string.IsNullOrEmpty(modXmlPath)) return "";

            if (_modXmlPath != modXmlPath || m == null)
            {
                _modXmlPath = modXmlPath;
                m = new ModXmlRead(modXmlPath);
            }

            m.xml.index = new string[]{
                prefabName
                };
            m.xml.key = "object";
            m.xml.key2 = "name";
            m.xml.key3 = "big-category";
            m.xml.key4 = "mid-category";
            m.xml.XmlFileToDict(3, 1, 1);
            if (m.xml.isFileFound && m.xml.isMatched && m.xml.prop.Count > 0 && m.xml.prop.ContainsKey(prefabName))
            {
                string _bigCategory = m.xml.prop[prefabName].value2;
                string _midCategory = m.xml.prop[prefabName].value3;
                string _itemName = m.xml.prop[prefabName].value1;
                return _bigCategory.PadLeft(8, '0') + "-" + _midCategory.PadLeft(8, '0') + "-" + _itemName;
            }
            else
            {
                return "";
            }
            
        }
    }
    public class ModXmlRead
    {
        public XmlFileRead xml;
        private string[] _xmlPath_mod;
        public ModXmlRead(string _prefabFolder)
        {
            DirectoryInfo directory = Directory.GetParent(_prefabFolder);
            string _directory = directory.ToString();
            _xmlPath_mod = new string[]{
                Path.Combine(_directory, @"mod.xml"),
                Path.Combine(_directory, @"mod.sxml"),
                Path.Combine(_directory, @"mod 1.xml"),
                Path.Combine(_directory, @"mod 1.sxml"),
                Path.Combine(_directory, @"mod 2.xml"),
                Path.Combine(_directory, @"mod 2.sxml"),
                Path.Combine(_directory, @"mod 3.xml"),
                Path.Combine(_directory, @"mod 3.sxml")
            };
            xml = new XmlFileRead(_xmlPath_mod);
        }
    }
}