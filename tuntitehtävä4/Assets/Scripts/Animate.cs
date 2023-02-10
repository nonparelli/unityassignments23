using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animate : MonoBehaviour
{
    public float AnimTime;
    private float accu = 0.0f;
    private bool IsAnimating = true;
    public MyProgressBar[] bars;
    //int Value = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AnimateIt();
    }
    private void AnimateIt()
    {
        accu += Time.deltaTime;
        if(accu > AnimTime)
        {
            IsAnimating = false;
            return;
        }
        float t = accu / AnimTime;
        foreach(MyProgressBar bar in bars)
        {
            bar.SetCurrent(Mathf.RoundToInt(100*t));
        }
    }
}
