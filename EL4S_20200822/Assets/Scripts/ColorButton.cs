using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorType
{
    RED,
    BLUE,
    YELLOW,
    GREEN
};

public class ColorButton : MonoBehaviour
{
    [SerializeField]
    private ColorType type = ColorType.RED;
    [SerializeField]
    private ColorSelector selector = null;

    private void Start()
    {
        if (selector == null) Debug.LogAssertion("セレクターアタッチして");
    }

    public void OnClick()
    {
        if (selector) selector.CompAnswer(type);
        SoundManager.Instance.PlaySe(SeTable.Entity.GetSE("Select"), 1f);
    }
}
