using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;

    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
    TMP_Text [] scoreText;
    //TMP_Text highScoreText;
    public int highScore;
    string highScoreKey = "HighScore";

    private void Start()
    {
        //setup gameover panel
        gameOverPanel.SetActive(false);
        //highScoreText = gameOverPanel.GetComponentInChildren<TMP_Text>();
        scoreText = gameOverPanel.GetComponentsInChildren<TMP_Text>();
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);

        //belakang
        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        //depan
        for (int z = 1; z <= frontDistance; z++)
        {
            //random value grass dan road
            var prefab = GetNextRandomTerrainPrefab(z);

            CreateTerrain(prefab, z);
        }

        player.SetUp(backDistance, extent);
    }

    private int playerLastMaxTravel;
    private void Update()
    {
        //cek player life or die?
        if (player.IsDie && gameOverPanel.activeInHierarchy == false)
            StartCoroutine(ShowGameOverPanel());

        //auto infinite terrain
        if (player.MaxTravel == playerLastMaxTravel)
            return;

        playerLastMaxTravel = player.MaxTravel;

        //create depan
        var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance);
        CreateTerrain(randTbPrefab, player.MaxTravel + frontDistance);

        //destroy belakang
        var lastTB = map[(player.MaxTravel - 1) + backDistance];

        //TerrainBlock lastTB = map[player.MaxTravel + frontDistance];
        //int lastPos = player.MaxTravel;
        //foreach (var (pos, tb) in map)
        //{
        //    if (pos < lastPos)
        //    {
        //        lastPos = pos;
        //        lastTB = tb;
        //    }
        //}

        map.Remove((player.MaxTravel - 1) + backDistance);
        Destroy(lastTB.gameObject);

        player.SetUp(player.MaxTravel + backDistance, extent);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(2);
        if (player.MaxTravel >= highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, player.MaxTravel);
            PlayerPrefs.Save();
            scoreText [0].text = "Score : " + player.MaxTravel;
            scoreText [1].text = "High Score : " + player.MaxTravel;
        }
        else
        {
            scoreText[0].text = "Score : " + player.MaxTravel;
            scoreText[1].text = "High Score : " + highScore;
        }

        //player.enabled = false;
        gameOverPanel.SetActive(true);
    }

    public void retryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void backMenu()
    {
        SceneManager.LoadScene("UI");
    }

    public void resetScore()
    {
            PlayerPrefs.DeleteAll();
            gameOverPanel.SetActive(false);
            scoreText[0].text = "Score : " + player.MaxTravel;
            scoreText[1].text = "High Score : " + highScore;
            gameOverPanel.SetActive(true);
    }

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos - 1];
        for (int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if (map[nextPos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if (isUniform)
        {
            if (tbRef is Grass)
                return road;
            else
                return grass;
        }

        //random generate block dengan chance 50%
        return Random.value > 0.5f ? road : grass;
    }
}
