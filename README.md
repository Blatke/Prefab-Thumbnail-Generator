# Prefab Thumbnail Generator
Unity scripts for generating thumbnails from selected prefabs.

# Installation
Download the **Source code.zip** file for the latest version on the [Release](https://github.com/Blatke/Prefab-Thumbnail-Generator/releases) page. Uncompress it and there will be .cs files as below:
1. AssetFileRead.cs
2. ModifyTextureImportSettings.cs
3. PathHepler.cs
4. PrefabThumbnailGenerator.cs
5. XmlFileRead.cs

Drag and drop the .cs files which you suppose to use into your project folder, **Asset/Editor/**. If you don't have this **Editor** folder, you need to create one in the Asset folder. 

> [!NOTE]
> 
> If you're updating this tool, please replace ALL the .cs files with the newly downloaded ones. Also, don't forget to RE-OPEN the Generator's window after finishing the updating.

# Use It
Go to Unity Editor, click the top menu: **Window -> Bl@ke -> Prefab Thumbnail Generator**, to show its window:

![2025-06-15_032112](https://github.com/user-attachments/assets/d4f9b6c0-a9d3-429d-ab0c-4a0bd5e02da3)

**Select one or more prefabs** that you want to create thumbnails from in your project window, then click the button of **Generate from Selected Prefabs**. The Generator will process it and save the images at the same path of the prefabs. 

The thumbnails will be named same as the prefabs. You can choose to give them a prefix and suffix beside the names of thumbnails.

If you haven't selected at least one prefab before clicking the button for generating, the Generator will give you a warning on the tab.

# For HS2/AIS Modders using hooh Modding tools
Starting from v1.0.2 of the Generator, it can name the generated thumbnails in the format of HS2/AIS studio item thumbnail by reading the mod.xml file, and save the generated thumbnails in 'thumbs' folder. This will make your modding more convenient.

![2025-06-16_221215](https://github.com/user-attachments/assets/e58d57e2-9b3d-4c73-a079-ae73baac03d7)

As the screenshot shown above, if checking the option of **"Name by 'mod.xml' for StuioItem"**, the Generator will automatically check if any mod.xml exists outside the current 'prefabs' folder, and if there is such a file, it will further read the tags in that file. If the selected prefabs can be found their file names in the tags with the Object attributes, the corresponding studio item's names will be got and put with the big- and mid-category's ids. Then the thumbnails will be named in the format of HS2/AIS studio item thumbnail, such like _00000001-00000001-StudioItemName.png_. If there's no such a file or no corresponding tags found, the naming will be the names of the prefabs.

When checking "Name by 'mod.xml' for StuioItem", the option of **"Save in 'thumbs' Folder"** is available. If check it, the thumbnails will be saved in the 'thumbs' folder outside the current 'prefabs' folder. If no such this folder found, it will create one and save them it.
