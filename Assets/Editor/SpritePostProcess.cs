using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpritePostProcess : AssetPostprocessor
{
    private void OnPreprocessAsset()
    {
        TextureImporter importer = assetImporter as TextureImporter;
        if (importer != null)
        {
            importer.spritePixelsPerUnit = 1;
            importer.textureType = TextureImporterType.Sprite;
        }
    }
}