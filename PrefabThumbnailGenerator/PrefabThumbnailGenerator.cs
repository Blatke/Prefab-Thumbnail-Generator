// First created on June 14, 2025.
// Version 1.0.0 on June 14, 2025.
/*
Click on the top menu Window -> Prefab Thumbnail Generator to show the window
*/
using UnityEngine;
using UnityEditor;
using S = System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Blatke.General.Texture
{
    public class PrefabThumbnailGenerator : EditorWindow
    {
        private int targetWidth = 128;
        private int targetHeight = 128;
        private string targetPrefix = "";
        private string targetSuffix = "";
        private int targetCompression = 0;
        private int targetType = 0;
        private bool targetMipMap = false;
        private List<string> savePath = new List<string>();
        private List<Object> prefabsToProcess = new List<Object>();
        private int _messageType = 0;
        private string _messageText = "";
        private bool _doMessageBubble = false;
        private bool isProcessing = false;

        [MenuItem("Window/Bl@ke/Prefab Thumbnail Generator")]
        public static void ShowWindow()
        {
            GetWindow<PrefabThumbnailGenerator>("Prefab Thumbnail Generator");
        }
        void OnGUI()
        {
            GUILayout.Label("Prefab Thumbnail Settings", EditorStyles.boldLabel);
            targetWidth = EditorGUILayout.IntField("Width", targetWidth);
            targetHeight = EditorGUILayout.IntField("Height", targetHeight);
            targetPrefix = EditorGUILayout.TextField("FileName Prefix", targetPrefix);
            targetSuffix = EditorGUILayout.TextField("FileName Suffix", targetSuffix);

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

            SavePathCheck(_savePath);
            string filePath = Path.Combine(_savePath, targetPrefix + prefab.name + targetSuffix + ".png");
            File.WriteAllBytes(filePath, bytes);
            ModifyTextureImportSettings textureSet = new ModifyTextureImportSettings(){type = targetType,
            compression = targetCompression,
            mipmap = targetMipMap};
            
            textureSet.SettingChangeProcess(filePath);

            Debug.Log($"Successfully saved thumbnail to: {filePath}");
        }
        void SavePathCheck(string _savePath)
        {
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
        }
    }
}