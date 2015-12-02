using UnityEngine;
using System.Collections;

// Things to do: 
// 1. Remove all bullets when health = 0
// 2. Respawn
// 3. Rotate transform.position for spawning so it fires in circle.
// 4. PARAMETRIC EQUATION BULLETS?!
// 5. Ari Butterfly Bullets (Animation | Creation).
// 6. Bomb (Clear Bullets, Kill enemies, hurt boss).

public class Boss_Mayumi : MonoBehaviour
{
    #region Fields
    //Each state will feature different movement and bullet shot's
    //Bosses 1-2 will feature up to 3 states. 
    //Bosses 3-4 will feature up to 4 states.
    //Bosses 5-6 Will feature up to 5-6 states.
    //Patterns are the same but add more and more as it progresses.
    //Spells are different each time never the same.
    private enum healthState { Pattern1, Spell1, Pattern2, Spell2, Pattern3, Spell3, Defeated }
    private healthState enemyHealth;

    public GameObject eBullet;
    private Animator anim;
    public BulletEnemy bulletScript;
    public Vector3 dirVel, offsetPos, startPos, resetPos, endingPos;
    // X is Start time Value. Y is Target time Value
    public Vector2 timeValue; 

    public float speed, xMove, health, angle, tBetweenFly, tBetweenFlyIdle, direction;

    public int moveCount;

    public bool flyRight, reseting, paraMoveReset, paraMovePattern;
    #endregion

    // Use this for initialization
	void Awake () {
        anim = this.GetComponent<Animator>();
        enemyHealth = healthState.Pattern1;
        health = 100;
        dirVel = new Vector3(1, 0, 0);
        //offsetPos = new Vector3(0, 0, 0);
        resetPos = new Vector3(-1.4f, 6, 0);
        flyRight = true;
        paraMoveReset = false;
        paraMovePattern = false;
        moveCount = 0;
        speed = 1f;
        tBetweenFly = 1.5f;
        tBetweenFlyIdle = 1.5f;

        //How many bullets will be fired before Coroutine Timer      
        SpawnFly();
	}
	
