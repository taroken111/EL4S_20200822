using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIScript : MonoBehaviour
{
    [SerializeField]
    private float m_Time;
    public int m_CorrectNum;

    private bool m_Start;

    [SerializeField, Space(20)]
    private Text m_Timer;
    [SerializeField]
    private Text m_CorrectCounter;

    [SerializeField]
    private GetCorrectScript m_GetCorrect;
    [SerializeField]
    private Fade m_Fade;

    // Start is called before the first frame update
    void Start()
    {
        m_Start = false;
        m_Time = 30.0f;
        m_GetCorrect = FindObjectOfType<GetCorrectScript>();
        m_Fade = FindObjectOfType<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_Fade.m_FadeEnd)
        {
            return;
        }

        m_Time -= Time.deltaTime;

        m_CorrectCounter.text = m_CorrectNum.ToString();

        if (m_Time >= 0.0f)
        {
            float seconds = Mathf.RoundToInt(m_Time % 60);

            if (m_Time < 10)
            {
                m_Timer.text = "0" + m_Time.ToString("F2");
            }
            else
            {
                m_Timer.text = m_Time.ToString("F2");
            }
        }
        else
        {
            m_Timer.text = "00.00";
            m_GetCorrect.m_Correct = m_CorrectNum;


            //! タイトルへ遷移
            SceneManager.LoadScene("ResultScene");
        }  
    }
}
