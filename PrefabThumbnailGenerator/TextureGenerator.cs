// First created by Bl@ke on July 2, 2025.
// Version 1.0.2 on July 3, 2025.

using UnityEngine;
using UnityEditor;

namespace Blatke.General.Texture
{
    public class TextureGenerator
    {
        public Texture2D tex;
        public int width = 128;
        public int height = 128;
        public Vector4 color = new Vector4(1, 1, 1, 1);
        public TextureFormat textureFormat = TextureFormat.RGB24;
        public TextureGenerator(int _width = 128, int _height = 128, bool _isAlpha = false)
        {
            Dispose();
            width = _width; height = _height;
            textureFormat = (_isAlpha) ? TextureFormat.RGBA32 : TextureFormat.RGB24;
            tex = new Texture2D(width, height, textureFormat, false);
        }
        public void PureColor(Vector4 _color)
        {
            color = _color;
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            tex.SetPixels(pixels);
            tex.Apply();
        }
        public void Dispose()
        {
            if (tex != null)
            {
                if (!Application.isPlaying)
                {
                    Object.DestroyImmediate(tex);
                }
                tex = null;
            }
        }
    }
}