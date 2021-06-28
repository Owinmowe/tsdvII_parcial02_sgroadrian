using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerrainGenerator : MonoBehaviour
{

    [SerializeField] TerrainConfiguration terrainConfig = null;

    GameObject currentTerrainGO = null;
    GameObject currentItemsGO = null;
    Spline terrainSpline = null;
    List<int> usedIndex = null;

    void Start()
    {
        CreateNewTerrain();
    }

    public void CreateNewTerrain()
    {
        if (currentTerrainGO != null)
        {
            Destroy(currentTerrainGO);
            Destroy(currentItemsGO);
        }
        currentTerrainGO = Instantiate(terrainConfig.terrainBase);
        currentTerrainGO.transform.parent = transform;
        terrainSpline = currentTerrainGO.GetComponent<SpriteShapeController>().spline;
        SetTerrainSize();
        usedIndex = new List<int>();
        SetNewRandomPoints();
        SmoothTerrain();
        currentItemsGO = new GameObject("Items");
        currentItemsGO.transform.parent = transform;
        SetLandingSites();
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
            Vector3 RandomPos = new Vector3(posX, terrainConfig.perlinMultiplier * Mathf.PerlinNoise(Random.Range(terrainSpline.GetPosition(i + 2).x - terrainConfig.nextPointRange, terrainSpline.GetPosition(i + 2).x + terrainConfig.nextPointRange), i + 2));
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
                    int aux = Random.Range(0, 1);
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
        if(terrainConfig.landings2XPerTerrain + terrainConfig.landings3XPerTerrain + terrainConfig.landings5XPerTerrain > terrainConfig.terrainPoints / 4)
        {
            throw new System.Exception("Terrain Generation: Too many landing spots for the ammount of points in map");
        }
        int aux = 0;
        while (aux < terrainConfig.landings2XPerTerrain)
        {
            int tempPosX = Random.Range(2, terrainConfig.terrainPoints - 1);
            if(!usedIndex.Contains(tempPosX - 1) && !usedIndex.Contains(tempPosX) && !usedIndex.Contains(tempPosX + 1))
            {
                usedIndex.Add(tempPosX - 1);
                usedIndex.Add(tempPosX);
                usedIndex.Add(tempPosX + 1);
                Vector3 pos1 = terrainSpline.GetPosition(tempPosX - 1);
                Vector3 pos2 = terrainSpline.GetPosition(tempPosX);
                Vector3 pos3 = terrainSpline.GetPosition(tempPosX + 1);
                terrainSpline.SetPosition(tempPosX - 1, new Vector3(pos1.x, pos2.y, 0));
                terrainSpline.SetPosition(tempPosX + 1, new Vector3(pos3.x, pos2.y, 0));
                Vector3 newLandingSitePos = new Vector3(pos2.x, pos2.y + terrainConfig.offsetFromSurface, 0);
                GameObject go = Instantiate(terrainConfig.landingSitePrefab, newLandingSitePos, Quaternion.identity, transform);
                go.transform.parent = currentItemsGO.transform;
                go.GetComponent<LandingSite>().SetLandingType(terrainConfig.baseLandingScore, terrainConfig.landingMultiplier2X, terrainConfig.landingSize2X, terrainConfig.landing2XUIColor);
                aux++;
            }
        }
        aux = 0;
        while (aux < terrainConfig.landings3XPerTerrain)
        {
            int tempPosX = Random.Range(2, terrainConfig.terrainPoints);
            if (!usedIndex.Contains(tempPosX - 1) && !usedIndex.Contains(tempPosX) && !usedIndex.Contains(tempPosX + 1))
            {
                usedIndex.Add(tempPosX - 1);
                usedIndex.Add(tempPosX);
                usedIndex.Add(tempPosX + 1);
                Vector3 pos1 = terrainSpline.GetPosition(tempPosX - 1);
                Vector3 pos2 = terrainSpline.GetPosition(tempPosX);
                terrainSpline.SetPosition(tempPosX - 1, new Vector3(pos1.x, pos2.y, 0));
                Vector3 newLandingSitePos = new Vector3(pos2.x, pos2.y + terrainConfig.offsetFromSurface, 0);
                GameObject go = Instantiate(terrainConfig.landingSitePrefab, newLandingSitePos, Quaternion.identity, transform);
                go.transform.parent = currentItemsGO.transform;
                go.GetComponent<LandingSite>().SetLandingType(terrainConfig.baseLandingScore, terrainConfig.landingMultiplier3X, terrainConfig.landingSize3X, terrainConfig.landing3XUIColor);
                aux++;
            }
        }
        aux = 0;
        while (aux < terrainConfig.landings5XPerTerrain)
        {
            int tempPosX = Random.Range(2, terrainConfig.terrainPoints - 1);
            if (!usedIndex.Contains(tempPosX - 1) && !usedIndex.Contains(tempPosX) && !usedIndex.Contains(tempPosX + 1))
            {
                usedIndex.Add(tempPosX - 1);
                usedIndex.Add(tempPosX);
                usedIndex.Add(tempPosX + 1);
                Vector3 pos2 = terrainSpline.GetPosition(tempPosX);
                Vector3 newLandingSitePos = new Vector3(pos2.x, pos2.y + terrainConfig.offsetFromSurface, 0);
                GameObject go = Instantiate(terrainConfig.landingSitePrefab, newLandingSitePos, Quaternion.identity, transform);
                go.transform.parent = currentItemsGO.transform;
                go.GetComponent<LandingSite>().SetLandingType(terrainConfig.baseLandingScore, terrainConfig.landingMultiplier5X, terrainConfig.landingSize5X, terrainConfig.landing5XUIColor);
                aux++;
            }
        }
    }

}
