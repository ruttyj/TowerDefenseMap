using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{


    [SerializeField]
    public GameObject groundPrefab;

    [SerializeField]
    public GameObject pathPrefab;

    [SerializeField]
    public GameObject borderPrefab;

    [SerializeField]
    public GameObject endZonePrefab;


    [SerializeField]
    public int mapWidth = 20;

    [SerializeField]
    public int mapLength = 50;

    public int borderSize = 2;



    // Start is called before the first frame update
    void Start()
    {
        spawnWorld();
    }


    void spawnWorld()
    {

        bool[,] pathExistsOnNode = new bool[mapWidth, mapLength];
        for (int x = 0; x < mapWidth; ++x)
            for (int z = 0; z < mapLength; ++z)
                pathExistsOnNode[x, z] = false;

        // Build odd ranges 
        float oddsTotal = 0;
        SortedDictionary<string, Vector2> Odds = new SortedDictionary<string, Vector2>();
        Odds.Add("previousDirection",   new Vector2(oddsTotal, oddsTotal += 2));
        Odds.Add("sidewaysDirection",   new Vector2(oddsTotal, oddsTotal += 1));
        Odds.Add("randomDirection",     new Vector2(oddsTotal, oddsTotal += 1));

        Vector2Int startPos = new Vector2Int(0, BetterRandomRange(borderSize, mapLength - borderSize));
        Vector2Int endPos = new Vector2Int(mapWidth - borderSize, BetterRandomRange(borderSize, mapLength - borderSize));



        //Vector2Int newDirection = new Vector2Int(1, 0);
        Vector2Int currentPosition = new Vector2Int(1, 0);
        Vector2Int prevPosition = new Vector2Int(1, 0);

        // Add beginning and end pathways
        for (int x = 0; x <= borderSize; ++x)
        {
            currentPosition.x = startPos.x + x;
            currentPosition.y = startPos.y;

            pathExistsOnNode[startPos.x + x, startPos.y] = true;
            pathExistsOnNode[endPos.x - x, endPos.y] = true;
        
            prevPosition.x = currentPosition.x;
            prevPosition.y = currentPosition.y;
        }

        // Randomly generate world





        // Spawn map prefabs
        for (int x = 0; x < mapWidth; ++x)
            for (int z = 0; z < mapLength; ++z)
            {
                if (pathExistsOnNode[x, z])
                {
                    Instantiate(pathPrefab, new Vector3(x, 0, z), Quaternion.identity);
                }
                else if (IsPositionInBorder(new Vector2Int(x, z)))
                {
                    Instantiate(borderPrefab, new Vector3(x, 0, z), Quaternion.identity);
                }
                else
                {
                    Instantiate(groundPrefab, new Vector3(x, 0, z), Quaternion.identity);
                }
            }
    }

    int BetterRandomRange(int min, int max)
    {
        return Mathf.RoundToInt(Random.Range(min * 100.0f, max * 100.0f) / 100.0f);
    }


    bool IsPositionInBorder(Vector2Int pos)
    {
        return (pos.x < borderSize || pos.x >= mapWidth - borderSize || pos.y < borderSize || pos.y >= mapLength - borderSize)
            && (pos.y < borderSize || pos.y >= mapLength - borderSize || pos.x < borderSize || pos.x >= mapWidth - borderSize);
    }

    bool IsPositionInBounds(Vector2Int pos)
    {
        return pos.x >= borderSize && pos.x < mapWidth - borderSize
           && pos.y >= borderSize && pos.y < mapLength - borderSize;
    }
}