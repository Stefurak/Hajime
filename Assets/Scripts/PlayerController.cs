using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    #region Fields
    //Will change Fire Bullet Method
    private enum powerState { powerOne, powerTwo, powerMax }
    private enum styleState { styleOne, styleTwo }

    private powerState playerPower;
    private styleState playerStyle;

    private gameManager gManager;
    private soundManager sManager;
    private Animator anim;
    private SpriteRenderer hitBoxRend;
    public GameObject bullet;
    public GameObject hitBox;
    public PlayerBullet bulletScript;

    // Speed is how fast Character moves.
    [HideInInspector]
    public float speed;
    public float angle;
    public float horizontal;
    public float vertical;
    private float xMove;
    private float yMove;

    private Vector3 offsetPos;

    private bool fired;
    private bool focusing;
    #endregion

    // Use this for initialization
    void Start()
    {
        playerPower = powerState.powerOne;
        playerStyle = styleState.styleOne;

        speed = 8;
        angle = 0;
        anim = this.GetComponent<Animator>();

        offsetPos = new Vector3(0, 0, 0); 

        hitBox = GameObject.Find("Ari_hitbox");
        hitBoxRend = hitBox.GetComponent<SpriteRenderer>();

        gManager = GameObject.Find("GameManager").GetComponent<gameManager>();
        sManager = GameObject.Find("GameManager").GetComponent<soundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            InputManager();
        }
        BorderCollisionCheck();
        //Movement
        transform.Translate(xMove, yMove, 0);
    }
     
    void BorderCollisionCheck()
    {
        #region X-Axis Boundary
        // Border Collisions Left and Right.
        if (transform.position.x >= 9.8f)
        {
            xMove = 0;
            transform.position = new Vector3(9.8f, transform.position.y, transform.position.z);
            if (horizontal < 0)
            {
                xMove = (horizontal * speed) * Time.deltaTime;
            }
        }
        else if (transform.position.x <= -9.8f)
        {
            xMove = 0;
            transform.position = new Vector3(-9.8f, transform.position.y, transform.position.z);
            if (horizontal > 0)
            {
                xMove = (horizontal * speed) * Time.deltaTime;
            }
        }
        else
        {
            xMove = (horizontal * speed) * Time.deltaTime;
        }
        #endregion

        #region Y-Axis Boundary
        // Border Collisions Top and Bottom.
        if (transform.position.y >= 9.6f)
        {
            yMove = 0;
            transform.position = new Vector3(transform.position.x, 9.6f, transform.position.z);
            if (vertical < 0)
            {
                yMove = (vertical * speed) * Time.deltaTime;
            }
        }
        else if (transform.position.y <= -9.6f)
        {
            yMove = 0;
            transform.position = new Vector3(transform.position.x, -9.6f, transform.position.z);
            if (vertical > 0)
            {
                yMove = (vertical * speed) * Time.deltaTime;
            }
        }
        else
        {
            yMove = (vertical * speed) * Time.deltaTime;
        }
        #endregion
    }

    void InputManager()
    {
        //Controller
        vertical = Input.GetAxis("YAxis");
        horizontal = Input.GetAxis("XAxis");
        #region Animation Direction
        // Which animation playes based off Left and Right movement
        if (horizontal > 0.5f)
        {
            anim.SetInteger("Direction", 1);
        }
        else if (horizontal < -0.5f)
        {
            anim.SetInteger("Direction", -1);
        }
        else
        {
            anim.SetInteger("Direction", 0);
        }
        #endregion

        #region Shoot
        if (Input.GetButton("Shoot") || Input.GetKey(KeyCode.Z))
        {
           
            // If Not fired yet, fire bullets based of Enum
            if (!fired)
            {
                sManager.audioPlayer.PlayOneShot(sManager.bulletFX, 0.2f);
                switch (playerStyle)
                {
                    case styleState.styleOne:
                        CmdfireBulletStyleOne();
                        break;
                    case styleState.styleTwo:
                        fireBulletStyleTwo();
                        break;
                }
            }
        }
        #endregion

        #region Focus
        // Slow Speed and show hitbox for precision
        if (Input.GetButtonDown("Focus") || Input.GetKeyDown(KeyCode.LeftShift))
        {
            hitBoxRend.enabled = !hitBoxRend.enabled;
            speed /= 2;
            focusing = true;
        }
        if (Input.GetButtonUp("Focus") || Input.GetKeyUp(KeyCode.LeftShift))
        {
            hitBoxRend.enabled = !hitBoxRend.enabled;
            speed *= 2;
            focusing = false;
        }
        #endregion

        #region Pause
        // For Pause Menu State
        if (Input.GetButtonUp("Pause"))
        {
            //Change to pause state
        }
        #endregion
    }

    [Command]
    void CmdfireBulletStyleOne() {

        switch (playerPower)
        {
            case powerState.powerOne:
                styleOnePowerOne();
                break;
            case powerState.powerTwo:
                styleOnePowerTwo();
                break;
            case powerState.powerMax:
                styleOnePowerMax();
                break;
        }
    }

    void fireBulletStyleTwo() {
        switch (playerPower)
        {
            case powerState.powerOne:
                styleTwoPowerOne();
                break;
            case powerState.powerTwo:
                styleTwoPowerTwo();
                break;
            case powerState.powerMax:
                styleTwoPowerMax();
                break;
        }
    }

    #region Stlye One Power Pattern
    void styleOnePowerOne() {
        
        //New Vector sets The Bullets position on creation, Quaternion changes it's direction Velocity based off it's angle.
        #region Bullet 1
        GameObject Object1 = Instantiate(bullet,
            transform.position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(Object1);
        #endregion

        #region Bullet 2
        if (focusing)
        {
            angle = -1;
            offsetPos = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }
        else
        {
            angle = -2;
            offsetPos = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
        }

        GameObject Object2 = Instantiate(bullet,
            offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        NetworkServer.Spawn(Object2);
        bulletScript = Object2.GetComponent<PlayerBullet>();
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object2.GetComponent<PlayerBullet>().dirVel;
        #endregion

        #region Bullet 3
        if (focusing)
        {
            angle = 1;
            offsetPos = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        }
        else
        {
            angle = 2;
            offsetPos = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
        }
        GameObject Object3 = Instantiate(bullet,
            offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        NetworkServer.Spawn(Object3);
        bulletScript = Object3.GetComponent<PlayerBullet>();
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object3.GetComponent<PlayerBullet>().dirVel;
        #endregion

        #region GameObject Parent + Name
        //Set the Name of the bullet
        Object1.name = "Bullet Straight";
        Object2.name = "Bullet Right";
        Object3.name = "Bullet Left";

        //Set bullet's parent to the World object
        Object1.transform.parent = transform.parent;
        Object2.transform.parent = transform.parent;
        Object3.transform.parent = transform.parent;
        #endregion

        //Call a Coroutine to make a Firing Cooldown, Sets Fire to true for a couple seconds.
        StartCoroutine(bulletTimer());

    }
    void styleOnePowerTwo() {
        styleOnePowerOne();
        //More bullet code for Power level 2
        #region Bullet 4
        if (focusing)
        {
            angle = -2;
            offsetPos = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }
        else
        {
            angle = -3;
            offsetPos = new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z);
        }

        GameObject Object4 = Instantiate(bullet,
            offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        bulletScript = Object4.GetComponent<PlayerBullet>();
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object4.GetComponent<PlayerBullet>().dirVel;
        #endregion

        #region Bullet 5
        if (focusing)
        {
            angle = 2;
            offsetPos = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        }
        else
        {
            angle = 3;
            offsetPos = new Vector3(transform.position.x - 0.3f, transform.position.y, transform.position.z);
        }

        GameObject Object5 = Instantiate(bullet,
            offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        bulletScript = Object5.GetComponent<PlayerBullet>();
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object5.GetComponent<PlayerBullet>().dirVel;
        #endregion

        #region ButterFlyBullet 1
        #endregion

        #region ButterFlyBullet 2
        #endregion

        #region GameObject Parent + Name
        //Set the Name of the bullet
        Object4.name = "Bullet Right2";
        Object5.name = "Bullet Left2";
        //ObjectB1.name = "BBullet Right";
        //ObjectB2.name = "BBullet Left";
        

        //Set bullet's parent to the World object
        Object4.transform.parent = transform.parent;
        Object5.transform.parent = transform.parent;
        //ObjectB1.transform.parent = transform.parent;
        //ObjectB2.transform.parent = transform.parent;
        #endregion
    }
    void styleOnePowerMax() {
        styleOnePowerTwo();
        //More bullet for max Power
    }
    #endregion

    #region Stlye Two Power Pattern
    void styleTwoPowerOne() {
        //New Vector sets The Bullets position on creation, Quaternion changes it's direction Velocity based off it's angle.
        GameObject Object1 = Instantiate(bullet,
            transform.position, Quaternion.identity) as GameObject;
        if (focusing)
        {
            angle = -1;
            offsetPos = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }
        else
        {
            angle = -3;
            offsetPos = new Vector3(transform.position.x + 0.3f, transform.position.y, transform.position.z);
        }
        GameObject Object2 = Instantiate(bullet,
            offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        bulletScript = Object2.GetComponent<PlayerBullet>();
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object2.GetComponent<PlayerBullet>().dirVel;

        if (focusing)
        {
            angle = 1;
            offsetPos = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        }
        else
        {
            angle = 3;
            offsetPos = new Vector3(transform.position.x - 0.3f, transform.position.y, transform.position.z);
        }
        GameObject Object3 = Instantiate(bullet,
            offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        bulletScript = Object3.GetComponent<PlayerBullet>();
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object3.GetComponent<PlayerBullet>().dirVel;


        //Set the Name of the bullet
        Object1.name = "Bullet Straight";
        Object2.name = "Bullet Right";
        Object3.name = "Bullet Left";

        //Set bullet's parent to the World object
        Object1.transform.parent = transform.parent;
        Object2.transform.parent = transform.parent;
        Object3.transform.parent = transform.parent;


    }
    void styleTwoPowerTwo() {
        styleTwoPowerOne();
        //More bullet code for Power level 2
    }
    void styleTwoPowerMax() {
        styleTwoPowerTwo();
        //More bullet code for Power level Max
    }
    #endregion

    IEnumerator bulletTimer()
    {
        fired = true;
        yield return new WaitForSeconds(0.07f);
        fired = false;
    }

    public void playDeathSound() 
    {
        sManager.audioPlayer.PlayOneShot(sManager.enemyDeath1);
    }

}