# Prefab-Thumbnail-Generator
Unity scripts for generating thumbnails from selected prefabs.

# Installation
Download the .zip file for the latest version on the [Release] page. Uncompress it and there will be .cs files as below:
1. ModifyTextureImportSettings.cs
2. ModifyTextureImportSettings_unity2018.cs
3. PrefabThumbnailGenerator.cs
4. PrefabThumbnailGenerator_unity2018.cs

For the users of Unity 2018, ModifyTextureImportSettings_**unity2018**.cs and PrefabThumbnailGenerator_**unity2018**.cs are suggested to use. For the users with the newer versions of Unity, please use the rest of them.

Drag and drop the .cs files which you suppose to use into your project folder, **Asset/Editor/**. If you don't have this **Editor** folder, you need to create one in the Asset folder.

# Use It
Go to Unity Editor, click the top menu: **Window -> Bl@ke -> Prefab Thumbnail Generator**, to show its window.

**Select one or more prefabs** that you want to create thumbnails from in your project window, then click the button of **Generate from Selected Prefabs**. The Generator will process it and save the images at the same path of the prefabs. 

The thumbnails will be named same as the prefabs. You can choose to give them a prefix and suffix beside the names of thumbnails.

If you haven't selected at least one prefab before clicking the button for generating, the Generator will give you a warning on the tab.
