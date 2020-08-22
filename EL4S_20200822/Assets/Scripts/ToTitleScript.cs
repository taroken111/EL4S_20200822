using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ToTitleScript : MonoBehaviour
{
    [SerializeField]
    private Button m_TitleButton;

    //private Fade m_Fade;

    // Start is called before the first frame update
    void Start()
    {
        //m_Fade = FindObjectOfType<Fade>();
        //! 押したら実行する関数を設定
        m_TitleButton.onClick.AddListener(Title);
    }

    void Title()
    {
        //! タイトルへ遷移
        //m_Fade.FadeIn(2.0f, "TitleScene");
        SceneManager.LoadScene("TitleScene");
    }
}
