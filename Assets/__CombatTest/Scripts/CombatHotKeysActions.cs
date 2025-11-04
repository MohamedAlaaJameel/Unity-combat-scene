using System;
using UnityEngine;
using Zenject;
using static GameInstaller;

public class CombatHotKeysActions : MonoBehaviour
{

    [Inject] SignalBus _signalBus;
  

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _signalBus.Fire<PunshBTNSignal>();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right mouse donw");
            _signalBus.Fire<KickBTNSignal>();
        }
    }
}