using UnityEngine;
using System.Collections;

public class loseState : IStateBase 
{

	private gameManager gManager;
    private soundManager sManager;

    public loseState() { }

    public loseState(gameManager _gManager, soundManager _sManager){
        gManager = _gManager;
        sManager = _sManager;
        Debug.Log("Constructing Lose State");
    }

    public void StateUpdate() {
        Debug.Log("LostState");
        if (Input.GetKeyUp(KeyCode.A))
        {
            gManager.SwitchState(new beginState(gManager, sManager));
        }
    }
}
