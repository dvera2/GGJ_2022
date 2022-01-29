using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject Cat;
    public GameObject SafeMessage;

    public TextMeshProUGUI score;

    public bool isHidden;

    int points;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        score.text = "Points:"+ points;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        points++;
        score.text = "Points:" + points;
    }
}
