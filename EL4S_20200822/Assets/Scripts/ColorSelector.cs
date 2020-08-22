using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    [Tooltip("このチェックボックスを入れると文字ではなく色が一致したものが正解となります")]
    [SerializeField]
    private bool ColorMode = false;

    //正解の文字
    private ColorType CorrectString = ColorType.RED;
    //正解の色
    private ColorType CorrectColor = ColorType.RED;

    /*
     * @fn SetCorrectAnswer
     * @brief 正解の文字と色をセット
     * @param (_string) 正解の文字番号
     * @param (_color) 正解の色番号
    */
    public void SetCorrectAnswer(int _string, int _color)
    {
        CorrectString = (_string >= 0 && _string < 4) ? (ColorType)_string : ColorType.RED;
        CorrectColor = (_color >= 0 && _color < 4) ? (ColorType)_color : ColorType.RED;
    }

    /*
     * @fn CompAnswer
     * @brief 合否判定
     * @param (answer) ボタンタイプ
    */
    public void CompAnswer(ColorType answer)
    {
        ColorType comp = ColorMode ? CorrectColor : CorrectString;
        if (comp == answer) Correct();
        else Wrong();
    }

    /*
     * @fn Correct
     * @brief 正解時の処理
    */
    void Correct()
    {
        Debug.Log("正解!!");
    }

    /*
     * @fn Wrong
     * @brief 間違った場合の処理
    */
    void Wrong()
    {
        Debug.Log("不正解...");
    }
}
