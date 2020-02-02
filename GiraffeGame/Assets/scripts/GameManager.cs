using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public int stage;
    public GameObject[] window;
    public int giraffeSpeed;
    private GameObject giraffe;
    private GameObject baseGiraffe;
    private GameObject baseWindow;
    private static float windowDistX = 8;
    private static float windowDistY = 4.5f;
    public GameObject player;
    public bool gameOver;
    public bool stageOver;
    private GameObject basePlatform;
    private GameObject[] platforms;
    private bool start;
    public GameObject startScreen;
    public Text stageText;
    public int score;
    public Text ShowScore;
    public GameObject credits;
    
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        stage = 1;
        baseWindow = (GameObject)Resources.Load("prefabs/window", typeof(GameObject));
        basePlatform = (GameObject)Resources.Load("prefabs/platform", typeof(GameObject));
        baseGiraffe = (GameObject)Resources.Load("prefabs/giraffe", typeof(GameObject));
        giraffe = GameObject.FindGameObjectWithTag("giraffe");
        startScreen = GameObject.Find("StartScreen");
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            updateScore();
            checkStart();
            checkLevelEnd();
        }
        
    }
    bool checkBrokenWindows() {
        window = GameObject.FindGameObjectsWithTag("window");
        for(int i = 0; i < window.Length; i++)
        {
            if (!window[i].GetComponent<window>().broken)
            {
                return false;
            }
        }

        return true;
    }

    void updateScore()
    {
        ShowScore.text = "Score: "+score;
    }
    void checkStart()
    {
        if (Input.GetButtonDown("Start"))
        {
            start = true;
            player.GetComponent<playerMovement>().unlockThem();
            giraffe.GetComponent<giraffe>().startThrowing();
            startScreen.SetActive(false);
            GameObject.Find("instructions").SetActive(false);
        }
        
    }
    void checkLevelEnd()
    {
        if (player.GetComponent<creatureHealth>().currentHealth == 0 || checkBrokenWindows())
        {
            gameOver = true;
        }

        if(giraffe.GetComponent<creatureHealth>().currentHealth == 0)
        {
            stageOver = true;
            
        }

        if (gameOver)
        {
            StartCoroutine(gameOverScreen());
        }
        else if (stageOver)
        {
            player.GetComponent<setAnimBools>().setAllFalse();
            score += 2000 * stage;
            GameObject[] hams = GameObject.FindGameObjectsWithTag("hammer");
            for(int i = 0; i < hams.Length; i++)
            {
                Destroy(hams[i]);
            }
            hams = GameObject.FindGameObjectsWithTag("hammer(player)");
            for (int i = 0; i < hams.Length; i++)
            {
                Destroy(hams[i]);
            }
            stageOver = false;
            destroyLevel();
            stage += 1;
            createStage(Mathf.Min(stage+6,12));
            StartCoroutine(transitionLevel());
        }


    }
    IEnumerator transitionLevel()
    {
        
        stageText.text = "stage " + stage;
        player.GetComponent<playerMovement>().lockThem();
        giraffe.GetComponent<giraffe>().stopThrowing();
        yield return new WaitForSeconds(3);
        stageText.text = "";
        player.GetComponent<playerMovement>().unlockThem();
        giraffe.GetComponent<giraffe>().startThrowing();
    }
    IEnumerator gameOverScreen()
    {
        player.GetComponent<playerMovement>().lockThem();
        destroyLevel();
        stageText.text = "Game Over";
        startScreen.SetActive(true);
        credits.SetActive(true);
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (Input.GetButton("Start"))
            {
                SceneManager.LoadScene(0);
            }
            yield return null;
        }
        
        
    }
    void destroyLevel()
    {
        window = GameObject.FindGameObjectsWithTag("window");
            if (window != null)
            {
                for(int i = 0; i < window.Length; i++)
                {
                    Destroy(window[i]);
                }
            }
        platforms = GameObject.FindGameObjectsWithTag("platform");
        if (platforms != null)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                Destroy(platforms[i]);
            }
        }
    }
    void createStage(int numWindows)
    {
        
        int winWidth;
        Vector3 spawnPoint = new Vector3(0, -6, 0);
        Vector3 spawnPlatform = new Vector3(0, -8, 0);
        if (numWindows < 13)
        {
            winWidth = 3;
        }
        else
        {
            winWidth = 4;
        }
        window = new GameObject[numWindows];

        for(int i =0; i< numWindows; i++)
        {
            window[i] = Instantiate(baseWindow, spawnPoint, Quaternion.identity);
            Instantiate(basePlatform, spawnPlatform, Quaternion.identity);
            if((i+1)%winWidth == 0)
            {
                spawnPoint = new Vector3(0, spawnPoint.y + windowDistY, 0);
            }
            else
            {
                spawnPoint = new Vector3(spawnPoint.x-windowDistX, spawnPoint.y, 0);
            }
            spawnPlatform = new Vector3(spawnPoint.x, spawnPoint.y - 2f, 0);
        }
        giraffe.GetComponent<giraffe>().setWindows(window);
        giraffe.GetComponent<giraffe>().interval = Mathf.Max(3-stage/5,.5f);
        giraffe.GetComponent<creatureHealth>().currentHealth = giraffe.GetComponent<creatureHealth>().maxHealth;
    }
}
