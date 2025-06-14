// First created on June 14, 2025.
// Version 1.0.1 on June 14, 2025.

#if UNITY_2020 || UNITY_2021 || UNITY_2022 || UNITY_2023 || UNITY_2024 || UNITY_6 || UNITY_7
#define UNITY_2020_OR_NEWER
#endif

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
namespace Blatke.General.Texture
{
    public class ModifyTextureImportSettings
    {
        public bool mipmap = false;
        public int compression = 0;
        public int type = 0;
        public Dictionary<bool, string> mipmapOnMenu;
        public Dictionary<int, string> compressionOnMenu;
        public Dictionary<int, string> typeOnMenu;

        private TextureImporter importer;

        public ModifyTextureImportSettings(bool _mipmap = false, int _compression = 0, int _type = 0)
        {
            mipmap = _mipmap; compression = _compression; type = _type;
        }
        public void SettingChangeProcess(List<string> processedFiles)
        {
            foreach (string filePath in processedFiles)
            {
                SettingChangeProcess(filePath);
            }
        }
        public void SettingChangeProcess(string[] processedFiles)
        {
            foreach (string filePath in processedFiles)
            {
                SettingChangeProcess(filePath);
            }
        }
        public void SettingChangeProcess(string filePath)
        {
            string assetPath = filePath.Replace("\\", "/");
            importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.mipmapEnabled = mipmap;
                TexType(type);
                TexCompression(compression);
                importer.SaveAndReimport();
            }
        }
        private void TexType(int type)
        {
            switch (type)
            {
                default:
                case 0:
                    importer.textureType = TextureImporterType.Default;
                    break;
                case 1:
                    importer.textureType = TextureImporterType.NormalMap;
                    break;
                case 2:
                    importer.textureType = TextureImporterType.GUI;
                    break;
                case 3:
                    importer.textureType = TextureImporterType.Sprite;
                    break;
                case 4:
                    importer.textureType = TextureImporterType.Cursor;
                    break;
                case 5:
                    importer.textureType = TextureImporterType.Cookie;
                    break;
                case 6:
                    importer.textureType = TextureImporterType.Lightmap;
                    break;
                case 7:
                    importer.textureType = TextureImporterType.SingleChannel;
                    break;
                #if UNITY_2020_OR_NEWER
                case 8:
                    importer.textureType = TextureImporterType.Shadowmask;
                    break;
                case 9:
                    importer.textureType = TextureImporterType.DirectionalLightmap;
                    break;
                #endif
            }
        }
        private void TexCompression(int compression)
        {
            switch (compression)
            {
                case 0:
                default:
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    break;
                case 1:
                    importer.textureCompression = TextureImporterCompression.Compressed;
                    break;
                case 2:
                    importer.textureCompression = TextureImporterCompression.CompressedHQ;
                    break;
                case 3:
                    importer.textureCompression = TextureImporterCompression.CompressedLQ;
                    break;
            }
        }
        public void OnMenu()
        {
            mipmapOnMenu = new Dictionary<bool, string>()
            {
                {false, "No MipMap"},
                {true, "Generate MipMap"}
            };
            compressionOnMenu = new Dictionary<int, string>()
            {
                {0, "Uncompressed"},
                {1, "Compressed"},
                {2, "Compressed - HQ"},
                {3, "Compressed - LQ"}
            };
            typeOnMenu = new Dictionary<int, string>()
            {
                {0, "Default"},
                {1, "NormalMap"},
                {2, "GUI"},
                {3, "Sprite"},
                {4, "Cursor"},
                {5, "Cookie"},
                {6, "Lightmap"},
                #if UNITY_2020_OR_NEWER
                    {7, "SingleChannel"},
                    {8, "Shadowmask"},
                    {9, "DirectionalLightmap"}
                #else
                    {7, "SingleChannel"}
                #endif        
            };
        }
    }
}