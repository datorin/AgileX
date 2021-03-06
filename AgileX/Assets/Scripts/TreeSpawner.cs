﻿using UnityEngine;

public class TreeSpawner : MonoBehaviour {

    public GameObject tree;
    private GameObject lastInstance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public TreeController SpawnTree(GameObject player)
    {
        if (lastInstance == null || !lastInstance.activeSelf)
        {
            lastInstance = Instantiate(tree, transform.position, Quaternion.identity);
            var script = lastInstance.GetComponent<TreeController>();
            script.target = player;
            return script;
        }
        else
        {
            return lastInstance.GetComponent<TreeController>();
        }
        
    }
}
