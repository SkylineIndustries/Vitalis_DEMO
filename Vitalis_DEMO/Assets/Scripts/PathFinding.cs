using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private static PathFinding instance;

    public static PathFinding GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PathFinding>();

            if (instance == null)
            {
                GameObject newGameObject = new GameObject("PathFinding");
                instance = newGameObject.AddComponent<PathFinding>();
            }
        }

        return instance;
    }
    
    public List<HexTile> FindPath(HexTile startTile, HexTile targetTile)
    {
        List<HexTile> openSet = new List<HexTile>();
        HashSet<HexTile> closedSet = new HashSet<HexTile>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            HexTile currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].GetFCost() < currentTile.GetFCost() || openSet[i].GetFCost() == currentTile.GetFCost() && openSet[i].GetHCost() < currentTile.GetHCost())
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == targetTile)
            {
                return RetracePath(startTile, targetTile);
            }

            foreach (HexTile neighbour in currentTile.GetNeighbours())
            {
                if (!neighbour.GetIsWalkable() || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentTile.GetGCost() + GetDistance(currentTile, neighbour);
                if (newMovementCostToNeighbour < neighbour.GetGCost() || !openSet.Contains(neighbour))
                {
                    neighbour.SetGCost(newMovementCostToNeighbour);
                    neighbour.SetHCost(GetDistance(neighbour, targetTile));
                    neighbour.SetParent(currentTile);

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    public int GetDistance(HexTile tileA, HexTile tileB)
    {
        int dstX = Mathf.Abs(tileA.GetX() - tileB.GetX());
        int dstZ = Mathf.Abs(tileA.GetZ() - tileB.GetZ());

        if (dstX > dstZ)
            return 14 * dstZ + 10 * (dstX - dstZ);
        return 14 * dstX + 10 * (dstZ - dstX);
    }

    public List<HexTile> RetracePath(HexTile startTile, HexTile endTile)
    {
        List<HexTile> path = new List<HexTile>();
        HexTile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.GetParent();
        }
        path.Reverse();
        return path;
    }
}
