// First created by Bl@ke on July 4, 2025.
// Version 1.0.0 on July 4, 2025.

using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Blatke.General.PathHepler
{

    public class FileNaming
    {
        public Dictionary<string, int> repeatedNames;
        public FileNaming()
        {
            repeatedNames = new Dictionary<string, int>() { };
        }
        public string FileName(string filePath, string filename, string extension)
        {
            string _wholeName = Path.Combine(filePath, filename + "." + extension);
            string _modifiedFileName = "";

            if (repeatedNames.ContainsKey(_wholeName))
            {
                NameProcessing(1, filePath, filename, extension, ref _wholeName, _modifiedFileName);
            }
            else if (File.Exists(_wholeName))
            {
                NameProcessing(2, filePath, filename, extension, ref _wholeName, _modifiedFileName);
            }
            else
            {
                NameProcessing(0, filePath, filename, extension, ref _wholeName, _modifiedFileName);
            }            
            return _wholeName;
        }
        private void NameProcessing(int index, string filePath, string filename, string extension, ref string _wholeName, string _modifiedFileName)
        {
            if (index == 1) repeatedNames[_wholeName] += 1;
            if (index == 2) repeatedNames.Add(_wholeName, 1);
            if (index > 0)
            {
                _modifiedFileName = ModifyFileName(filename);
                _wholeName = FileName(filePath, _modifiedFileName, extension);
            }
            if (!repeatedNames.ContainsKey(_wholeName)) repeatedNames.Add(_wholeName, 1);
        }
        private string ModifyFileName(string filename)
        {
            string pattern = @"^[^\n]+[ ]{1}[0-9]*([0-9]{1})$";
            string replacement = @"$1";
            string _tailNumber = Regex.Replace(filename, pattern, replacement);
            if (_tailNumber == filename)
            {
                _tailNumber = "";
            }
            if (int.TryParse(_tailNumber, out int _tailNumberInt))
            {
                return filename.Substring(0, filename.Length - 1) + (_tailNumberInt + 1).ToString();
            }
            else
            {
                return filename + " 1";
            }
        }
        public void Clear()
        {
            if (repeatedNames == null) return;
            repeatedNames.Clear();
        }
    }
}