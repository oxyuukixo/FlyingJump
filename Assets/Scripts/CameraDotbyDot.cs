using UnityEngine;
using System.Collections;

public class CameraDotbyDot : MonoBehaviour {

    void Awake()
    {
        Camera camera = GetComponent<Camera>();

        camera.orthographicSize = Screen.height / 2;
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
