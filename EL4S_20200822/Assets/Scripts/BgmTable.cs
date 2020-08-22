using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Resources/Tables/BGMTable", fileName = "BGMTable")]

public class BgmTable : ScriptableObject
{
    //BgmTableが保存してある場所のパス
    public const string PATH = "Tables/BGMTable";

    //BgmTableの実体
    private static BgmTable _entity;
    public static BgmTable Entity
    {
        get
        {
            //初アクセス時にロードする
            if (_entity == null)
            {
                _entity = Resources.Load<BgmTable>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if (_entity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
                else _entity.Load();
            }

            return _entity;
        }
    }


    void Load()
    {
        m_Indexes = new Dictionary<string, int>();

        int i = 0;
        foreach (AudioClip bgm in BgmDatas)
        {
            m_Indexes[bgm.ToString().Substring(0, bgm.ToString().IndexOf("(") - 1)] = i;
            i++;
        }
    }

    public AudioClip GetBGM(string bgm_name) { return GetBGM(m_Indexes.ContainsKey(bgm_name) ? m_Indexes[bgm_name] : 0); }

    public AudioClip GetBGM(int index) { return (BgmDatas.Count <= index) ? null : BgmDatas[index]; }

    private Dictionary<string, int> m_Indexes;

    [SerializeField]
    private List<AudioClip> BgmDatas = default;
}
