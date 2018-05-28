using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    [SerializeField] public int initialPoints = 25;
    [SerializeField] public uint pointsToWin = 70;
    [SerializeField] private int totalSeconds = 60;
    [SerializeField] private int coinsEachRound = 2;
    [SerializeField] private int batsEachRound = 1;
    [SerializeField] private int treesEachRound = 1;
    [SerializeField] private float secondsBetweenRounds = 5.0f;

    [SerializeField] private float porcentageMonedas5 = 0.5f;
    [SerializeField] private float porcentageMonedas10 = 0.35f;
    [SerializeField] private float porcentageMonedas15 = 0.15f;

    private GameObject[] coinObjects;
    private GameObject[] batSpawners;
    private GameObject[] treeSpawners;

    private Text countDownText;
    private GameObject endTextObject;
    private System.TimeSpan countdown;
    private bool gamePaused = false;

    public GameObject grid1;
    public GameObject grid2;
    public GameObject grid3;
    public GameObject actualGrid;

    public double Countdown
    {
        get
        {
            return countdown.TotalSeconds;
        }

        set
        {
            countdown = System.TimeSpan.FromSeconds(value);
            countDownText.text = string.Format("{0:D2}:{1:D2}", countdown.Minutes, countdown.Seconds);
        }
    }

    // Use this for initialization
    void Start () {
        countDownText = GameObject.Find("CountDownText").GetComponent<Text>();
        endTextObject = GameObject.Find("EndText");

        endTextObject.SetActive(false);
        endTextObject.GetComponent<Text>().enabled = false;
        countdown = System.TimeSpan.FromSeconds(totalSeconds);

        coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        DisableAllCoins();
        InvokeRepeating("ActivateCoins", 2.0f, secondsBetweenRounds);

        batSpawners = GameObject.FindGameObjectsWithTag("BatSpawner");
        InvokeRepeating("SpawnBats", 2.0f, secondsBetweenRounds);

        treeSpawners = GameObject.FindGameObjectsWithTag("TreeSpawner");
        InvokeRepeating("SpawnTrees", 2.0f, secondsBetweenRounds);

        actualGrid = grid1;

    }

    void DisableAllCoins()
    {
        if (coinsEachRound > coinObjects.Length)
        {
            coinsEachRound = coinObjects.Length;
        }
        foreach (var coin in coinObjects)
        {
            coin.SetActive(false);
        }
    }

    void ActivateCoins()
    {
        var coinsToActivate = coinObjects
                .Where(coin => !coin.activeSelf && coin.GetComponent<CoinController>().Ready)
                .OrderBy(coin => Random.Range(0, 1))
                .Take(coinsEachRound).ToList();

        foreach (var coin in coinsToActivate)
        {

            float random = Random.Range(1, 100);
            float bronzeBound = porcentageMonedas5 * 100;
            float silverBound = bronzeBound + (porcentageMonedas10 * 100);
            float goldBound = silverBound + (porcentageMonedas15 * 100);

            var controller = coin.GetComponent<CoinController>();
            if (1 <= random && random < bronzeBound)
            {
                controller.EnableAs(CoinController.CoinType.BRONZE);
            }
            else if (bronzeBound <= random && random < silverBound)
            {
                controller.EnableAs(CoinController.CoinType.SILVER);
            }
            else if (silverBound <= random && random <= goldBound)
            {
                controller.EnableAs(CoinController.CoinType.GOLD);
            }
        }
    }

    void SpawnBats()
    {
        for (int i = 0; i < batsEachRound; i++)
        {
            int index = UnityEngine.Random.Range(0, batSpawners.Length);

            batSpawners[index].GetComponent<BatSpawner>().SpawnBat(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    void SpawnTrees()
    {
        for (int i = 0; i < treesEachRound; i++)
        {
            int index = UnityEngine.Random.Range(0, treeSpawners.Length);

            treeSpawners[index].GetComponent<TreeSpawner>().SpawnTree(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    public void GameOver(PlayerController player)
    {
        endTextObject.SetActive(true);
        var endText = endTextObject.GetComponent<Text>();
        if (player.Points >= pointsToWin)
        {
            if (SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadScene("Main 1", LoadSceneMode.Single);
            }else if (SceneManager.GetActiveScene().name == "Main 1")
            {
                SceneManager.LoadScene("Main 2", LoadSceneMode.Single);
            }
            else
            {
                Destroy(player.gameObject);
                endText.text = "You Win :)";
                endText.enabled = true;
            }
        }
        else
        {
            Destroy(player.gameObject);
            endText.text = "Game Over :(";
            endText.enabled = true;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Countdown > 0)
        {
            Countdown -= Time.deltaTime;
        }else if (!endTextObject.activeSelf)
        {
            var players = GameObject.FindGameObjectsWithTag("Player").Select(player => player.GetComponent<PlayerController>());
            foreach (PlayerController player in players)
            {
                GameOver(player);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused)
            {
                resumeGame();
            }else
            {
                pauseGame();
            }
        }
    }
    private void pauseGame()
    {
        endTextObject.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;
        var endText = endTextObject.GetComponent<Text>();
        endText.alignment = TextAnchor.MiddleCenter;
        endText.text = "PAUSE";
        endText.enabled = true;
    }

    public void resumeGame()
    {
        endTextObject.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1;
        var endText = endTextObject.GetComponent<Text>();
        endText.enabled = false;
    }
}
