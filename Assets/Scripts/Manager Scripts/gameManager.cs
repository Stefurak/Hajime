using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour
{
    #region StateManager
    //public static gameManager instance;
    private IStateBase activeState;
    #endregion

    #region Manager
    private soundManager sManager;
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

        sManager = this.GetComponent<soundManager>();

        sManager.songPlaying = false;
        sManager.currentSong = soundManager.songState.Title;

        //Set The state to start in here
        //activeState = new beginState(this, this.GetComponent<soundManager>());
        //Debug.Log("this object is of type " + activeState);
    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI()
    {
   
    }



}
