using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif
[ExecuteInEditMode()]

public class MyProgressBar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/My Progress Bar")]
    public static void AddProgressBar()
    {
        Object o = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("UI/MyProgressBar"),
            Selection.activeGameObject.transform);
    }
#endif
    public int Maximum = 100;
    public int Minimum = 0;
    [SerializeField][Range(0,100)]
    private int Current = 100;

    public EasingFunction.Ease EasingType;

    private EasingFunction.Function EasingFun;

    //public Image ProgressBar;
    public Image Fill_image;
    void Start()
    {
        EasingFun = EasingFunction.GetEasingFunction(EasingType);
    }

    // Update is called once per frame
    void Update()
    {
        if( Fill_image != null)
        {
            //GetComponent<Image>().fillAmount = ((float)Current /(Maximum-Minimum));
            float tval = EasingFun(0f, 1f, (Current-Minimum) / (float)(Maximum - Minimum));
            GetComponent<Image>().fillAmount = tval;

            if (GetComponent<Image>().fillAmount >= 0.5f)
            {
                Fill_image.color = Color.Lerp(Color.yellow, Color.green, GetComponent<Image>().fillAmount/0.5f-1f);
            }
            else
            {
                Fill_image.color = Color.Lerp(Color.red, Color.yellow, GetComponent<Image>().fillAmount/0.5f);
            }
        }
    }

    public void SetMinimum(int value)
    {
        Minimum = value;

    }
    public void SetMaximum(int value)
    {
        Maximum = value;
    }
    public void SetCurrent(int value)
    {
        Current = value;
    }
}
