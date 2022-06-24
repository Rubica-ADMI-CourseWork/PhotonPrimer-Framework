using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains info of which player fired this shell
/// to control for the scenario where multiple players are shooting
/// at the tank, each time a shell strikes the shooter name is updated on teh 
/// target player.
/// </summary>
public class ShellInfo : MonoBehaviour
{
    public string ShooterName { get; set; }
    public int playerNo { get; set; }

}
