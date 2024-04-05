using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gamemodes { Cube = 0, Ship = 1 };
public enum Gravity { Upright = 1, Upsidedown = -1 }; // Adjusted enum values
public enum Speeds { Normal = 0, Slow = 1 }; // Adjusted enum values

public class PortalScript : MonoBehaviour
{
    public Gamemodes Gamemode;
    public Speeds Speed;
    public Gravity gravity;
    public int State;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.ChangeThroughPortal(Gamemode, Speed, gravity, State);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error handling portal trigger: " + e.Message);
        }
    }
}
