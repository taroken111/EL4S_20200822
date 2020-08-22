using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    public float DestructionTimer = 5.0f; // 自壊までのタイムリミット
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destrucution());
    }

    IEnumerator Destrucution()
    {
        //待機時間
        yield return new WaitForSeconds(DestructionTimer);
        Destroy(this.transform.root.gameObject);
    }
}
