using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    private float timeCounter;
    public CoinType LastType { get; private set; }

	// Use this for initialization
	void Awake () {
        timeCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        var tmp = timeCounter - Time.deltaTime;
        if (tmp >= 0) timeCounter = tmp;
        else if (timeCounter > 0) timeCounter = 0;
	}

    public void EnableAs(CoinType type)
    {
        Debug.Log("Activating a coin " + gameObject.name + " of type " + type.Tag);
        LastType = type;
        gameObject.SetActive(true);
        gameObject.tag = type.Tag;
        gameObject.GetComponent<Animator>().SetInteger("type", type.Id);
    }

    public void Disable()
    {
        timeCounter = 1.0f;
        gameObject.SetActive(false);
    }

    public void Disable(PlayerController player)
    {
        if (timeCounter > 0) return;
        Disable();
        player.Points += LastType.Points;
        Debug.Log(LastType.Tag);
    }

    public bool Ready
    {
        get
        {
            return timeCounter == 0;
        }
    }

    public class CoinType
    {
        public static readonly CoinType GOLD = new CoinType(0, "CoinGold", 15);
        public static readonly CoinType SILVER = new CoinType(1, "CoinSilver", 10);
        public static readonly CoinType BRONZE = new CoinType(2, "CoinBronze", 5);

        public int Id { get; private set; }
        public string Tag { get; private set; }
        public int Points { get; private set; }

        private CoinType(int id, string tag, int points)
        {
            Id = id;
            Tag = tag;
            Points = points;
        }
    }
}
