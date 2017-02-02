using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector2 Speed;

    private Rigidbody2D rigibody;

	// Use this for initialization
	void Start () {

        rigibody = GetComponent<Rigidbody2D>();

        rigibody.velocity = new Vector2(Speed.x + GameObject.Find("Main Camera").GetComponent<CameraMovement>().Speed, Speed.y);
    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(this.gameObject);
    }
}
