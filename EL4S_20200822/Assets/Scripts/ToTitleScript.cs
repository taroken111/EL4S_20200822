using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ToTitleScript : MonoBehaviour
{
    [SerializeField]
    private Button m_TitleButton;

    // Start is called before the first frame update
    void Start()
    {
        //! 押したら実行する関数を設定
        m_TitleButton.onClick.AddListener(Title);
    }

    void Title()
    {
        //! タイトルへ遷移
        SceneManager.LoadScene("TitleScene");
    }
}
