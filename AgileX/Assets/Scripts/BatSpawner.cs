using System.Collections;
using UnityEngine;

public class BatSpawner : MonoBehaviour {

    public Transform bat;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public BatController SpawnBat(GameObject player)
    {
        var script = Instantiate(bat, transform.position, Quaternion.identity).GetComponent<BatController>();
        script.target = player;
        return script;
    }
}
