using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private Material glowMaterial;
    [SerializeField] private LineRenderer pathLineRenderer; // Reference to LineRenderer
    private Material originalMaterial;
    private MapCoordinates _mapCoordinates;
    private GameObject player;
    private TurnManager _turnManager;
    private bool _pressedMoveButton;

    private void Start()
    {
        _turnManager = TurnManager.GetInstance();
        _mapCoordinates = MapCoordinates.GetInstance();
        player = GameObject.FindWithTag("Player");
        pathLineRenderer.positionCount = 0; // Initialize with no line
    }

    private void OnMouseEnter()
    {
        if (_pressedMoveButton) return;
        ChangeTileMaterial();
        PreviewPath(); // Preview the path when hovering over a tile
    }

    private void OnMouseExit()
    {
        ResetTileMaterial();
        ClearPath(); // Clear the path preview when exiting a tile
    }

    private void OnMouseDown()
    {
        if (_turnManager.GetUsedMove() || _pressedMoveButton) return;
        _pressedMoveButton = true;

        var selectedTile = _mapCoordinates.GetTile(transform.position);
        var playerTile = _mapCoordinates.GetClosestTile(player.transform.position);

        if (playerTile.transform.position != selectedTile.transform.position)
        {
            var path = PathFinding.GetInstance().FindPath(playerTile, selectedTile);
            if (path == null)
            {
                Debug.Log("No valid path found between the tiles!");
                return;
            }

            // Start moving the player along the path
            StartCoroutine(MovePlayerAlongPath(path));
            _pressedMoveButton = false;
        }
    }

    private void ChangeTileMaterial()
    {
        originalMaterial = GetComponent<Renderer>().material;
        GetComponent<Renderer>().material = glowMaterial;
    }

    private void ResetTileMaterial()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }

    // Preview the path when hovering over a tile
    private void PreviewPath()
    {
        var selectedTile = _mapCoordinates.GetTile(transform.position);
        var playerTile = _mapCoordinates.GetClosestTile(player.transform.position);

        // If the hovered tile is different from the player's current tile
        if (playerTile.transform.position != selectedTile.transform.position)
        {
            var path = PathFinding.GetInstance().FindPath(playerTile, selectedTile);
            if (path != null)
            {
                DisplayPath(path); // Display the path if it's valid
            }
        }
    }

    // Display the path using LineRenderer
    private void DisplayPath(List<HexTile> path)
    {
        pathLineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            pathLineRenderer.SetPosition(i, path[i].transform.position + Vector3.up * 0.1f); // Raise the line above tiles
        }
    }

    // Clear the path by resetting the LineRenderer
    private void ClearPath()
    {
        pathLineRenderer.positionCount = 0;
    }

    // Coroutine to move the player along the path
    private IEnumerator MovePlayerAlongPath(List<HexTile> path)
    {
        _turnManager.SetUsedMove(true);
        
        foreach (HexTile tile in path)
        {
            Vector3 targetPosition = tile.transform.position;
            targetPosition.y = player.transform.position.y;
            // Smoothly move the player to the next tile
            while (Vector3.Distance(player.transform.position, targetPosition) > 0.1f)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime * 5f); // Adjust speed as needed
                yield return null;
            }

            // Snap player to the target position at the end of movement
            player.transform.position = targetPosition;

            // Optionally, add a small delay before moving to the next tile
            yield return new WaitForSeconds(0.1f);
        }
    }
}
