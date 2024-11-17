using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    // [SerializeField] private GameObject useMachineButton;
    // [SerializeField] private GameObject useWalkButton;
    // [SerializeField] private GameObject useEndTurnButton;
    // [SerializeField] private GameObject player;
    // [SerializeField] private GameObject machine;
    // private MapCoordinates _mapCoordinates;
    //
    // private IEnumerator Start()
    // {
    //     yield return new WaitUntil(() => MapCoordinates.GetInstance() != null);
    //     _mapCoordinates = MapCoordinates.GetInstance();
    // }
    //
    // private void Update()
    // {
    //     SetUseMachineActive(_mapCoordinates.GetClosestTile(player.transform.position), _mapCoordinates.GetClosestTile(machine.transform.position));
    // }
    //
    // private void SetUseMachineActive(HexTile PlayerTile, HexTile MachineTile)
    // {
    //     useMachineButton.SetActive(CheckIfPlayerIsNextToMachine(PlayerTile, MachineTile));
    // }
    //
    // private bool CheckIfPlayerIsNextToMachine(HexTile PlayerTile, HexTile MachineTile)
    // {
    //     
    //     if (PlayerTile.transform.position == MachineTile.transform.position)
    //     {
    //         //Debug.Log("Player is next to machine");
    //         return true;
    //     }
    //     {
    //         //Debug.Log("Player is next to machine");
    //         return true;
    //     }
    //     //Debug.Log("Player is not next to machine");
    //     return false;
    // }
}
