using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Color Palette")]
public class ColorPalette : SerializedScriptableObject
{
    public class Palette
    {
        [SerializeField, ColorPalette()]
        private List<Color> _colors = new List<Color>()
        {
            new Color()
        };

        public bool HasColor(int colorIndex)
        {
            return colorIndex >= _colors.Count;
        }

        public Color GetColor(int colorIndex)
        {
            if (colorIndex >= _colors.Count)
            {
                Debug.LogError($"Invalid color index: {colorIndex}, Number of colors: {_colors.Count}");
                return Color.black;
            }

            return _colors[colorIndex];
        }
    }

    [SerializeField, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout, KeyLabel = "Palette ID")]
    private Dictionary<string, Palette> _palettes = new Dictionary<string, Palette>()
    {
        { "Default", new Palette() }
    };

    public bool HasPalette(string paletteID)
    {
        if (_palettes.ContainsKey(paletteID))
        {
            return true;
        }

        return false;
    }

    public bool HasColor(string paletteID, int colorIndex)
    {
        if (HasPalette(paletteID))
        {
            return _palettes[paletteID].HasColor(colorIndex);
        }

        return false;
    }

    public Palette GetPalette(string paletteID)
    {
        if (_palettes.TryGetValue(paletteID, out Palette palette))
        {
            return palette;
        }

        Debug.LogError($"Invalid palette ID: {paletteID}", this);
        return null;
    }

    public Color GetPaletteColor(string paletteID = "Default", int colorIndex = 0)
    {
        return GetPalette(paletteID).GetColor(colorIndex);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ClearNullPalettes();
    }

    private void ClearNullPalettes()
    {
        try
        {
            foreach (KeyValuePair<string, Palette> pair in _palettes)
            {
                if (pair.Value == null)
                {
                    _palettes.Remove(pair.Key);
                }
            }
        }
        catch (InvalidOperationException) { }
    }
#endif
}
