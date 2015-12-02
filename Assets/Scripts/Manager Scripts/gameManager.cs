using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour
{
    #region StateManager
    //public static gameManager instance;
    private IStateBase activeState;
    #endregion

    #region GUI Variables
    //GUI Variables
    public float game_width = 1024;
    public float game_height = 768;
    public float rx
    {
        get
        {
            return Screen.width / game_width;
        }
    }
    public float ry
    {
        get
        {
            return Screen.height / game_height;
        }
    }
    #endregion

    #region SpawnVariables *Move later*
    public Boss_Mayumi mayumi;
    public float displayHP;
    #endregion

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(transform.gameObject);
        //Set The state to start in here
        activeState = new beginState(this, this.GetComponent<soundManager>());
        Debug.Log("this object is of type " + activeState);
	}
	
	// Update is called once per frame
	void Update () {
        if (activeState != null)
        {
            activeState.StateUpdate();
        }
	}

    public void SwitchState(IStateBase newState) {
        //Switch States here
        activeState = newState;
    }

    void OnGUI()
    {
        #region GUI for each State
        // Can I use a switch here instead maybe? 
        // Set an Enum ause these if's on smaller code to change the State perhaps
        if (activeState.GetType() == typeof(beginState))
        {
           //GUI for main menu here 
        }
        else if (activeState.GetType() == typeof(playState))
        {
            // this code sets up the GUI outline for the Scores.
            #region Matrix Set up
            Matrix4x4 OrigMatrix = GUI.matrix;

            GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

            GUI.skin.label.fontSize = 25;
            #endregion

            #region Change this to Canvas
        
            GUI.Label(new Rect((game_width - 270), 0, 300, game_height),
                "High Score: 000000000");
            GUI.Label(new Rect((game_width - 270), 40, 300, game_height),
                "Score: 0");

            GUI.Label(new Rect((game_width - 240), 250, 300, game_height),
                "Lives: 0");
            GUI.Label(new Rect((game_width - 240), 300, 300, game_height),
                "Bombs: 0");

            if (mayumi != null)
            {
                GUI.Label(new Rect((game_width - 240), 500, 300, game_height),
                    "Enemy Health: " + mayumi.health);
            }
            #endregion

            //do I need this still? Still checking do not remove.
            GUI.matrix = OrigMatrix;
        }
        #endregion
    }



}
