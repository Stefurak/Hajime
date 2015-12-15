using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    Canvas canvas;
    Text gText;
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

        mayumi.isActiveAndEnabled.Equals(false);
        //Doesnt seem to be working right now
        GUI.Label(new Rect(10, 15, 200, 200), "JUST WRITE SOMETHING");
        
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.tag == "EnemyHP")
        {

            Debug.Log("its going in");
            displayHP = mayumi.health;
            OnGUI();
            //GUI.Label(new Rect(200, 15, 120, 120), "Enemy Health: " + displayHP.ToString());
        }
        else if (gameObject.tag == "PlayerHP")
        {
            Debug.Log("its going in");
            //displayHP = PlayerController.;
        }
    }

    void OnGUI()
    {
            Debug.Log("onGUI");
            if(gameObject.tag=="EnemyHP")
                canvas.GetComponent<Text>().text = "Enemy Health: " + displayHP.ToString();
            //GUI.Label(new Rect(650, 100, 120, 120), "Enemy Health: " + displayHP.ToString());
    }



}
