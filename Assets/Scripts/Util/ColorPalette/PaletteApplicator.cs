using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using TMPro;
using Sirenix.OdinInspector;

public class PaletteApplicator : MonoBehaviour
{
    // Grab references to images and text components attached to current game object
    // Set their colors to be the color based on the palette and color index (default to 0)
    // Grab references to children and set their palette group to the same as the parent

    // Allow changing palette for super parent
    // Allow choosing whether to apply palette to images/text
    // 
    [SerializeField, Required]
    private ColorPalette _colorPalette;

    [SerializeField, ReadOnly]
    private Color _targetColor = Color.black;

    [SerializeField]
    private string _paletteID = "Default";

    [SerializeField]
    private int _colorIndex = 0;

    protected void OnAwake()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        GetTargetReference();
        SetTargetColor();
        ApplyTargetColor();
    }

    public void SetPalette(string paletteID)
    {
        if (_colorPalette.HasPalette(paletteID))
        {
            _paletteID = paletteID;
            _targetColor = _colorPalette.GetPaletteColor(_paletteID, _colorIndex);

        }
    }

    public void SetColorIndex(int colorIndex)
    {

    }

    private void GetTargetReference()
    {
        // Select from an enum, store the reference graphic or material in a field
        // In editor, if selected reference is invalid, show error
    }

    private void SetTargetColor()
    {
        // Set the target color to be used on the reference
        // Get from color palette based on palette ID and color index
    }

    private void ApplyTargetColor()
    {
        // Apply target color to current reference
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        UpdateColor();
    }
#endif
}
