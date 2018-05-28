using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnGrid1 : MonoBehaviour {

	public List<GameObject> places;
	public GameObject heart;
	public GameObject energies;
	public GameObject projectiles;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		spawnAwards();
	}
	
	void spawnAwards()
	{
		var random = UnityEngine.Random.Range(0, 500);

		if (random == 4)
		{
			var randomPlace = UnityEngine.Random.Range(0, places.Count - 1);
			Instantiate(heart, places[randomPlace].transform.position + new Vector3(1, 0, 0), Quaternion.identity);
		}

		if (random == 7)
		{
			var randomPlace = UnityEngine.Random.Range(0, places.Count - 1);
			Instantiate(energies, places[randomPlace].transform.position - new Vector3(1, 0, 0), Quaternion.identity);
		}

		if (random == 11)
		{
			var randomPlace = UnityEngine.Random.Range(0, places.Count - 1);
			Instantiate(projectiles, places[randomPlace].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
		}
	}
}
