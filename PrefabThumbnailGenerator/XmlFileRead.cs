// First created by Bl@ke on June 14, 2025.
// Version 1.0.0 on June 16, 2025.
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
namespace Blatke.General.XML
{
    public class XmlAttribute
    {
        public string value1, value2, value3, value4, value5;
    }

    public class XmlFileRead
    {
        private XElement root;
        private string[] _fileName;
        public bool isFileFound = false;
        public string path;
        public Dictionary<string, XmlAttribute> prop = new Dictionary<string, XmlAttribute>();
        public bool isMatched = false;
        public string[] index;
        public string[] index2;
        public string key;
        public string key2;
        public string key3;
        public string key4;

        public XmlFileRead(string[] fileName)
        {
            _fileName = fileName;
            XDocument xDoc;
            XmlFileGet(out xDoc);
            if (xDoc == null) return;
            root = xDoc.Root;
            if (root == null)
            {
                isFileFound = false;
                return;
            }
        }
        public void XmlFileToDict(int _recursion = 1, int searchMode = 0, int getMode = 0)
        {
            if (!isFileFound) return;
            IEnumerable<XElement> _elements = new[] { root };
            for (int i = 0; i < _recursion; i++)
            {
                _elements = _elements.SelectMany(x => x.Elements());
            }
            IEnumerable<XElement> _matches;
            switch (searchMode)
            {
                default:
                case 0: // Search for props where ONE designated attribute name has designated values;
                    _matches = _elements
                        .Where(e => index.Contains((string)e.Attribute(key)) &&
                        e.Attribute(key) != null &&
                        e.Attribute(key2) != null)
                        .ToList();
                    break;
                case 1:
                    _matches = _elements
                        .Where(e => index.Contains((string)e.Attribute(key)) &&
                        e.Attribute(key) != null &&
                        e.Attribute(key2) != null &&
                        e.Attribute(key3) != null &&
                        e.Attribute(key4) != null )
                        .ToList();
                    break;
            }
            if (_matches.Count() > 0)
            {
                isMatched = true;
            }
            else
            {
                isMatched = false; return;
            }
            string _dictKey;
            foreach (XElement m in _matches)
            {
                _dictKey = m.Attribute(key).Value.ToString();
                switch (getMode)
                {
                    default:
                    case 0:
                        if (!prop.ContainsKey(_dictKey))
                        {
                            prop.Add(_dictKey,
                                new XmlAttribute
                                {
                                    value1 = m.Attribute(key2).Value.ToString()
                                }
                            );
                        }
                        break;
                    case 1:
                        if (!prop.ContainsKey(_dictKey))
                        {
                            prop.Add(_dictKey,
                                new XmlAttribute
                                {
                                    value1 = m.Attribute(key2).Value.ToString(),
                                    value2 = m.Attribute(key3).Value.ToString(),
                                    value3 = m.Attribute(key4).Value.ToString()
                                }
                            );
                        }
                        break;
                }
            }
        }
        private void XmlFileGet(out XDocument xDoc)
        {
            xDoc = null;
            TextAsset xmlFile = null;
            foreach (string f in _fileName)
            {
                xmlFile = AssetDatabase.LoadAssetAtPath<TextAsset>(f);
                if (xmlFile == null)
                {
                    continue;
                }
                else
                {
                    path = f;
                    Debug.Log("XML Found: "+f+". ");
                    break;
                }
            }
            if (xmlFile == null)
            {
                isFileFound = false;
                Debug.Log("XML Found. ");
                return;
            }
            xDoc = XDocument.Parse(xmlFile.text);
            isFileFound = true;
        }
    }
}
