using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject Cat;
    public GameObject SafeMessage;
    public GameObject Human;
    public GameObject humanIcon;
    public Light roomlight;
    public Color darkRoom;
    public Color lightRoom;

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
    public GameObject freakingOutTrippin;
    public int breakablesInRoom;


    int points;
    bool isGameOver;
    bool isFreakingOut;
  //  CatController cc;


    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        score.text = "Points:"+ points;
        safeTimerIsRunning = true;
        audioSource = GetComponent<AudioSource>();
        roomlight.color = darkRoom;
        PlayState.SetActive(true);
 //       cc = Cat.GetComponent<CatController>();

        setSafeTimer();
        
        Human.SetActive(false);
        humanIcon.SetActive(false);
        caught.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (safeTimerIsRunning)
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

        if (breakablesInRoom == 0)
        {
            Debug.Log("Everything is Broken!");
        }



        if (unsafeTimerIsRunning)
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
        audioSource.clip = DoorClose;
        audioSource.Play();
        Human.SetActive(false);
        humanIcon.SetActive(false);

    }

    void GameOver()
    {
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
        Safetimer.text = "Hide!!";
        audioSource.clip = Footsteps;
        audioSource.Play();

        yield return new WaitForSeconds(3);

        audioSource.clip = DoorOpen;

        yield return new WaitForSeconds(1);

        audioSource.Play();
        makeHumanActive();
        safeTimerIsRunning = false;
        unsafeTimerIsRunning = true;
        unsafeTime = 5.0f;
    }



}
