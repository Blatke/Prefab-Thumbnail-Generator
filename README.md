# Prefab-Thumbnail-Generator
Unity scripts for generating thumbnails from selected prefabs.

# Installation
Download the **Source code.zip** file for the latest version on the [Release](https://github.com/Blatke/Prefab-Thumbnail-Generator/releases) page. Uncompress it and there will be .cs files as below:
1. ModifyTextureImportSettings.cs
2. ModifyTextureImportSettings_unity2018.cs
3. PrefabThumbnailGenerator.cs
4. PrefabThumbnailGenerator_unity2018.cs

For the users of Unity 2018, ModifyTextureImportSettings_**unity2018**.cs and PrefabThumbnailGenerator_**unity2018**.cs are suggested to use. For the users with the newer versions of Unity, please use the rest of them.

Drag and drop the .cs files which you suppose to use into your project folder, **Asset/Editor/**. If you don't have this **Editor** folder, you need to create one in the Asset folder.

# Use It
Go to Unity Editor, click the top menu: **Window -> Bl@ke -> Prefab Thumbnail Generator**, to show its window.

For Unity 2018:

![2025-06-15_031852](https://github.com/user-attachments/assets/8abdc696-71e1-4360-8e33-0847704deb74)

For Unity's newer versions:

![2025-06-15_032112](https://github.com/user-attachments/assets/d4f9b6c0-a9d3-429d-ab0c-4a0bd5e02da3)

**Select one or more prefabs** that you want to create thumbnails from in your project window, then click the button of **Generate from Selected Prefabs**. The Generator will process it and save the images at the same path of the prefabs. 

The thumbnails will be named same as the prefabs. You can choose to give them a prefix and suffix beside the names of thumbnails.

If you haven't selected at least one prefab before clicking the button for generating, the Generator will give you a warning on the tab.
