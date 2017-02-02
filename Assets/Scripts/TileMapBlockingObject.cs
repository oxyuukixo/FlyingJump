using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapBlockingObject : MonoBehaviour {

    public GameObject BlockingObject;

    public GameObject BackGroundObject;

    public float BlockHeight;

	// Use this for initialization
	void Start () {

        float Width = 0;

        foreach (Transform child in transform)
        {
            if (Width < child.GetComponent<Map>().m_NumX)
            {
                Width = child.GetComponent<Map>().m_NumX;
            }
        }

        float Height = BackGroundObject.GetComponent<SpriteRenderer>().bounds.size.y;

        GameObject UpBlock = Instantiate(BlockingObject);
        GameObject DownBlock = Instantiate(BlockingObject);

        UpBlock.transform.parent = transform;
        DownBlock.transform.parent = transform;

        UpBlock.GetComponent<BoxCollider2D>().size = new Vector2(Width, BlockHeight);
        UpBlock.GetComponent<BoxCollider2D>().transform.position = new Vector2(transform.position.x + Width / 2, transform.position.y + BlockHeight / 2);

        DownBlock.GetComponent<BoxCollider2D>().size = new Vector2(Width, BlockHeight);
        DownBlock.GetComponent<BoxCollider2D>().transform.position = new Vector2(transform.position.x + Width / 2, transform.position.y - (Height + BlockHeight / 2));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