	// Update is called once per frame
	void Update ()
    {

        #region Update Movement
        //If Enemy is in reset position mode between Patterns
        if (paraMoveReset)
        {
            #region Reset Position
            timeValue.x += Time.deltaTime;
            // C = (1-t)A + tB
            if (timeValue.x >= timeValue.y)
            {
                timeValue.x = timeValue.y;
                reseting = false;
                paraMoveReset = false;
                SwitchPatterns();
            }
            transform.position = ((1 - timeValue.x) * startPos) + (timeValue.x * resetPos);
            #endregion
        } // Else if Enemy is in the pattern use this movement
        else if (paraMovePattern)
        {
            #region Pattern Position
            timeValue.x += (Time.deltaTime * speed);
            // C = (1-t)A + tB
            if (timeValue.x >= timeValue.y)
            {
                timeValue.x = timeValue.y;
                paraMovePattern = false;
                SwitchMovement();
            }
            transform.position = ((1 - timeValue.x) * startPos) + (timeValue.x * endingPos);
            #endregion
        }
        else
        {
            //transform.position += (dirVel * speed) * Time.deltaTime;
        }
        #endregion

        #region SwitchHealthStates
        //will be changed to an enum
        if(health < 1)
        {
            reseting = true;
            switch (enemyHealth)
            {
                case healthState.Pattern1:
                    enemyHealth = healthState.Spell1;
                    paraMovePattern = false;
                    moveCount = 0;
                    health = 100;
                    ResetMoveLine();
                    break;
                case healthState.Spell1:
                    enemyHealth = healthState.Pattern2;
                    paraMovePattern = false;
                    moveCount = 0;
                    health = 200;
                    ResetMoveLine();
                    break;
                case healthState.Pattern2:
                    enemyHealth = healthState.Spell2;
                    paraMovePattern = false;
                    moveCount = 0;
                    health = 200;
                    ResetMoveLine();
                    break;
                case healthState.Spell2:
                    enemyHealth = healthState.Pattern3;
                    paraMovePattern = false;
                    moveCount = 0;
                    health = 200;
                    ResetMoveLine();
                    break;
                case healthState.Pattern3:
                    enemyHealth = healthState.Spell3;
                    paraMovePattern = false;
                    moveCount = 0;
                    health = 300;
                    ResetMoveLine();
                    break;
                case healthState.Spell3:
                    enemyHealth = healthState.Defeated;
                    paraMovePattern = false;
                    moveCount = 0;
                    Destroy(transform.gameObject);
                    break;
            }
        }
        #endregion

    }
    public void decreaseHealth() {
        if (!reseting)
        {
            health -= 1;
            Debug.Log(health);
        }
    }
    public void SwitchPatterns()
    {
        switch (enemyHealth)
        {
            case healthState.Pattern1:
                MovePattern(0, 5, 0);
                StartCoroutine(startFiringPattern11(10, 1f, 0.5f));
                StartCoroutine(startFiringPattern12(5, 1f, 0.5f));
                break;
            case healthState.Spell1:
                MovePattern(0, 3, 90);
                StartCoroutine(startFiringSpell11(1, 1f, 0.5f));
                StartCoroutine(startFiringSpell12(1, 1f, 0.5f));
                break;
            case healthState.Pattern2:
                MovePattern(0, 5, 0);
                StartCoroutine(startFiringSpell11(2, 1f, 0.5f));
                StartCoroutine(startFiringSpell12(2, 1f, 0.5f));
                break;
            case healthState.Spell2:
                MovePattern(0, 5, 0);
                StartCoroutine(startFiringSpell11(1, 1f, 0.5f));
                StartCoroutine(startFiringSpell12(1, 1f, 0.5f));
                break;
            case healthState.Pattern3:
                MovePattern(0, 3, 90);
                StartCoroutine(startFiringSpell11(1, 1f, 0.5f));
                StartCoroutine(startFiringSpell12(1, 1f, 0.5f));
                break;
            case healthState.Spell3:
                MovePattern(0, 3, 270);
                StartCoroutine(startFiringSpell11(1, 1f, 0.5f));
                StartCoroutine(startFiringSpell12(1, 1f, 0.5f));
                StartCoroutine(startFiringSpell11(2, 3f, 1.5f));
                break;
            case healthState.Defeated:
                break;
        }
    }
    //Change Fly to Parametric Equation
    #region Fly
    public void SpawnFly()
    {
        reseting = true;
        paraMoveReset = true;
        timeValue = new Vector2(0, 1);
        startPos = transform.position;
        anim.SetInteger("Direction", 0);
    }
    public void ResetMoveLine()
    {
        paraMoveReset = true;
        timeValue = new Vector2(0,1);
        startPos = transform.position;
    }
    public void MovePattern(float startTime, float endTime, float angle)
    {
        //This allows the update to be called
        paraMovePattern = true;
        //Set time for the Time parameter of Parametric equation
        timeValue = new Vector2(startTime, endTime);
        //The Current Position of the Line
        startPos = transform.position;
        Debug.Log(moveCount);
        //The ending position of the Line
        //If Time is 1 Then the rotated endingPos is the exact ending position.
        //Setting the endTime will extend it past that point.
        endingPos = startPos + (Quaternion.Euler(0, 0, angle) * dirVel).normalized;

        #region Set Animation to Direction
        direction = endingPos.x - startPos.x;
        if (direction >= 0.5)
        {
            anim.SetInteger("Direction", 2);
        }
        else if (direction <= -0.5)
        {
            anim.SetInteger("Direction", 1);
        }
        else
        {
            anim.SetInteger("Direction", 0);
        }
        #endregion
    }
    public void SwitchMovement()
    {
        switch (enemyHealth)
        {
            #region Pattern 1 Movement
            case healthState.Pattern1:
                switch (moveCount)
                {
                    case 0:
                        MovePattern(0, 5, 180);
                        moveCount++;
                        break;
                    case 1:
                        MovePattern(0, 5, 180);
                        moveCount++;
                        break;
                    case 2:
                        MovePattern(0, 5, 0);
                        moveCount++;
                        break;
                    case 3:
                        MovePattern(0, 5, 0);
                        moveCount = 0;
                        break;
                }
                break;
            #endregion
            #region Spell 1 Movement
            case healthState.Spell1:
                switch (moveCount)
                {
                    case 0:
                        MovePattern(0, 2, 270);
                        speed = 1f;
                        moveCount++;
                        break;
                    case 1:
                        MovePattern(0, 2, 90);
                        moveCount++;
                        break;
                    case 2:
                        MovePattern(0, 4, 315);
                        speed = 1.5f;
                        moveCount++;
                        break;
                    case 3:
                        MovePattern(0, 5.6f, 180);
                        speed = 2f;
                        moveCount++;
                        break;
                    case 4:
                        MovePattern(0, 4, 45);
                        speed = 1.5f;
                        moveCount = 0;
                        break;
                }
                break;
            #endregion
            #region Pattern 2 Movement
            case healthState.Pattern2:
                switch (moveCount)
                {
                    case 0:
                        MovePattern(0, 5, 180);
                        moveCount++;
                        break;
                    case 1:
                        MovePattern(0, 5, 180);
                        moveCount++;
                        break;
                    case 2:
                        MovePattern(0, 5, 0);
                        moveCount++;
                        break;
                    case 3:
                        MovePattern(0, 5, 0);
                        moveCount = 0;
                        break;
                }
                break;
            #endregion
            #region Spell 2 Movement
            case healthState.Spell2:
                switch (moveCount)
                {
                    case 0:
                        MovePattern(0, 2, 270);
                        speed = 1f;
                        moveCount++;
                        break;
                    case 1:
                        MovePattern(0, 2, 90);
                        moveCount++;
                        break;
                    case 2:
                        MovePattern(0, 4, 315);
                        speed = 1.5f;
                        moveCount++;
                        break;
                    case 3:
                        MovePattern(0, 5.6f, 180);
                        speed = 2f;
                        moveCount++;
                        break;
                    case 4:
                        MovePattern(0, 4, 45);
                        speed = 1.5f;
                        moveCount = 0;
                        break;
                }
                break;
            #endregion
            #region Pattern 3 Movement
            case healthState.Pattern3:
                switch (moveCount)
                {
                    case 0:
                        MovePattern(0, 5, 180);
                        moveCount++;
                        break;
                    case 1:
                        MovePattern(0, 5, 180);
                        moveCount++;
                        break;
                    case 2:
                        MovePattern(0, 5, 0);
                        moveCount++;
                        break;
                    case 3:
                        MovePattern(0, 5, 0);
                        moveCount = 0;
                        break;
                }
                break;
            #endregion
            #region Spell 3 Movement
            case healthState.Spell3:
                switch (moveCount)
                {
                    case 0:
                        MovePattern(0, 2, 270);
                        speed = 1f;
                        moveCount++;
                        break;
                    case 1:
                        MovePattern(0, 2, 90);
                        moveCount++;
                        break;
                    case 2:
                        MovePattern(0, 4, 315);
                        speed = 1.5f;
                        moveCount++;
                        break;
                    case 3:
                        MovePattern(0, 5.6f, 180);
                        speed = 2f;
                        moveCount++;
                        break;
                    case 4:
                        MovePattern(0, 4, 45);
                        speed = 1.5f;
                        moveCount = 0;
                        break;
                }
                break;
            #endregion
        }
    }

