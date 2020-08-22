using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGMScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBgm(BgmTable.Entity.GetBGM(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
