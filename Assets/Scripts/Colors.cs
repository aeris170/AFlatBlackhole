using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class Colors : MonoBehaviour
{
    public bool ReadFromXML;
    public TextAsset ColorsXMLFile;

    private Dictionary<string, Color[]> paletteToColor = new Dictionary<string, Color[]>();

    public Color[] ColorsFromXML;

    void Awake()
    {
        if (ReadFromXML)
        {
            ReadColorsFromXMLFile(ColorsXMLFile);
        }
    }

    private void ReadColorsFromXMLFile(TextAsset file)
    {
        int totalColorCount = 0;

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(file.text);
        foreach (XmlNode palette in doc.DocumentElement.ChildNodes)
        {
            string paletteName = palette.Attributes[0].Value;
            paletteToColor[paletteName] = new Color[palette.ChildNodes.Count];
            for (int i = 0; i < palette.ChildNodes.Count; ++i)
            {
                ColorUtility.TryParseHtmlString(palette.ChildNodes[i].InnerText, out paletteToColor[paletteName][i]);
            }
            totalColorCount += palette.ChildNodes.Count;
        }
        ColorsFromXML = new Color[totalColorCount];
        int index = 0;
        foreach (Color[] colors in paletteToColor.Values)
        {
            for (int i = 0; i < colors.Length; ++i)
            {
                ColorsFromXML[index++] = colors[i];
            }
        }
    }


    public Color GetRandomColorOfOrder(int i)
    {
        if (ReadFromXML)
        {
            return paletteToColor.ElementAt(Random.Range(0, paletteToColor.Count)).Value[i];
        }
        else
        {
            return Color.HSVToRGB(Random.Range(0, 360) / 360f, .2f, 1);
        }
    }

}
