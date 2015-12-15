using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerBullet : NetworkBehaviour
{

    #region Fields
    private enum bulletState { Normal, Special }
    private bulletState pBulletState;

    private Animator pBullet;
    public Vector3 dirVel;
    public float bulletSpeed;
    public float bulletAccel;
    #endregion

    void Awake() {
        //This is here because otherwise this overrides the Instantiated changes.
        dirVel = new Vector3(0, 1, 0);
    }

	// Use this for initialization
	void Start () {
        pBullet = this.GetComponent<Animator>();
        // 0 is Ari bullets, 1 Kouhei, 2 Isaac, 3 Hikari, 4 Oliver, 5 Winter 
        // pBullet.SetInteger("Color", 2);
        bulletSpeed = 25f;
	}
	
	// Update is called once per frame
	void Update () {

        // P = vit + (at^2)/2
        transform.position += (dirVel * bulletSpeed * Time.deltaTime) + ((dirVel * bulletAccel) * Mathf.Pow(Time.deltaTime, 2)) / 2;

        #region Collisions
        //Border Collisions Right and left.
        if (transform.position.x >= 10.9f || transform.position.x <= -13.0f || transform.position.y >= 12.6f || transform.position.y <= -12.6f)
        {
            NetworkServer.Destroy(transform.gameObject);
        }
        #endregion
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Enemy")
        {        
            c.gameObject.GetComponent<Boss_Mayumi>().decreaseHealth();
            NetworkServer.Destroy(transform.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D c)
    {
        //Might need this later
    }
}
