using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject Cat;
  //  public GameObject SafeMessage;
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
    public AudioClip CatWow;
    AudioSource audioSource;

    [Header("Set Timers")]
    public float tripingTime;
    public float safeTimeMin;
    public float safeTimeMax;
    float safeTime;

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


    float points;
    bool isGameOver;
    bool isFreakingOut;



    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        score.text = "Damage: $"+ points;
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
                Safetimer.text = "Distory it all!!";
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
                Safetimer.text = "Careful! He's Watching.";

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

    public void PlayFreakingOutSound()
    {
        audioSource.clip = CatWow;
        Debug.Log("sound played");
        audioSource.Play();
    }

    public void SetFreakingOutFalse()
    {
        isFreakingOut = false;
        freakingOutTrippin.SetActive(false);

    }

    public void SetHidden()
    {
        isHidden = true;
    }

    public void SetVisable()
    {
        isHidden = false;
    }

 //Add Points
    public void AddPoint(float value)
    {
        if (isFreakingOut)
        {
            points += value*2;
        }
        else
        {
            points += value;
        }
        breakablesInRoom--;
        score.text = "Damage $" + points;
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
        isFreakingOut = false;
        if (breakablesInRoom == 0)
        {
            finalMessage.text = "Sucess! You broke everything!";
        }
        else if (isFreakingOut)
        {
            finalMessage.text = "You got caught tripping...";
        }
        else
        {
            finalMessage.text = "You got caught destorying stuff...";
        }
        caught.gameObject.SetActive(true);
        PlayState.SetActive(false);
        freakingOutTrippin.SetActive(false);
        GameOverState.SetActive(true);
        finalScore.text = "$"+points.ToString();
    }

    void setSafeTimer()
    {
        float randomTime = Random.Range(safeTimeMin,safeTimeMax);
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
        makeHumanActive();
        safeTimerIsRunning = false;
        unsafeTimerIsRunning = true;
        unsafeTime = 5.0f;
    }

    public void reloadScene()
    {
        Debug.Log("Realoading Scene?");
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }



}
