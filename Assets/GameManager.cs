using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event System.Action<float> OnPointsAdded;
    public static void TriggerAddPoint( float points ) => OnPointsAdded?.Invoke( points );

    public GameObject Cat;
  //  public GameObject SafeMessage;
    public GameObject Human;
    public GameObject humanIcon;
    public GameObject HumanCamera;
    public Light roomlight;
    public Color darkRoom;
    public Color lightRoom;
    public Animator doorAnim;
    public AudioSource music;

    public TextMeshProUGUI score;
    public TextMeshProUGUI Safetimer;
    public TextMeshProUGUI caught;
    public AudioClip Footsteps;
    public AudioClip DoorOpen;
    public AudioClip DoorClose;
    public AudioClip CatWow;
    public AudioClip HumanWalksIn;
    
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

    private void Awake()
    {
        OnPointsAdded += AddPoint;
    }

    private void OnDestroy()
    {
        OnPointsAdded -= AddPoint;
    }


    // Start is called before the first frame update
    void Start()
    {
        breakablesInRoom = FindObjectsOfType<Breakable>().Length;

        points = 0;
        score.text = points.ToString();
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
                Safetimer.text = "Coast is clear! Destroy it all!!";
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

            if(unsafeTime < 1f)
            {
                if( Human.TryGetComponent( out Animator anim ) )
                {
                    anim.SetBool( "IsVisible", false );
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
        if( !isGameOver )
        {
            isFreakingOut = true;
            freakingOutTrippin.SetActive( true );
            music.volume = 0.1f;
        }
        
    }

    public void PlayFreakingOutSound()
    {
        audioSource.clip = CatWow;
        Debug.Log("sound played");
        audioSource.Play();
    }

    public void SetFreakingOutFalse()
    {
        if( isFreakingOut )
        {
            music.volume = 1f;
            if( audioSource.clip == CatWow )
                audioSource.Stop();
        }

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
        score.text = points.ToString();

        // if human is around, lose
        if (Human.activeSelf)
        {
            GameOver();
        }
    }

    void makeHumanActive()
    {
        if(!Human.activeSelf)
        {
            audioSource.clip = HumanWalksIn;
            audioSource.Play();

            Human.SetActive( true );

            if( Human.TryGetComponent( out Animator anim ) )
            {
                anim.SetBool( "IsVisible", true );
            }
        }
        
        humanIcon.SetActive(true);        

        music.volume = 0.25f;
    }

    void makeHumanInactive()
    {
        if( Human.activeSelf )
        {
            audioSource.clip = DoorClose;
            audioSource.Play();
            Human.SetActive( false );
            humanIcon.SetActive( false );
            doorAnim.SetBool( "IsOpen", false );

            roomlight.color = darkRoom;
            music.volume = 1f;

            if( HumanCamera )
                HumanCamera.SetActive( false );
        }
    }

    void GameOver()
    {
        isGameOver = true;
        isFreakingOut = false;
        if (breakablesInRoom == 0)
        {
            finalMessage.text = "Success! You broke everything!";
        }
        else if (isFreakingOut)
        {
            finalMessage.text = "Bad Kitty! You got caught nip-tripping...";
        }
        else
        {
            finalMessage.text = "Bad Kitty! You got caught destroying stuff...";
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
        Safetimer.text = "Quick, act natural!";
        audioSource.clip = Footsteps;
        audioSource.Play();
        Debug.Log( "Footsteps" );

        yield return new WaitForSeconds( 2 );

        if( HumanCamera )
            HumanCamera.SetActive( true );

        yield return new WaitForSeconds(1);

        audioSource.clip = DoorOpen;
        roomlight.color = lightRoom;
        Debug.Log( "DoorOpen" );
        
        doorAnim.SetBool("IsOpen", true);
        makeHumanActive();
        
        yield return new WaitForSeconds(1);

        audioSource.Play();
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