    IEnumerator Idle()
    {  
        // Idle for WaitForSeconds
        dirVel = new Vector3(0, 0, 0);
        anim.SetInteger("Direction", 0);
        yield return new WaitForSeconds(tBetweenFlyIdle);
    }
    #endregion

    #region Pattern1

    #region Pattern1 Shot Type 1

    IEnumerator startFiringPattern11(int _totalShots,float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            //Make sure to check if healthState has changed if it has changed
            //STOP COROUTINE.
            if (enemyHealth == healthState.Pattern1)
            {
                yield return new WaitForSeconds(_tBetweenShot);
                FireBulletPattern11();
                shotCount++;
                if (shotCount >= _totalShots)
                {
                    break;
                }
            }
            else {
                yield break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdlePattern11(_tBetweenCluster));
    }
    IEnumerator shotIdlePattern11(float tBetweenCluster) 
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringPattern11(1, 1f, 0.5f));
    }
    #endregion

    #region Pattern1 Shot Type 2
    IEnumerator startFiringPattern12(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            if (enemyHealth == healthState.Pattern1)
            {
                yield return new WaitForSeconds(_tBetweenShot);
                FireBulletPattern12();
                shotCount++;
                if (shotCount >= _totalShots)
                {
                    break;
                }
            }
            else
            {
                yield break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdlePattern12(_tBetweenCluster));
    }
    IEnumerator shotIdlePattern12(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringPattern12(1, 1f, 0.5f));
    }
    #endregion

    #endregion
    #region Pattern2

    #region Pattern2 Shot Type1

    IEnumerator startFiringPattern21(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            if (enemyHealth == healthState.Pattern2)
            {
                yield return new WaitForSeconds(_tBetweenShot);
                FireBulletPattern21();
                shotCount++;
                if (shotCount >= _totalShots)
                {
                    break;
                }
            }
            else
            {
                yield break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdlePattern21(_tBetweenCluster));
    }
    IEnumerator shotIdlePattern21(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringPattern21(1, 1f, 0.5f));
    }
    #endregion

    #region Pattern2 Shot Type2
    IEnumerator startFiringPattern22(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;        
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            if (enemyHealth == healthState.Pattern2)
            {
                yield return new WaitForSeconds(_tBetweenShot);
                FireBulletPattern22();
                shotCount++;
                if (shotCount >= _totalShots)
                {
                    break;
                }
            }
            else
            {
                yield break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdlePattern22(_tBetweenCluster));
    }
    IEnumerator shotIdlePattern22(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringPattern22(1, 1f, 0.5f));
    }
    #endregion

    #endregion
    #region Pattern3

    #region Pattern3 Shot Type 1

    IEnumerator startFiringPattern31(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            if (enemyHealth == healthState.Pattern1) //change to 3 later
            {
                yield return new WaitForSeconds(_tBetweenShot);
                FireBulletPattern11();
                shotCount++;
                if (shotCount >= _totalShots)
                {
                    break;
                }
            }
            else
            {
                yield break;
            }
        }
        #endregion
        StartCoroutine(shotIdlePattern31(_tBetweenCluster));
    }
    IEnumerator shotIdlePattern31(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringPattern31(1, 1f, 0.5f));
    }
    #endregion

    #region Pattern3 Shot Type 2
    IEnumerator startFiringPattern32(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {

        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            yield return new WaitForSeconds(_tBetweenShot);
            FireBulletPattern32();
            shotCount++;
            if (shotCount >= _totalShots)
            {
                break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdlePattern32(_tBetweenCluster));
    }
    IEnumerator shotIdlePattern32(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringPattern12(1, 1f, 0.5f));
    }
    #endregion

    #endregion

    #region Spell1

    #region Spell1 Shot Type 1
    IEnumerator startFiringSpell11(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            
            if (enemyHealth == healthState.Spell1)
            {
                yield return new WaitForSeconds(_tBetweenShot);
                FireBulletSpell11();
                shotCount++;
                if (shotCount >= _totalShots)
                {
                    break;
                }
            }
            else
            {
                yield break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdleSpell11(_tBetweenCluster));
    }
    IEnumerator shotIdleSpell11(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringSpell11(1, 1f, 0.5f));
    }
    #endregion

    #region Spell1 Shot Type 2
    IEnumerator startFiringSpell12(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;    
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            if (enemyHealth == healthState.Spell1)
            {
                yield return new WaitForSeconds(_tBetweenShot);
                FireBulletSpell12();
                shotCount++;
                if (shotCount >= _totalShots)
                {
                    break;
                }
            }
            else
            {
                yield break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdleSpell12(_tBetweenCluster));
    }
    IEnumerator shotIdleSpell12(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringSpell12(1, 1f, 0.5f));
    }
    #endregion

    #endregion
    #region Spell2

    #region Spell2 Shot Type1

    IEnumerator startFiringSpell21(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            yield return new WaitForSeconds(_tBetweenShot);
            FireBulletSpell21();
            shotCount++;
            if (shotCount >= _totalShots)
            {
                break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdleSpell21(_tBetweenCluster));
    }
    IEnumerator shotIdleSpell21(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(1f);
        StartCoroutine(startFiringSpell21(1, 1f, 0.5f));
    }
    #endregion

    #region Spell2 Shot Type2
    IEnumerator startFiringSpell22(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            yield return new WaitForSeconds(_tBetweenShot);
            FireBulletSpell22();
            shotCount++;
            if (shotCount >= _totalShots)
            {
                break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdleSpell22(_tBetweenCluster));
    }
    IEnumerator shotIdleSpell22(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringSpell22(1, 1f, 0.5f));
    }
    #endregion

    #endregion
    #region Spell3

    #region Spell3 Shot Type 1

    IEnumerator startFiringSpell31(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            yield return new WaitForSeconds(_tBetweenShot);
            FireBulletSpell31();
            shotCount++;
            if (shotCount >= _totalShots)
            {
                break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdleSpell31(_tBetweenCluster));
    }
    IEnumerator shotIdleSpell31(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringSpell31(1, 1f, 0.5f));
    }
    #endregion

    #region Spell3 Shot Type 2
    IEnumerator startFiringSpell32(int _totalShots, float _tBetweenShot, float _tBetweenCluster)
    {
        int shotCount = 0;
        #region Enemy Bullet Timer
        //Fire bullets until shotCount is reached Then Cooldown
        while (true)
        {
            yield return new WaitForSeconds(_tBetweenShot);
            FireBulletSpell32();
            shotCount++;
            if (shotCount >= _totalShots)
            {
                break;
            }
            //yield return new WaitForSeconds(1f);
        }
        #endregion
        StartCoroutine(shotIdleSpell32(_tBetweenCluster));
    }
    IEnumerator shotIdleSpell32(float tBetweenCluster)
    {
        //wait for timer to stop before firing again
        yield return new WaitForSeconds(tBetweenCluster);
        StartCoroutine(startFiringSpell32(1, 1f, 0.5f));
    }
    #endregion

    #endregion


    // Bullets get Instantiated here.
    #region Pattern1
    public void FireBulletPattern11()
    {
        #region Bullet1
        angle = 0;
        offsetPos = transform.position;

        GameObject Object1 = Instantiate(eBullet,
        offsetPos, Quaternion.Euler(0,0,angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Target;
        bulletScript.speed = 1f;
        bulletScript.accel = 0.5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Set Variables

        

        #endregion
    }

    public void FireBulletPattern12() {

        #region Bullet1
        angle = 4;
        offsetPos = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);

        GameObject Object1 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Wave;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Bullet2
        angle = -4;
        offsetPos = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);

        GameObject Object2 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object2.transform.parent = transform.parent;
        bulletScript = Object2.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Wave;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object2.GetComponent<BulletEnemy>().dirVel;
        #endregion

    }
    #endregion 
    #region Pattern2
    public void FireBulletPattern21()
    {
        #region Bullet1
        angle = 0;
        offsetPos = transform.position;

        GameObject Object1 = Instantiate(eBullet,
        offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Target;
        bulletScript.accel = 2.5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Set Variables



        #endregion
    }

    public void FireBulletPattern22()
    {

        #region Bullet1
        angle = 4;
        offsetPos = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);

        GameObject Object1 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Straight;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Bullet2
        angle = -4;
        offsetPos = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);

        GameObject Object2 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object2.transform.parent = transform.parent;
        bulletScript = Object2.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Straight;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object2.GetComponent<BulletEnemy>().dirVel;
        #endregion

    }
    #endregion 
    #region Pattern3
    public void FireBulletPattern31()
    {
        #region Bullet1
        angle = 0;
        offsetPos = transform.position;

        GameObject Object1 = Instantiate(eBullet,
        offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Target;
        bulletScript.accel = 2.5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Set Variables



        #endregion
    }

    public void FireBulletPattern32()
    {

        #region Bullet1
        angle = 4;
        offsetPos = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);

        GameObject Object1 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Straight;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Bullet2
        angle = -4;
        offsetPos = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);

        GameObject Object2 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object2.transform.parent = transform.parent;
        bulletScript = Object2.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Straight;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object2.GetComponent<BulletEnemy>().dirVel;
        #endregion

    }
    #endregion 

    #region Spell1
    public void FireBulletSpell11()
    {
        #region Bullet1
        angle = 0;
        offsetPos = transform.position;

        GameObject Object1 = Instantiate(eBullet,
        offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Wave;
        bulletScript.amplitude = 0.8f;
        bulletScript.omega = 5;
        bulletScript.speed = 1f;
        bulletScript.accel = 0.5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Set Variables



        #endregion
    }

    public void FireBulletSpell12()
    {

        #region Bullet1
        angle = -10;
        offsetPos = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);

        GameObject Object1 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.RotWave;
        bulletScript.amplitude = 0.8f;
        bulletScript.omega = 5;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Bullet2
        angle = 10;
        offsetPos = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);

        GameObject Object2 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object2.transform.parent = transform.parent;
        bulletScript = Object2.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.RotWave;
        bulletScript.amplitude = 0.8f;
        bulletScript.omega = 5;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object2.GetComponent<BulletEnemy>().dirVel;
        #endregion

    }
    #endregion
    #region Spell2
    public void FireBulletSpell21()
    {
        #region Bullet1
        angle = 0;
        offsetPos = transform.position;

        GameObject Object1 = Instantiate(eBullet,
        offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Target;
        bulletScript.accel = 2.5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Set Variables



        #endregion
    }

    public void FireBulletSpell22()
    {

        #region Bullet1
        angle = 4;
        offsetPos = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);

        GameObject Object1 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Straight;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Bullet2
        angle = -4;
        offsetPos = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);

        GameObject Object2 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object2.transform.parent = transform.parent;
        bulletScript = Object2.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Straight;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object2.GetComponent<BulletEnemy>().dirVel;
        #endregion

    }
    #endregion
    #region Spell3
    public void FireBulletSpell31()
    {
        #region Bullet1
        angle = 0;
        offsetPos = transform.position;

        GameObject Object1 = Instantiate(eBullet,
        offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Target;
        bulletScript.accel = 2.5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Set Variables



        #endregion
    }

    public void FireBulletSpell32()
    {

        #region Bullet1
        angle = 4;
        offsetPos = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);

        GameObject Object1 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object1.transform.parent = transform.parent;
        bulletScript = Object1.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Straight;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object1.GetComponent<BulletEnemy>().dirVel;
        #endregion

        #region Bullet2
        angle = -4;
        offsetPos = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);

        GameObject Object2 = Instantiate(eBullet, offsetPos, Quaternion.Euler(0, 0, angle)) as GameObject;
        Object2.transform.parent = transform.parent;
        bulletScript = Object2.GetComponent<BulletEnemy>();
        bulletScript.eBulletState = BulletEnemy.bulletState.Straight;
        bulletScript.speed = 5f;
        bulletScript.dirVel = Quaternion.Euler(0, 0, angle) * Object2.GetComponent<BulletEnemy>().dirVel;
        #endregion

    }
    #endregion 
}