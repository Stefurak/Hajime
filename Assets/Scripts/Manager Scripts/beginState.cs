using UnityEngine;
using System.Collections;

public class beginState : IStateBase
{
    #region Fields
    private gameManager gManager;
    private soundManager sManager;
    private int count;
    private float vertical;
    private float horizontal;
    private bool movedVertically;
    #endregion

    public beginState(gameManager _gManager, soundManager _sManager)
    {
        //This gets called when State is switched
        Debug.Log("Constructing Begin State");
        gManager = _gManager;
        sManager = _sManager;
        count = 0;
        //This is for Controller Axis since I can't get ButtonDown for Axis
        movedVertically = false;
        //Play the Title song
        //sManager.currentSong = soundManager.songState.Title;
    }

    public void StateUpdate() {
        //Debug.Log("beginState");
        menuInput();
        /*
         * Read up on this
        GUI.FocusControl
        */
    }
    private void menuInput()
    {
        #region Menu Selection Input
        //Get if up or down was pressed and change the count For GUI Selection
        // 0 Starts game, 1 "Options", 2 "Extras?", 3 Exit Game.
        vertical = Input.GetAxis("YAxis");
        horizontal = Input.GetAxis("XAxis");
        //If I wasn't pressed yet!
        if (!movedVertically)
        {
            //Check How much im pushing the JoystickUpward
            if (vertical > 0.5f)
            {
                //Increase Counter
                count++;
                Debug.Log(count);
                //If I'm at the bottom return to top.
                if (count > 3)
                {
                    count = 0;
                }
                //I MOVED, So dont come back until I return to 0
                movedVertically = true;
            } // Same thing below Just JoyStickDownward
            else if (vertical < -0.5f)
            {
                count--;
                Debug.Log(count);

                if (count < 0)
                {
                    count = 3;
                }
                movedVertically = true;
            }
        } // I am returning back to not moving Joystick
        else 
        {
            if (vertical < 0.1f && vertical > -0.1f)
            {
                movedVertically = false;
            }
        }
        #endregion

        //Don't erase this is for controller Input!
        #region Select Button code
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Bomb") || Input.GetButtonDown("Shoot") && count == 0)
        {
            gManager.SwitchState(new playState(gManager, sManager));
            Application.LoadLevel("GameScene");
            Debug.Log("Start Game");
            Debug.Log(count);
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Bomb") || Input.GetButtonDown("Shoot") && count == 1)
        {
            //gManager.SwitchState(new playState(gManager));
            Application.LoadLevel("GameScene");
            Debug.Log("Resume a save");
            Debug.Log(count);
        } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Bomb") || Input.GetButtonDown("Shoot") && count == 2)
        {
            //gManager.SwitchState(new playState(gManager));
            Application.LoadLevel("GameScene");
            Debug.Log("Options");
            Debug.Log(count);
        } else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Bomb") || Input.GetButtonDown("Shoot") && count == 3)
        {
            //gManager.SwitchState(new playState(gManager));
            Application.LoadLevel("GameScene");
            Debug.Log("Exit");
            Debug.Log(count);
        }
        #endregion

        #region Cancel Button Code
        //if (getButtonDown("Cancel")) {}
        #endregion
    }
}
