using System.Collections.Generic;
using UnityEngine;

public class HexMapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject standardHexPrefab;
    [SerializeField] private GameObject[] hexPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject machinePrefab;
    [SerializeField] private int width = 50;   
    [SerializeField] private int height = 50; 
    [SerializeField] private long seed;

    private float hexWidth;
    private float hexHeight;
    private Vector3 startPos;
    private OpenSimplexNoise noise;
    private MapCoordinates _mapCoordinates;
    private HexTile playerOnTile;

    void Start()
    {
        _mapCoordinates = MapCoordinates.GetInstance();
        if (seed == 0)
        {
            seed = (long)Random.Range(1000000000, 9999999999);
        }
        noise = new OpenSimplexNoise(seed);
        hexWidth = standardHexPrefab.GetComponent<Renderer>().bounds.size.x;
        hexHeight = standardHexPrefab.GetComponent<Renderer>().bounds.size.z;
        startPos = new Vector3(-width / 2f * hexWidth, 0, -height / 2f * hexHeight);
        
        CreateHexMap();         
    }

    void CreateHexMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = CalculateHexPosition(x, z);
                HexTile hexTileObject;
                var noiseResult = (float)noise.Evaluate(x * 0.1, z * 0.1);

                if (noiseResult < 0.01f)
                {
                    hexTileObject = Instantiate(hexPrefab[0], pos, Quaternion.identity).GetComponent<HexTile>();
                    hexTileObject.Initialize("Water", x, z, 0, true, 0,0);
                }
                else if (noiseResult < 0.3f)
                {
                    hexTileObject = Instantiate(hexPrefab[1], pos, Quaternion.identity).GetComponent<HexTile>();
                    hexTileObject.Initialize("Grass", x, z, 1, true, 1 ,1);
                }
                else if (noiseResult < 0.4f)
                {
                    hexTileObject = Instantiate(hexPrefab[2], pos, Quaternion.identity).GetComponent<HexTile>();
                    hexTileObject.Initialize("Flower", x, z, 1, true , 1, 1);
                }
                else if (noiseResult < 0.7f)
                {
                    hexTileObject = Instantiate(hexPrefab[3], pos, Quaternion.identity).GetComponent<HexTile>();
                    hexTileObject.Initialize("Forest", x, z, 1, true , 2, 2);
                }
                else
                {
                    hexTileObject = Instantiate(hexPrefab[4], pos, Quaternion.identity).GetComponent<HexTile>();
                    hexTileObject.Initialize("Snow", x, z, 0, false , 0, 0);
                }
                _mapCoordinates.AddTile(hexTileObject);
            }
        }
        SpwanPlayer();
        SpwanMachine();
        UpdateNeighbors();
    }


    private void SpwanPlayer()
    {
        var randomTileNumber = Random.Range(0, _mapCoordinates.GetTilesLength());
        var hexTile = _mapCoordinates.GetTileBasedOnNumber(randomTileNumber);
        if (hexTile.GetIsWalkable())
        {   
            var pos = hexTile.transform.position;
            pos.y = 1;
            Instantiate(playerPrefab, pos, Quaternion.identity);
            playerOnTile = hexTile;
        }
        else
        {
            SpwanPlayer();
        }
    }
    
    private void SpwanMachine()
    {
        var randomTileNumber = Random.Range(1, _mapCoordinates.GetTilesLength());
        var hexTile = _mapCoordinates.GetTileBasedOnNumber(randomTileNumber);
        if (hexTile.GetIsWalkable() && hexTile != playerOnTile)
        {   
            var pos = hexTile.transform.position;
            pos.y = 0.5f;
            Instantiate(machinePrefab, pos, Quaternion.identity);
        }
        else
        {
            SpwanMachine();
        }
    }

    Vector3 CalculateHexPosition(int x, int z)
    {
        float xOffset = 0;
        if (z % 2 != 0) xOffset = hexWidth / 2f;

        float xPos = x * hexWidth + xOffset;
        float zPos = z * (hexHeight * 0.75f);
        return new Vector3(xPos, 0, zPos) + startPos;
    }

    private static readonly List<Vector2Int> HexNeighborOffsets = new List<Vector2Int>
    {
        new Vector2Int(1, 0),  // Right (East)
        new Vector2Int(-1, 0), // Left (West)
        new Vector2Int(0, 1),  // Top-right (Northeast)
        new Vector2Int(0, -1), // Bottom-left (Southwest)
        new Vector2Int(-1, 1), // Top-left (Northwest)
        new Vector2Int(1, -1)  // Bottom-right (Southeast)
    };

        private void UpdateNeighbors()
        {
            for (int i = 0; i < _mapCoordinates.GetTilesLength(); i++)
            {
                var hexTile = _mapCoordinates.GetTileBasedOnNumber(i);
                var x = hexTile.GetX();
                var z = hexTile.GetZ();
                var neighbors = new List<HexTile>();
                foreach (var offset in HexNeighborOffsets)
                {
                    var neighborX = x + offset.x;
                    var neighborZ = z + offset.y;
                    var neighbor = _mapCoordinates.GetTile(neighborX, neighborZ);
                    if (neighbor != null)
                    {
                        neighbors.Add(neighbor);
                    }
                }
                hexTile.SetNeighbours(neighbors);
            }
        }
}
