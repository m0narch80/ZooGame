using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Portal : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        Debug.Log("Player entered the portal");
    }
}
