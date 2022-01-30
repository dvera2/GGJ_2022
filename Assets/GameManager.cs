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

    float safeTime;
    bool safeTimerIsRunning;

    float unsafeTime;

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
        }

        if (safeTime <= 0)
        {
            safeTime = 0;
            Safetimer.text = Mathf.Floor(safeTime).ToString();
            makeHumanActive();
            safeTimerIsRunning = false;
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

    void yourCaught()
    {
        caught.gameObject.SetActive(true);
    }

    void setSafeTimer()
    {
        float randomTime = Random.Range(2,5);
        safeTime = randomTime;
        Safetimer.text = safeTime.ToString();
        Debug.Log("Timer is set to "+safeTime);
    }

}
