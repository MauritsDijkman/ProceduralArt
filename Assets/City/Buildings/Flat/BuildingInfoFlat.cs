using UnityEngine;

[System.Serializable]
public class BuildingInfoFlat
{
    [Header("Height")]
    public int staticHeight = 0;
    [Tooltip("If randomHeight is turned off, the staticHeight will be used.")]
    public bool randomHeight = false;
    public int minHeight = 0;
    public int maxHeight = 0;

    [Header("Roof")]
    public bool buildRoofWall = false;
    public float roofWallHeight = 0.0f;

    [Header("Wall Textures")]
    public Material brickMaterial = null;

    [Header("Row Texture")]
    public Material firstRowMaterial = null;
    [Tooltip("If lastRowDifferent is turned off, the last row will have the brickMaterial.")]
    public bool lastRowDifferent = false;
    public Material lastRowMaterial = null;

    [Header("Roof Textures")]
    public Material roofMaterial = null;
    public Material roofWallMaterial = null;

    [Header("Airco")]
    public bool buildAirco = true;
}
