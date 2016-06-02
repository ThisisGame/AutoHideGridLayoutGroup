using UnityEngine;
using System.Collections;

namespace ThisisGame
{
    public class test : MonoBehaviour
    {

        [SerializeField]
        GameObject gridLayoutGroup;

        // Use this for initialization
        void Start()
        {
            ThisisGame.AutoHideGridLayoutGroup autoHideGridLayoutGroup = gridLayoutGroup.GetComponent<ThisisGame.AutoHideGridLayoutGroup>();
            if (autoHideGridLayoutGroup != null)
            {
                autoHideGridLayoutGroup.Clear();
            }
            else
            {
                autoHideGridLayoutGroup = gridLayoutGroup.gameObject.AddComponent<ThisisGame.AutoHideGridLayoutGroup>();
            }
            autoHideGridLayoutGroup.onShow = this.OnShow;
            autoHideGridLayoutGroup.Init();
        }

        void OnShow(Transform trans, bool show)
        {
            for (int index = 0; index < trans.childCount; index++)
            {
                if (trans.GetChild(index).gameObject.activeSelf != show)
                {
                    trans.GetChild(index).gameObject.SetActive(show);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

