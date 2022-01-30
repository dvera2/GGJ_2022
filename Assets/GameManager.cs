using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject Cat;
    public GameObject SafeMessage;
    public GameObject Human;
    public GameObject humanIcon;
    public Light roomlight;
    public Color darkRoom;
    public Color lightRoom;
    public Animator doorAnim;

    public TextMeshProUGUI score;
    public TextMeshProUGUI Safetimer;
    public TextMeshProUGUI caught;
    public AudioClip Footsteps;
    public AudioClip DoorOpen;
    public AudioClip DoorClose;
    AudioSource audioSource;

    float safeTime = 5.0f;
    bool safeTimerIsRunning;

    float unsafeTime;
    bool unsafeTimerIsRunning;

    public bool isHidden;
    public bool isHumanAround;

    [Header("Ui States")]
    public GameObject PlayState;
    public GameObject GameOverState;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI finalMessage;
    public GameObject freakingOutTrippin;
    public int breakablesInRoom;


    int points;
    bool isGameOver;
    bool isFreakingOut;



    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        score.text = "Points:"+ points;
        safeTimerIsRunning = true;
        audioSource = GetComponent<AudioSource>();
        roomlight.color = darkRoom;
        PlayState.SetActive(true);
        GameOverState.SetActive(false);


        setSafeTimer();
        
        Human.SetActive(false);
        humanIcon.SetActive(false);
        caught.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (safeTimerIsRunning && !isGameOver)
        {
            if (safeTime > 0)
            {
                safeTime -= Time.deltaTime;
                Safetimer.text = Mathf.Floor(safeTime).ToString();
            }

            if (safeTime <= 0)
            {
                safeTimerIsRunning = false;
                StartCoroutine("HumansComing");
                
            }
        }

        if (breakablesInRoom == 0 && !isGameOver)
        {
            GameOver();
        }



        if (unsafeTimerIsRunning && !isGameOver)
        {
            if (unsafeTime > 0)
            {
                unsafeTime -= Time.deltaTime;
                Safetimer.text = "UNSAFE: "+ Mathf.Floor(unsafeTime).ToString();

                if (isFreakingOut)
                {
                    GameOver();
                }
            }

            if(unsafeTime <= 0)
            {
                makeHumanInactive();
                unsafeTimerIsRunning = false;               
                safeTimerIsRunning = true;
                setSafeTimer();

            }
        }


        if (isHumanAround)
        {
            makeHumanActive();

            if (!isHidden)
            {
                GameOver();
            }
        }
        
    }

    public void SetFreakingOutTrue()
    {
        isFreakingOut = true;
        freakingOutTrippin.SetActive(true);
        
    }

    public void SetFreakingOutFalse()
    {
        isFreakingOut = false;
        freakingOutTrippin.SetActive(false);

    }

    public void SetHidden()
    {
        SafeMessage.SetActive(true);
        isHidden = true;
    }

    public void SetVisable()
    {
        SafeMessage.SetActive(false);
        isHidden = false;
    }

    public void AddPoint()
    {
        if (isFreakingOut)
        {
            points += 2;
        }
        else
        {
            points++;
        }
        breakablesInRoom--;
        score.text = "Points:" + points;
        if (!safeTimerIsRunning)
        {
            GameOver();
        }
    }

    void makeHumanActive()
    {
        Human.SetActive(true);
        humanIcon.SetActive(true);
        
    }

    void makeHumanInactive()
    {
        doorAnim.SetBool("IsOpen", false);
        audioSource.clip = DoorClose;
        audioSource.Play();
        Human.SetActive(false);
        humanIcon.SetActive(false);
        roomlight.color = darkRoom;
        

    }

    void GameOver()
    {
        isGameOver = true;
        if (breakablesInRoom == 0)
        {
            finalMessage.text = "You broke everything!";
        }
        caught.gameObject.SetActive(true);
        PlayState.SetActive(false);
        GameOverState.SetActive(true);
        finalScore.text = points.ToString();
    }

    void setSafeTimer()
    {
        float randomTime = Random.Range(5,10);
        safeTime = randomTime;
    }

    IEnumerator HumansComing()
    {
        Safetimer.text = "Act natural!!";
        audioSource.clip = Footsteps;
        audioSource.Play();
        Debug.Log( "Footsteps" );

        yield return new WaitForSeconds(3);

        audioSource.clip = DoorOpen;
        roomlight.color = lightRoom;
        Debug.Log( "DoorOpen" );
        
        doorAnim.SetBool("IsOpen", true);
        yield return new WaitForSeconds(1);

        audioSource.Play();
        Debug.Log( "Play #1" );
        makeHumanActive();
        safeTimerIsRunning = false;
        unsafeTimerIsRunning = true;
        unsafeTime = 5.0f;
    }

    public void reloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }



}
