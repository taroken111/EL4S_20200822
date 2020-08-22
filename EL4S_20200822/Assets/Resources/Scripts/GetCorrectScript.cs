using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCorrectScript : MonoBehaviour
{
    public static GetCorrectScript m_Instance;

    public int m_Correct;
    void Awake()
    {
        //!instanceがuniqueであるように
        if (m_Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            m_Instance = this;
        }
        else if (m_Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
