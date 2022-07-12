using UnityEngine;

[System.Serializable]
public class BuildingInfoGab
{
    [Header("Height")]
    public int staticHeight = 0;
    [Tooltip("If randomHeight is turned off, the staticHeight will be used.")]
    public bool randomHeight = true;
    public int minHeight = 0;
    public int maxHeight = 0;

    [Header("Rows")]
    public int staticRows = 0;
    [Tooltip("If randomRows is turned off, the staticRows will be used.")]
    public bool randomRows = true;
    public int minRows = 0;
    public int maxRows = 0;

    [Header("Row Depth")]
    [Range(0, 90)]
    public int rowDepthPercentage = 0;
    public bool randomDepth = false;

    [Header("Ground level")]
    public bool keepGroundFloorSquared = false;
    public int squaredFloorLevel = 0;

    [Header("Roof")]
    public bool buildRoofWall = false;
    public float roofWallHeight = 0.0f;

    [Header("Wall Textures")]
    public Material brickMaterial = null;

    [Header("Row Textures")]
    public Material firstRowMaterial = null;
    public Material lastRowMaterial = null;

    [Header("Roof Textures")]
    public Material roofMaterial = null;
    public Material roofWallMaterial = null;

    [Header("Direction (Enable only one)")]
    public bool faceLeft = false;
    public bool faceRight = false;
    public bool randomDirection = false;

    [Header("Airco")]
    public bool buildAirco = true;
}
