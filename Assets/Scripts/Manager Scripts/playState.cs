using UnityEngine;
using System.Collections;

public class playState : IStateBase
{

    #region Fields
    private gameManager gManager;
    private soundManager sManager;
    private levelManager lvlManager;
    private int count;
    private float vertical;
    private float horizontal;
    private bool movedVertically;
    #endregion

    public playState() { }

    public playState(gameManager _gManager, soundManager _sManager)
    {
        gManager = _gManager;
        sManager = _sManager;
        lvlManager = gManager.GetComponent<levelManager>();

        //set up the Song for stage 1
        sManager.audioPlayer.Stop();
        sManager.songPlaying = false;
        sManager.currentSong = soundManager.songState.Stage1;
        
        //sManager.audioPlayer.Play();

        Debug.Log("Constructing Play State");
    }

    public void StateUpdate() {

        menuInput();
        //levelManager();
        // create an Enum that sets which stage we are on, or if we are on a boss.
        // The entire game should be in this one Scene.
        // Enum should spawn enemies, and the boss when Enum calls for it.
    }

    void menuInput()
    {

        #region Pause Code Button
        if (Input.GetButtonDown("Pause") || Input.GetKeyUp(KeyCode.Escape))
        {
            //Leave this here for pause stuff
            //gManager.SwitchState(new wonState(gManager));
            Debug.Log("I will pause someday");
            Application.Quit();
        }
        #endregion
    }

}
