using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerrainGenerator : MonoBehaviour
{

    [SerializeField] TerrainConfiguration terrainConfig = null;

    GameObject currentTerrainGO = null;
    Spline terrainSpline = null;
    GameObject currentLandings = null;
    List<int> ladingUsedPositions = new List<int>();
    GameObject currentWindZones = null;
    List<int> windZoneUsedPositions = new List<int>();

    public Action<int> OnSetNewLimit;

    void Start()
    {
        CreateNewTerrain();
    }

    public void CreateNewTerrain()
    {
        if (currentTerrainGO != null)
        {
            Destroy(currentTerrainGO);
            Destroy(currentLandings);
            Destroy(currentWindZones);
            ladingUsedPositions = new List<int>();
            windZoneUsedPositions = new List<int>();
        }
        currentTerrainGO = Instantiate(terrainConfig.terrainBase);
        currentTerrainGO.transform.parent = transform;
        terrainSpline = currentTerrainGO.GetComponent<SpriteShapeController>().spline;
        SetTerrainSize();
        SetNewRandomPoints();
        SmoothTerrain();
        SetLandingSites();
        SetWindZones();
        SetLimits();
    }

    void SetTerrainSize()
    {
        terrainSpline.SetPosition(0, terrainSpline.GetPosition(0) - Vector3.right * terrainConfig.terrainSize / 2);
        terrainSpline.SetPosition(1, terrainSpline.GetPosition(1) - Vector3.right * terrainConfig.terrainSize / 2);
        terrainSpline.SetPosition(2, terrainSpline.GetPosition(2) + Vector3.right * terrainConfig.terrainSize / 2);
        terrainSpline.SetPosition(3, terrainSpline.GetPosition(3) + Vector3.right * terrainConfig.terrainSize / 2);
    }

    void SetNewRandomPoints()
    {
        float distanceBetweenPoints = (float)terrainConfig.terrainSize / terrainConfig.terrainPoints;
        for (int i = 0; i < terrainConfig.terrainPoints; i++)
        {
            float posX = terrainSpline.GetPosition(i + 1).x + distanceBetweenPoints;
            Vector3 RandomPos = new Vector3(posX, terrainConfig.perlinMultiplier * Mathf.PerlinNoise(UnityEngine.Random.Range(terrainSpline.GetPosition(i + 2).x - terrainConfig.nextPointRange, terrainSpline.GetPosition(i + 2).x + terrainConfig.nextPointRange), i + 2));
            terrainSpline.InsertPointAt(i + 2, RandomPos);
        }
    }

    void SmoothTerrain()
    {
        for (int i = 2; i < terrainConfig.terrainPoints + 2; i++)
        {
            switch (terrainConfig.currentTerrainLinesTypes)
            {
                case TerrainLinesTypes.Linear:
                    terrainSpline.SetTangentMode(i, ShapeTangentMode.Linear);
                    break;
                case TerrainLinesTypes.Curved:
                    terrainSpline.SetTangentMode(i, ShapeTangentMode.Continuous);
                    terrainSpline.SetLeftTangent(i, Vector3.left * 0.5f);
                    terrainSpline.SetRightTangent(i, Vector3.right * 0.5f);
                    break;
                case TerrainLinesTypes.Both:
                    int aux = UnityEngine.Random.Range(0, 1);
                    ShapeTangentMode nextShape = aux == 0 ? ShapeTangentMode.Continuous : ShapeTangentMode.Linear;
                    terrainSpline.SetTangentMode(i, nextShape);
                    terrainSpline.SetLeftTangent(i, Vector3.left * 0.5f);
                    terrainSpline.SetRightTangent(i, Vector3.right * 0.5f);
                    break;
                default:
                    break;
            }
        }
    }

    void SetLandingSites()
    {
        currentLandings = new GameObject("Landing Positions");
        currentLandings.transform.parent = transform;
        if (terrainConfig.landings2XPerTerrain + terrainConfig.landings3XPerTerrain + terrainConfig.landings5XPerTerrain > terrainConfig.terrainPoints / 4)
        {
            throw new System.Exception("Terrain Generation: Too many landing spots for the ammount of points in map");
        }
        int aux = 0;
        while (aux < terrainConfig.landings2XPerTerrain)
        {
            int tempPosX = UnityEngine.Random.Range(2, terrainConfig.terrainPoints - 1);
            if(!ladingUsedPositions.Contains(tempPosX - 1) && !ladingUsedPositions.Contains(tempPosX) && !ladingUsedPositions.Contains(tempPosX + 1))
            {
                ladingUsedPositions.Add(tempPosX - 1);
                ladingUsedPositions.Add(tempPosX);
                ladingUsedPositions.Add(tempPosX + 1);
                Vector3 pos1 = terrainSpline.GetPosition(tempPosX - 1);
                Vector3 pos2 = terrainSpline.GetPosition(tempPosX);
                Vector3 pos3 = terrainSpline.GetPosition(tempPosX + 1);
                terrainSpline.SetPosition(tempPosX - 1, new Vector3(pos1.x, pos2.y, 0));
                terrainSpline.SetPosition(tempPosX + 1, new Vector3(pos3.x, pos2.y, 0));
                Vector3 newLandingSitePos = new Vector3(pos2.x, pos2.y + terrainConfig.landingOffsetFromSurface, 0);
                GameObject go = Instantiate(terrainConfig.landingSitePrefab, newLandingSitePos, Quaternion.identity, transform);
                go.transform.parent = currentLandings.transform;
                go.GetComponent<LandingSite>().SetLandingType(terrainConfig.baseLandingScore, terrainConfig.landingMultiplier2X, terrainConfig.landingSize2X, terrainConfig.landing2XUIColor);
                aux++;
            }
        }
        aux = 0;
        while (aux < terrainConfig.landings3XPerTerrain)
        {
            int tempPosX = UnityEngine.Random.Range(2, terrainConfig.terrainPoints);
            if (!ladingUsedPositions.Contains(tempPosX - 1) && !ladingUsedPositions.Contains(tempPosX) && !ladingUsedPositions.Contains(tempPosX + 1))
            {
                ladingUsedPositions.Add(tempPosX - 1);
                ladingUsedPositions.Add(tempPosX);
                ladingUsedPositions.Add(tempPosX + 1);
                Vector3 pos1 = terrainSpline.GetPosition(tempPosX - 1);
                Vector3 pos2 = terrainSpline.GetPosition(tempPosX);
                terrainSpline.SetPosition(tempPosX - 1, new Vector3(pos1.x, pos2.y, 0));
                Vector3 newLandingSitePos = new Vector3(pos2.x, pos2.y + terrainConfig.landingOffsetFromSurface, 0);
                GameObject go = Instantiate(terrainConfig.landingSitePrefab, newLandingSitePos, Quaternion.identity, transform);
                go.transform.parent = currentLandings.transform;
                go.GetComponent<LandingSite>().SetLandingType(terrainConfig.baseLandingScore, terrainConfig.landingMultiplier3X, terrainConfig.landingSize3X, terrainConfig.landing3XUIColor);
                aux++;
            }
        }
        aux = 0;
        while (aux < terrainConfig.landings5XPerTerrain)
        {
            int tempPosX = UnityEngine.Random.Range(2, terrainConfig.terrainPoints - 1);
            if (!ladingUsedPositions.Contains(tempPosX - 1) && !ladingUsedPositions.Contains(tempPosX) && !ladingUsedPositions.Contains(tempPosX + 1))
            {
                ladingUsedPositions.Add(tempPosX - 1);
                ladingUsedPositions.Add(tempPosX);
                ladingUsedPositions.Add(tempPosX + 1);
                Vector3 pos2 = terrainSpline.GetPosition(tempPosX);
                Vector3 newLandingSitePos = new Vector3(pos2.x, pos2.y + terrainConfig.landingOffsetFromSurface, 0);
                GameObject go = Instantiate(terrainConfig.landingSitePrefab, newLandingSitePos, Quaternion.identity, transform);
                go.transform.parent = currentLandings.transform;
                go.GetComponent<LandingSite>().SetLandingType(terrainConfig.baseLandingScore, terrainConfig.landingMultiplier5X, terrainConfig.landingSize5X, terrainConfig.landing5XUIColor);
                aux++;
            }
        }
    }

    void SetWindZones()
    {
        currentWindZones = new GameObject("Windzones");
        currentWindZones.transform.parent = transform;
        if (terrainConfig.windZonesAmountPerTerrain > terrainConfig.terrainPoints / 4)
        {
            throw new System.Exception("Terrain Generation: Too many wind zones for the ammount of points in map");
        }
        int aux = 0;
        while (aux < terrainConfig.windZonesAmountPerTerrain)
        {
            int tempPosX = UnityEngine.Random.Range(2, terrainConfig.terrainPoints - 1);
            if (!windZoneUsedPositions.Contains(tempPosX - 1) && !windZoneUsedPositions.Contains(tempPosX) && !windZoneUsedPositions.Contains(tempPosX + 1))
            {
                windZoneUsedPositions.Add(tempPosX - 1);
                windZoneUsedPositions.Add(tempPosX);
                windZoneUsedPositions.Add(tempPosX + 1);
                Vector3 pos1 = terrainSpline.GetPosition(tempPosX - 1);
                Vector3 pos2 = terrainSpline.GetPosition(tempPosX);
                Vector3 pos3 = terrainSpline.GetPosition(tempPosX + 1);
                GameObject go = Instantiate(terrainConfig.windZonePrefab, pos2, Quaternion.identity, currentWindZones.transform);
                float randomBaseForce = UnityEngine.Random.Range(terrainConfig.windZoneBaseStrenghtMinimun, terrainConfig.windZoneBaseStrenghtMaximun);
                go.GetComponent<WindZoneController>().SetWindzone(randomBaseForce, terrainConfig.windZoneVarianceStrenght);
                aux++;
            }
        }
    }

    void SetLimits()
    {
        Vector3 pos1 = new Vector3(terrainSpline.GetPosition(terrainConfig.hurracaineOffsetFromEnds).x, terrainSpline.GetPosition(terrainConfig.hurracaineOffsetFromEnds).y - terrainConfig.hurracaineOffsetFromGround);
        Vector3 pos2 = new Vector3(terrainSpline.GetPosition(terrainConfig.terrainPoints - terrainConfig.hurracaineOffsetFromEnds).x, terrainSpline.GetPosition(terrainConfig.terrainPoints - terrainConfig.hurracaineOffsetFromEnds).y - terrainConfig.hurracaineOffsetFromGround);
        GameObject go1 = Instantiate(terrainConfig.hurracainePrefab, pos1, Quaternion.identity, currentWindZones.transform);
        go1.GetComponent<AreaEffector2D>().forceAngle = 90;
        GameObject go2 = Instantiate(terrainConfig.hurracainePrefab, pos2, Quaternion.identity, currentWindZones.transform);
        go2.GetComponent<AreaEffector2D>().forceAngle = 90;
        OnSetNewLimit?.Invoke(terrainConfig.outOfGravityLimit);
    }

}
