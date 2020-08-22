using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberScript: MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text m_Num;
    [SerializeField]
    private GetCorrectScript m_GetCorrect;

    // Start is called before the first frame update
    void Start()
    {
        m_GetCorrect = FindObjectOfType<GetCorrectScript>();
        m_Num.text = m_GetCorrect.m_Correct.ToString();
    }
}
