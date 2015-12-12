using UnityEngine;
using System.Collections;

public class wonState : IStateBase 
{

	private gameManager gManager;
    private soundManager sManager;

    public wonState() { }

    public wonState(gameManager _gManager, soundManager _sManager){
        gManager = _gManager;
        sManager = _sManager;
        Debug.Log("Constructing Won State");
    }

    public void StateUpdate() {
        Debug.Log("WonState");
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //gManager.SwitchState(new loseState(gManager, sManager));
        }
    }
}
