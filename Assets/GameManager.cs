using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject Cat;
    public GameObject SafeMessage;
    public GameObject Human;
    public GameObject humanIcon;

    public TextMeshProUGUI score;
    public TextMeshProUGUI Safetimer;
    public TextMeshProUGUI caught;

    float safeTime = 5.0f;
    bool safeTimerIsRunning;

    float unsafeTime;
    bool unsafeTimerIsRunning;

    public bool isHidden;
    public bool isHumanAround;

    int points;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        score.text = "Points:"+ points;
        safeTimerIsRunning = true;

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
                makeHumanActive();
                safeTimerIsRunning = false;
                unsafeTimerIsRunning = true;
                unsafeTime = 5.0f;
            }
        }



        if (unsafeTimerIsRunning)
        {
            if (unsafeTime > 0)
            {
                unsafeTime -= Time.deltaTime;
                Safetimer.text = "UNSAFE: "+ Mathf.Floor(unsafeTime).ToString();
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
                yourCaught();
            }
        }
        
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
        if (!safeTimerIsRunning)
        {
            yourCaught();
        }
        points++;
        score.text = "Points:" + points;
    }

    void makeHumanActive()
    {
        Human.SetActive(true);
        humanIcon.SetActive(true);
        
    }

    void makeHumanInactive()
    {
        Human.SetActive(false);
        humanIcon.SetActive(false);

    }

    void yourCaught()
    {
        caught.gameObject.SetActive(true);
    }

    void setSafeTimer()
    {
        float randomTime = Random.Range(5,10);
        safeTime = randomTime;
        Debug.Log("Timer is set to "+safeTime);
    }

}
