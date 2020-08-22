using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManagerScript : MonoBehaviour
{
    public Sprite[] ColorImage;
    int ColorNum;

    // 現在の表示文字
    int CurrentColor;

    // 文字色
    int LetterColor;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ColorNum = ColorImage.Length;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCurrentColor()
    {
        return CurrentColor;
    }

    public int GetLetterColor()
    {
        return LetterColor;
    }

    public void ChangeColor()
    {
        CurrentColor = Random.Range(0, ColorNum);
        LetterColor = Random.Range(0, ColorNum);
        spriteRenderer.sprite = ColorImage[CurrentColor];

        switch (LetterColor)
        {
            case 0:
                spriteRenderer.color = new Color(1, 0, 0, 1); // 赤
                break;
            case 1:
                spriteRenderer.color = new Color(0, 0, 1, 1); // 青
                break;
            case 2:
                spriteRenderer.color = new Color(0, 1, 0, 1); // 黄色
                break;
            case 3:
                spriteRenderer.color = new Color(1, 1, 0, 1); // 緑
                break;


            default:
                spriteRenderer.color = new Color(1, 1, 1, 1); // 白
                break;
        }
    }
}
