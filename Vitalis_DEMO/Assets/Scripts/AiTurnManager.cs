using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiTurnManager : MonoBehaviour
{
    [SerializeField] private GameObject DestroyedTilePrefab;
    private MapCoordinates _mapCoordinates;
    private List<HexTile> healtyTiles = new();
    private List<HexTile> destroyedTiles = new();
    private int destroyedTileCount = 0;
    private int healtyTileCount = 0;
    private int TotalTileCount = 0;
    private float worldHealth = 0.9f;
    private int amountOfTilesAllowedToDestroy = 50;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => MapCoordinates.GetInstance() != null);
        _mapCoordinates = MapCoordinates.GetInstance();
    }

    private void GetHealtyTiles()
    {
        List<HexTile> tiles = _mapCoordinates.GetTiles();
        healtyTiles.Clear(); // Lijst leegmaken voordat deze opnieuw gevuld wordt

        foreach (var tile in tiles)
        {
            if (tile.GetTileType() != "Water" && tile.GetTileType() != "Snow" && tile.GetTileType() != "Grass" 
                && tile.GetTileType() != "Flower" && tile.GetTileType() != "Forest")
            {
                continue;
            }
            healtyTiles.Add(tile);
        }
        healtyTileCount = healtyTiles.Count;
    }

    private void GetDestroyedTiles()
    {
        List<HexTile> tiles = _mapCoordinates.GetTiles();
        destroyedTiles.Clear(); // Lijst leegmaken voordat deze opnieuw gevuld wordt

        foreach (var tile in tiles)
        {
            if (tile.GetTileType() == "Destroyed")
            {
                destroyedTiles.Add(tile);
            }
        }
        destroyedTileCount = destroyedTiles.Count;
    }

    public void CheckTile()
    {
        TotalTileCount = _mapCoordinates.GetTilesLength();
        List<HexTile> tilesToDestroy = new();
        GetHealtyTiles();
        GetDestroyedTiles();
        int destroyedTile = 0;

        // Itereer door gezonde tegels en bepaal welke moeten worden vernietigd
        foreach (var tile in healtyTiles)
        {
            if (ShouldDestroyTile() && destroyedTile < amountOfTilesAllowedToDestroy)
            {
                tilesToDestroy.Add(tile);
                destroyedTile++;
            }
        }

        // Vernietig tegels buiten de oorspronkelijke lijst iteratie
        foreach (var tile in tilesToDestroy)
        {
            DestroyTile(tile);
        }

        UpdateWorldHealth();
        healtyTiles.Clear();
        destroyedTiles.Clear();
        healtyTileCount = 0;
        destroyedTileCount = 0;
    }

    private bool ShouldDestroyTile()
    {
        float destroyChance = Mathf.Lerp(0.1f, 0.9f, worldHealth);
        return Random.value < destroyChance;
    }

    private void DestroyTile(HexTile tile)
    {
        // Maak een nieuwe instantie van HexTile in plaats van een component toe te voegen
        HexTile destroyedTile = Instantiate(DestroyedTilePrefab, tile.transform.position, Quaternion.identity).GetComponent<HexTile>();
        destroyedTile.Initialize("Destroyed", tile.GetX(), tile.GetZ(), 0, false, 0, 0, tile);
        _mapCoordinates.ReplaceTile(tile, destroyedTile);
        tile.gameObject.SetActive(false);
    }

    public void UpdateWorldHealth()
    {
        if (destroyedTileCount == 0)
        {
            worldHealth = Mathf.Clamp01(1f);
            amountOfTilesAllowedToDestroy = 50;
        }
        else if (healtyTileCount == 0)
        {
            worldHealth = Mathf.Clamp01(0f);
            amountOfTilesAllowedToDestroy = 10;
        }
        else
        {
            worldHealth = (float)healtyTileCount / TotalTileCount;
            amountOfTilesAllowedToDestroy = Mathf.RoundToInt(50 * worldHealth);

            if (amountOfTilesAllowedToDestroy < 5)
            {
                amountOfTilesAllowedToDestroy = 5;
            }
        }
    }
}
