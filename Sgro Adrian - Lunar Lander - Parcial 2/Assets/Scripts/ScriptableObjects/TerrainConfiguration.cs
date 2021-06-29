using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "Terrain Configuration", menuName = "Terrain/Configuration")]
public class TerrainConfiguration : ScriptableObject
{

    [Header("Terrain Configuration")]
    public GameObject terrainBase = null;
    public TerrainLinesTypes currentTerrainLinesTypes = TerrainLinesTypes.Both;
    public int terrainSize = 100;
    public int terrainPoints = 150;
    public float nextPointRange = 2f;
    public float perlinMultiplier = 10f;
    [Header("Landing Configuration")]
    public GameObject landingSitePrefab = null;
    public float landingOffsetFromSurface = -3.5f;
    public int baseLandingScore = 100;
    public int landings2XPerTerrain = 7;
    public int landingMultiplier2X = 2;
    public float landingSize2X = .06f;
    public Color landing2XUIColor = Color.green;
    public int landings3XPerTerrain = 7;
    public int landingMultiplier3X = 3;
    public float landingSize3X = .04f;
    public Color landing3XUIColor = Color.yellow;
    public int landings5XPerTerrain = 7;
    public int landingMultiplier5X = 5;
    public float landingSize5X = .02f;
    public Color landing5XUIColor = Color.red;
    [Header("Wind Zone Configuration")]
    public GameObject windZonePrefab = null;
    public int windZonesAmountPerTerrain = 5;
    public float windZoneBaseStrenghtMinimun = .5f;
    public float windZoneBaseStrenghtMaximun = 1.5f;
    public float windZoneVarianceStrenght = .1f;
    [Header("Limits Configuration")]
    public GameObject hurracainePrefab = null;
    public int hurracaineOffsetFromEnds = 20;
    public float hurracaineOffsetFromGround = -3.5f;
    public int outOfGravityLimit = 20;

}
