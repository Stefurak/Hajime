using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public GameObject gameManager;

	// Use this for initialization
	void Start () {
        if (gameManager != null)
        {
            GameObject gm =  Instantiate(gameManager);
            gm.name = "GameManager";
        }
    }
	
}
