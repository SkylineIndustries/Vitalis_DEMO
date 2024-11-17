using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public string tileType;
    public int x, z;
    public int movementCost;
    public bool isWalkable;
    
    private HexTile parent;
    private int gCost;
    private int hCost;
    private List<HexTile> neighbours = new List<HexTile>();
    private HexTile orginalTile;

    // Initialize method
    public void Initialize(string tileType, int x, int z, int movementCost, bool isWalkable, int gCost, int hCost, HexTile orginalTile = null)
    {
        this.tileType = tileType;
        this.x = x;
        this.z = z;
        this.movementCost = movementCost;
        this.isWalkable = isWalkable;
        this.gCost = gCost;
        this.hCost = hCost;
        this.orginalTile = orginalTile;
    }

    // Getters and Setters
    public string GetTileType() { return tileType; }
    public int GetX() { return x; }
    public int GetZ() { return z; }
    public int GetMovementCost() { return movementCost; }
    public bool GetIsWalkable() { return isWalkable; }
    public int GetGCost() { return gCost; }
    public void SetGCost(int value) { gCost = value; }
    public int GetHCost() { return hCost; }
    public void SetHCost(int value) { hCost = value; }
    public int GetFCost() { return gCost + hCost; }

    public HexTile GetParent() { return parent; }
    public void SetParent(HexTile value) { parent = value; }
    
    public List<HexTile> GetNeighbours() { return neighbours; }
    public void SetNeighbours(List<HexTile> value) { neighbours = value; }
    
    public HexTile GetOrginalTile() { return orginalTile; }
    
    public void SetOrginalTile(HexTile value) { orginalTile = value; }
}