using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUILogic : MonoBehaviour
{
    TextMeshProUGUI timerText;

    private float timer_;
    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer_ += Time.deltaTime;
        timerText.text = timer_.ToString("0.0") + "s";
    }
}
