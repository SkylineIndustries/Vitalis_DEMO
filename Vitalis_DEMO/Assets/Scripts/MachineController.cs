using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private MapCoordinates _mapCoordinates;

    private void Start()
    {
        _mapCoordinates = MapCoordinates.GetInstance();
    }

    public void StartMachine()
    {
        _particleSystem.Play();
        HealTiles();
    }
    
    private void HealTiles()
    {
        List<HexTile> destroyedTiles = _mapCoordinates.GetDestroyedTiles();
        List<HexTile> tiles = _mapCoordinates.GetTiles();

        // Keep track of tiles to remove after iteration
        List<HexTile> tilesToRemove = new List<HexTile>();

        for (var i = 0; i < tiles.Count; i++)
        {
            var tile = tiles[i];

            // Find the corresponding destroyed tile
            var originalTile = destroyedTiles.Find(destroyedTile => destroyedTile.GetX() == tile.GetX() && destroyedTile.GetZ() == tile.GetZ());

            if (originalTile != null)
            {
                // Heal the tile by replacing the destroyed one with the original
                HealTile(tile, originalTile);
            
                // Mark the destroyed tile for removal
                tilesToRemove.Add(originalTile);
            }
        }

        // After all tiles have been healed, remove the destroyed tiles
        foreach (var tileToRemove in tilesToRemove)
        {
            _mapCoordinates.RemoveDestroyedTile(tileToRemove);
        }
    }

    private void HealTile(HexTile destroyedTile, HexTile originalTile)
    {
        originalTile.gameObject.SetActive(true);
        _mapCoordinates.ReplaceTile(destroyedTile, originalTile);
        DestroyImmediate(destroyedTile.gameObject); // Gebruik DestroyImmediate voor directe vernietiging
    }


}