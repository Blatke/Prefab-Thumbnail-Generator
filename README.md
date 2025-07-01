# Prefab Thumbnail Generator
Unity scripts for generating thumbnails from selected prefabs.

# Requirements
1. **Unity Editor** with the version newer than 2018.1.
2. **Newtonsoft.Json**. It requires Newtonsoft.Json to save the settings, please download it at first at https://github.com/JamesNK/Newtonsoft.Json, if you don't have it in Unity. Drag and drop the **net20** folder from the compression pack into your **Asset** folder in Unity. 

# Installation
Download the **Source code.zip** file for the latest version on the [Release](https://github.com/Blatke/Prefab-Thumbnail-Generator/releases) page. Uncompress it and there will be .cs files as below:
1. JsonRead.cs
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

![2025-07-02_015147](https://github.com/user-attachments/assets/0d3d3cdc-afee-41a1-bdca-f0de61c08132)

**Select one or more prefabs** that you want to create thumbnails from in your project window, then click the button of **Generate from Selected Prefabs**. The Generator will process it and save the images at the same path of the prefabs. 

The thumbnails will be named same as the prefabs. You can choose to give them a prefix and suffix beside the names of thumbnails.

If you haven't selected at least one prefab before clicking the button for generating, the Generator will give you a warning on the tab.

You can click "Save Settings" button at the right-top to persist your adjustments in settings. [v1.0.4 Updated]

# For HS2/AIS Modders using hooh Modding tools
Starting from v1.0.2 of the Generator, it can name the generated thumbnails in the format of HS2/AIS studio item thumbnail by reading the mod.xml file, and save the generated thumbnails in 'thumbs' folder. This will make your modding more convenient.

![2025-06-16_221215](https://github.com/user-attachments/assets/e58d57e2-9b3d-4c73-a079-ae73baac03d7)

As the screenshot shown above, if checking the option of **"Name by 'mod.xml' for StuioItem"**, the Generator will automatically check if any mod.xml exists outside the current 'prefabs' folder, and if there is such a file, it will further read the tags in that file. If the selected prefabs can be found their file names in the tags with the Object attributes, the corresponding studio item's names will be got and put with the big- and mid-category's ids. Then the thumbnails will be named in the format of HS2/AIS studio item thumbnail, such like _00000001-00000001-StudioItemName.png_. If there's no such a file or no corresponding tags found, the naming will be the names of the prefabs.

> [!TIP]
>
> 1. The .xml file has to be named like: **mod.xml**, **mod 1.xml** or **mod 2.xml**. Otherwise, the Generator cannot find it.
> 
> 2. Tags in** mod.sxml**, **mod 1.sxml** or **mod 2.sxml** that are used in hooh Modding Tools v0.7.0 can be also read. [1.0.5 Updated]
>
> 3. If the Generator find the **first** compatible .xml/.sxml file for a particular prefab, it will read it and stop searching.

When checking "Name by 'mod.xml' for StuioItem", the option of **"Save in 'thumbs' Folder"** is available. If check it, the thumbnails will be saved in the 'thumbs' folder outside the current 'prefabs' folder. If no such this folder found, it will create one and save them it.
