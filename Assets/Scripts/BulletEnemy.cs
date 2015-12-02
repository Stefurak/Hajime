using UnityEngine;
using System.Collections;

public class BulletEnemy : MonoBehaviour
{
    #region Fields
    private Animator bullet;

    private PlayerController playerScript;
    private GameObject player;

    private Boss_Mayumi enemyScript;
    private GameObject enemy;

    public enum bulletState { Straight, RotWave, Target, Wave }
    public bulletState eBulletState;

    public Vector3 dirVel, dirRot, dirAccel, SinWave, perpVec;
    
    public Quaternion rotAngle;
    public Quaternion velQuat;
    public Vector3 pos;

    private Vector3 basePosition;

    public float speed, accel, amplitude, omega, angle, timeVal, counter, duration, reTargetTime;
    public bool called;
    #endregion

    void Awake() {

        bullet = this.GetComponent<Animator>();
        eBulletState = bulletState.Straight;
        called = false;

        #region Vectors
        //If Targeting Player Set to this.
        //dirVel = (player.transform.position - enemy.transform.position).normalized;
        //else Set to a velocity going down something else will rotate it.
        dirVel = new Vector3(0, -1, 0);
        dirRot = new Vector3(0, 0, 1);
        SinWave = new Vector3(0, 0, 0);
        #endregion

        #region Set Scripts (Player & Enemy)
        //get these for getting their positions
        if (GameObject.Find("Ari") != null)
        {
            player = GameObject.Find("Ari");
            playerScript = player.GetComponent<PlayerController>();
        }
        if (GameObject.Find("Mayumi") != null)
        {
            enemy = GameObject.Find("Mayumi");
            enemyScript = enemy.GetComponent<Boss_Mayumi>();
        }
        #endregion

        #region Floats & Direction Rotation Vector
        speed = 0f;
        accel = 0f;
        // duration is currently for how many times Retarget is called. Can be used for other counters as well
        duration = 5;
        // time it takes to retarget to player
        reTargetTime = 1f;

        amplitude = 1f;
        omega = 5f;

        angle = 90;
        // Rotate the direction velocity by the angle set above.
        dirRot = Quaternion.Euler(0, 0, angle) * dirVel;
        #endregion

        #region Coroutines
        //For certain patterns
        #endregion
    }

    // Use this for initialization
	void Start () {
      
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region BulletPatterns
        //Patterns that need to be called here.
        switch (eBulletState)
        {
            case bulletState.Straight:
                break;
            case bulletState.RotWave:
                RotatingSinWave();
                break;
            case bulletState.Target:
                if (!called)
                {
                    StartCoroutine(reTargetCall());
                    called = true;
                }
                break;
            case bulletState.Wave:
                SinWaveBullet();
                break;
            default:
                break;
        }
        //
        //
        //
        #endregion

        #region Physics & Math for Position
        // P = vit + (at^2)/2
        transform.position += ((dirVel + SinWave) * speed * Time.deltaTime) + ((dirVel * accel) * Mathf.Pow(Time.deltaTime, 2))/2;
        speed += accel * Time.deltaTime;
        #endregion

        #region Border Checks
        //Remove bullets if player is dead or offscreen
        if (transform.position.x >= 7.9f || 
            transform.position.x <= -10.0f || 
            transform.position.y >= 10f || 
            transform.position.y <= -10f ||
            player == null)
        {
            DestroyBullet();
        }
        #endregion
    }

    public void DestroyBullet() {
        Destroy(transform.gameObject);
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player") {
            //Invoke("respawnPlayer()", 4f);
            Destroy(c.transform.parent.gameObject);
            
        }
    }
    void OnTriggerExit2D(Collider2D c)
    {
        //Might need this later
    }

    #region Pattern Methods
    public void SinWaveBullet()
    {
        //*~*~*~*Sin Wave Movement*~*~*~*\\
        //SinWaves Direction is always perpendicular to the Velocities Direction
        perpVec = new Vector3(-dirVel.y, dirVel.x, 0);
        timeVal += Time.deltaTime;
        SinWave = perpVec * amplitude * (Mathf.Sin(omega * timeVal));
    }
    public void RotatingSinWave(){
        //*~*~*~*Sin Wave Movement*~*~*~*\\
        //Rotates the Sinwave direction
        angle += 1f;
        dirRot = Quaternion.Euler(0, 0, angle) * dirVel;
        timeVal += Time.deltaTime;
        SinWave = dirRot * amplitude * (Mathf.Sin(omega * timeVal));   
    }
    IEnumerator reTargetCall()
    {
        while (true)
        {
            //Wait here for timer before calling retarget
            yield return new WaitForSeconds(reTargetTime);
            break;
        }
        //Duration is the amount of times it will Retarget
        //Counter is how many times it has Retargeted.
        if(counter <= duration)
        {
            ReTarget();
            counter++;
        }
    }
    public void ReTarget()
    {
        dirVel = (player.transform.position - transform.position).normalized;
        StartCoroutine(reTargetCall());
    }

    #endregion 
    /*
    #region Test Coroutine
    IEnumerator sinWaveCall()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            break;
        }

        SinWaveBullet();
        
        //counter++;
    }
    #endregion
    */

    /* //~*~*~* Sin Wave movement*~*~*~* //
        //Speed moving along a direction
        basePosition += dirVel * speed * Time.deltaTime;
        // Base + (The Vector of Oscillation) * Max peak of wave * (Frequency * time)
        Vector3 SinWave = dirRot * amplitude * Mathf.Sin(omega * Time.time);
        
        transform.position = basePosition + SinWave;
     */
}
