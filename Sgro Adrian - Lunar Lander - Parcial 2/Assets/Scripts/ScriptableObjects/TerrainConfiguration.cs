using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "Terrain Configuration", menuName = "Terrain/Configuration")]
public class TerrainConfiguration : ScriptableObject
{

    [Header("Terrain Config")]
    public GameObject terrainBase = null;
    public TerrainLinesTypes currentTerrainLinesTypes = TerrainLinesTypes.Both;
    public int terrainSize = 100;
    public int terrainPoints = 150;
    public float nextPointRange = 2f;
    public float perlinMultiplier = 10f;
    [Header("Landing Configuration")]
    public GameObject landingSitePrefab = null;
    public float offsetFromSurface = -3.5f;
    public int landings2XPerTerrain = 7;
    public int landingPoints2X = 2;
    public float landingSize2X = .06f;
    public Color landing2XUIColor = Color.green;
    public int landings3XPerTerrain = 7;
    public int landingPoints3X = 3;
    public Color landing3XUIColor = Color.yellow;
    public float landingSize3X = .04f;
    public int landings5XPerTerrain = 7;
    public int landingPoints5X = 5;
    public Color landing5XUIColor = Color.red;
    public float landingSize5X = .02f;
}
