using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class levelManager : NetworkBehaviour
{

    //KEEP TRACK OF GAMEOBJECTS TO RESPAWN AND SUCH
    //If ARI is dead, TURN BULLETS INTO BLEH 

    public Vector3 offsetPos;
    public GameObject eBoss, player;
    public GameObject bg1, bg2;
    public GameObject gManager;
    public Boss_Mayumi mayumi;
    
    public soundManager sManager;


    public bool bossSpawn;

    #region BackgroundScrolling
    private enum backGroundState { Scrolling, Idle }
    private backGroundState bgState;
    private GameObject Object1;
    public Vector3 bgDirVel; //direction the background will scroll. Downard only atm
    public float bgSpeed; //speed the background will scroll.
    public float bgAccel; //Acceleration for the scroll speed.

    #endregion

    // Use this for initialization
    void Start () {
        sManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<soundManager>();

        Debug.Log("We got here Stage1 not set yet.");
        Debug.Log(sManager.currentSong);

        sManager.audioPlayer.Stop();
        sManager.songPlaying = false;
        sManager.currentSong = soundManager.songState.Stage1;
        //sManager.audioPlayer.Play();

        //Debug.Log("We got here Stage1 set.");
        //Debug.Log(sManager.currentSong);

        bgDirVel = new Vector3(0, -1, 0); // direction we're going to scroll
        bgSpeed = 5.0f; //speed of scroll
    }
	
	// Update is called once per frame
	void Update () {

        scrollBackground();

        // Random Coroutines for enemy spawning
        // int i changes randomly
        // StartCoroutine(spawnEnemyWave[i]());
        // At Certain waves a special wave of enemies appear
        // int j increments Afterwards
        // StartCoroutine(spawnSpEnemy[j]()) 
        // ^ will include Minibosses.
        // j increments Afterwards and resumes the previous enemy spawn loop
        // if a number of waves in i and J have looped then spawn boss

        if (isServer)
        {
            if (bossSpawn == false)
            {
                StartCoroutine(spawnBoss());
            }
        }

    }
    #region The Game's loop
    #endregion

    public void scrollBackground() {

        bg1.transform.position += (bgDirVel * bgSpeed * Time.deltaTime) + ((bgDirVel * bgAccel) * Mathf.Pow(Time.deltaTime, 2)) / 2;
        bg2.transform.position += (bgDirVel * bgSpeed * Time.deltaTime) + ((bgDirVel * bgAccel) * Mathf.Pow(Time.deltaTime, 2)) / 2;

        //determine if the center reached the bottom of the screen. Flip it's position above the other background.
        if (bg1.transform.position.y <= -12.6f + -10)
        {
            bg1.transform.position = new Vector3(0, bg2.transform.position.y + 24.5f, 0); 
        }
        else if (bg2.transform.position.y <= -12.6f + -10)
        {
            bg2.transform.position = new Vector3(0, bg1.transform.position.y + 24.5f, 0);
        }
        
    }

    IEnumerator spawnBoss()
    {
        bossSpawn = true;
        yield return new WaitForSeconds(5f);
        #region Mayumi

        offsetPos = new Vector3(0, 10, 0);

        Object1 = Instantiate(eBoss,
        offsetPos, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(Object1);
        mayumi = Object1.GetComponent<Boss_Mayumi>();
        Object1.transform.parent = GameObject.Find("World").transform;
        
        #endregion

    }
    

    public void respawnPlayer()
    {
        #region Respawn the Player 
        Object1 = Instantiate(player,
        offsetPos, Quaternion.identity) as GameObject;
        //mayumi = Object1.GetComponent<Boss_Mayumi>();
        Object1.transform.parent = GameObject.Find("World").transform;
        #endregion

    }

}
