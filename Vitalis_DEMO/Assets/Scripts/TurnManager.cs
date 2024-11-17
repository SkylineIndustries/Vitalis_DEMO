using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TurnManager : MonoBehaviour
{
    private char turn = 'X';
    [SerializeField] private AiTurnManager aiTurnManager;
    private bool usedMove;
    private bool usedMachine;

    public static TurnManager instance;
    // Start is called before the first frame update
    
    public static TurnManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<TurnManager>();

            if (instance != null) return instance;
            var newGameObject = new GameObject("TurnManager");
            instance = newGameObject.AddComponent<TurnManager>();
        }

        return instance;
    }
    
    void Start()
    {
        TurnManagerSwitch();
    }

    private void TurnManagerSwitch()
    {
        switch (turn)
        {
            case 'X':
                turn = 'P';
                break;
            case 'A':
                DoAITurn();
                break;
        }; 
    }
    
    private void DoAITurn()
    {
        if (Random.Range(0, 2) == 0)
        {
            turn = 'P';
        }
        aiTurnManager.CheckTile();
        turn = 'P';
    }
    
    public void SetUsedMachine()
    {
        if (usedMachine) return;
        usedMachine = true;
    }
    
    public void SetUsedMove(bool used)
    {
        usedMove = used;
    }
    
    public bool GetUsedMachine()
    {
        return usedMachine;
    }
    
    public bool GetUsedMove()
    {
        return usedMove;
    }
    
    public void CheckEndPlayerTurn()
    {
        if (turn != 'P') return;
        
        turn = 'A';
        SetUsedMove(false);
        usedMachine = false;
        TurnManagerSwitch();
    }
}
