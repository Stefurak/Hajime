using UnityEngine;
using System.Collections;

public class soundManager : MonoBehaviour {

    #region StateManager

    #region Audio Variables
    // audioManager Variables.
    public AudioSource audioPlayer;
    public enum songState
    {
        Title,
        Stage1, Boss1,
        Stage2, Boss2,
        Stage3, Boss3,
        Stage4, Boss4,
        Stage5, Boss5,
        Stage6, Boss6
    }

    public songState currentSong;
    public bool songPlaying;
    public AudioClip Title;
    public AudioClip Stage1;
    //public AudioClip Boss1;
    //public AudioClip Stage2;
    //public AudioClip Boss2;
    //public AudioClip Stage3;
    //public AudioClip Boss3;
    //public AudioClip Stage4;
    //public AudioClip Boss4;
    //public AudioClip Stage5;
    //public AudioClip Boss5;
    //public AudioClip Stage6;
    //public AudioClip Boss6;
    public AudioClip style1Bomb;
    public AudioClip style2Bomb;
    public AudioClip enemyDeath1;
    public AudioClip enemyDeath2;
    public AudioClip enemyBossHit;
    public AudioClip bulletFX;
    #endregion
    //public static gameManager instance;
    private IStateBase activeState;
    #endregion

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        //Set The state to start in here
    }

    void Update()
    {
        
        //if (activeState != null)
        //{
            
            //call the active states update
            #region Song Enumeration
            if (!songPlaying)
            {
            switch (currentSong)
                {
                    case songState.Title:
                        audioPlayer.PlayOneShot(Title, 0.5f);
                        songPlaying = true;
                        break;
                    case songState.Stage1:
                        audioPlayer.PlayOneShot(Stage1, 0.3f);
                        
                        songPlaying = true;
                        break;
                    case songState.Boss1:
                        break;
                    case songState.Stage2:
                        break;
                    case songState.Boss2:
                        break;
                    case songState.Stage3:
                        break;
                    case songState.Boss3:
                        break;
                    case songState.Stage4:
                        break;
                    case songState.Boss4:
                        break;
                    case songState.Stage5:
                        break;
                    case songState.Boss5:
                        break;
                    case songState.Stage6:
                        break;
                    case songState.Boss6:
                        break;
                }
            }
            #endregion

           //activeState.StateUpdate();
        //}
    }

}