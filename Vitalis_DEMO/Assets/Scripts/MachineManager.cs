using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{

    private MachineController _machine;
    private TurnManager _turnManager;
    
    private void Start()
    {
        _turnManager = TurnManager.GetInstance();
    }
    public void StartMachine()
    {
        _machine = GameObject.FindWithTag("Machine").GetComponent<MachineController>();

        if (_turnManager.GetUsedMachine()) return;
        _machine.StartMachine();
    }
}
