        using System.Collections.Generic;
        using UnityEngine;

        public class MapCoordinates : MonoBehaviour
        {
            private static MapCoordinates instance;
            List<HexTile> tiles = new();
            List<HexTile> destroyedTiles = new();

            public static MapCoordinates GetInstance()
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MapCoordinates>();

                    if (instance == null)
                    {
                        GameObject newGameObject = new GameObject("MapCoordinates");
                        instance = newGameObject.AddComponent<MapCoordinates>();
                    }
                }

                return instance;
            }
            
            public void AddTile(HexTile tile)
            {
                tiles.Add(tile);
            }
            
            public void RemoveTile(HexTile tile)
            {
                tiles.Remove(tile);
            }
            
            public HexTile GetTile(Vector3 position)
            {
                foreach (var tile in tiles)
                {
                    if (tile.transform.position == position)
                    {
                        return tile;
                    }
                }
                return null;
            }
            
            public HexTile GetTile(int x, int z)
            {
                foreach (var tile in tiles)
                {
                    if (tile.GetX() == x && tile.GetZ() == z)
                    {
                        return tile;
                    }
                }
                return null;
            }
            
            public int GetTilesLength()
            {
                return tiles.Count;
            }
            
            public HexTile GetTileBasedOnNumber(int number)
            {
                return tiles[number];
            }
            
            public void UpdateTile(HexTile tile)
            {
                for (var i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i].transform.position == tile.transform.position)
                    {
                        tiles[i] = tile;
                    }
                }
            }
            
            public List<HexTile> GetTiles()
            {
                return tiles;
            }
            
            public HexTile GetClosestTile(Vector3 position)
            {
                HexTile closestTile = null;
                var closestDistanceSqr = Mathf.Infinity;

                foreach (var tile in tiles)
                {
                    Vector3 directionToTarget = tile.transform.position - position;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        closestTile = tile;
                    }
                }
                return closestTile;
            }
            
            public void ReplaceTile(HexTile oldTile, HexTile newTile)
            {
                for (var i = 0; i < tiles.Count; i++)
                {
                    if (tiles[i].transform.position == oldTile.transform.position)
                    {
                        tiles[i] = newTile;
                        if (oldTile.GetTileType() != "Destroyed")
                        {
                            destroyedTiles.Add(oldTile);
                        }
                        oldTile.gameObject.SetActive(false);
                    }
                }
            }

            public List<HexTile> GetDestroyedTiles()
            {
                return destroyedTiles;
            }
            
            public void RemoveDestroyedTile(HexTile tile)
            {
                destroyedTiles.Remove(tile);
            }
        }
