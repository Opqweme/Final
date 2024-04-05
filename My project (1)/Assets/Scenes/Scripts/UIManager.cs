using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Button playButton; 


    private void Start()
    {
        // Check if the game is already in play mode on start
        if (Global.InPlayMode)
        {
            // If the game is already in play mode, hide the play button
            playButton.gameObject.SetActive(false);
        }
    }


    public void PlayGame()
    {
        if(!Global.InPlayMode)
        {
            Global.InPlayMode = true;
            playButton.gameObject.SetActive(false);
        }
    }

    public void OnPlayerRespawn()
    {
        playButton.gameObject.SetActive(false);
    }

}
