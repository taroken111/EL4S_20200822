using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Resources/Tables/SETable", fileName = "SETable")]
public class SeTable : ScriptableObject
{
    //SeTableが保存してある場所のパス
    public const string PATH = "Tables/SETable";

    //SeTableの実体
    private static SeTable _entity;
    public static SeTable Entity
    {
        get
        {
            //初アクセス時にロードする
            if (_entity == null)
            {
                _entity = Resources.Load<SeTable>(PATH);

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
        foreach (AudioClip se in SeDatas)
        {
            m_Indexes[se.ToString().Substring(0, se.ToString().IndexOf("(") - 1)] = i;
            i++;
        }
    }

    public AudioClip GetSE(string se_name) { return GetSE(m_Indexes.ContainsKey(se_name) ? m_Indexes[se_name] : 0); }

    public AudioClip GetSE(int index) { return (SeDatas.Count <= index) ? null : SeDatas[index]; }

    private Dictionary<string, int> m_Indexes;

    [SerializeField]
    private List<AudioClip> SeDatas = default;
    //! SE再生OBJ
    public GameObject OneShotSoundObj;
}
