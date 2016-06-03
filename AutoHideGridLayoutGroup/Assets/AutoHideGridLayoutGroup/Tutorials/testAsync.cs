using UnityEngine;
using System.Collections;

namespace ThisisGame
{
    public class testAsync : MonoBehaviour
    {
        [SerializeField]
        GameObject gridLayoutGroup;

        [SerializeField]
        GameObject item;


        // Use this for initialization
        void Start()
        {
            StartCoroutine(CreateAsync());
        }

        IEnumerator CreateAsync()
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

            for (int index = 0; index < 200; index++)
            {
                GameObject obj = Instantiate(item) as GameObject;
                obj.transform.SetParent(item.transform.parent);
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;

                autoHideGridLayoutGroup.Add();
                yield return null;
            }

            
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


