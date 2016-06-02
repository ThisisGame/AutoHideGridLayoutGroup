/**************************
 * 文件名:AutoHideGridLayoutGroup.cs;
 * 文件描述:超出ScrollRect的部分隐藏;
 * 创建日期:2016/06/02;
 * Author:ThisisGame;
 * Page:https://github.com/ThisisGame/AutoHideGridLayoutGroup
 ***************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace ThisisGame
{
    public class AutoHideGridLayoutGroup : MonoBehaviour
    {
        public Action<Transform, bool> onShow = null;

        RectTransform rectTransform;

        GridLayoutGroup gridLayoutGroup;

        ScrollRect scrollRect;

        List<RectTransform> children = new List<RectTransform>();

        Vector2 startPosition;

        Vector2 gridLayoutSize;
        Vector2 gridLayoutPos;
        Dictionary<Transform, Vector2> childsAnchoredPosition = new Dictionary<Transform, Vector2>();

        // Use this for initialization
        public void Init()
        {
            rectTransform = GetComponent<RectTransform>();

            gridLayoutGroup = GetComponent<GridLayoutGroup>();

            gridLayoutPos = rectTransform.anchoredPosition;
            gridLayoutSize = rectTransform.sizeDelta;


            //注册ScrollRect滚动回调;
            scrollRect = transform.parent.GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener((data) => { ScrollCallback(data); });

            //获取所有child anchoredPosition 以及 SiblingIndex;
            for (int index = 0; index < transform.childCount; index++)
            {
                Transform child = transform.GetChild(index);
                RectTransform childRectTrans = child.GetComponent<RectTransform>();
                childsAnchoredPosition.Add(child, childRectTrans.anchoredPosition);
            }

            //获取所有child;
            
            for (int index = 0; index < transform.childCount; index++)
            {
                children.Add(transform.GetChild(index).GetComponent<RectTransform>());
            }

            startPosition = rectTransform.anchoredPosition;
        }

        public void Clear()
        {
            if (scrollRect!=null)
            {
                scrollRect.onValueChanged.RemoveAllListeners();
            }
            if (childsAnchoredPosition != null)
            {
                childsAnchoredPosition.Clear();
            }
            if (children != null)
            {
                children.Clear();
            }

            onShow = null;
        }

        void ScrollCallback(Vector2 data)
        {
            Vector2 currentPos = rectTransform.anchoredPosition;

            float offsetY = currentPos.y - startPosition.y;

            startPosition = currentPos;
            
            if (scrollRect.vertical)
            {
                RectTransform scrollRectTrans = scrollRect.GetComponent<RectTransform>();
                float scrollRectUp = scrollRect.transform.TransformPoint(new Vector3(0, scrollRectTrans.rect.height * (1 - scrollRectTrans.pivot.y))).y;

                RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
                Vector3 scrollRectAnchorBottom = new Vector3(0, -scrollRectTransform.rect.height - gridLayoutGroup.spacing.y, 0f);
                float scrollRectBottom = scrollRect.transform.TransformPoint(scrollRectAnchorBottom+ new Vector3(0, scrollRectTrans.rect.height * (1 - scrollRectTrans.pivot.y))).y;

                for (int childindex = 0; childindex < children.Count; childindex++)
                {
                    Vector3 childBottomLeft = new Vector3(children[childindex].anchoredPosition.x, children[childindex].anchoredPosition.y + (1-children[childindex].pivot.y) * children[childindex].rect.height - gridLayoutGroup.cellSize.y, 0f);
                    float childBottom = transform.TransformPoint(childBottomLeft).y;

                    Vector3 childUpLeft = new Vector3(children[childindex].anchoredPosition.x, children[childindex].anchoredPosition.y + (1-children[childindex].pivot.y) * children[childindex].rect.height, 0f);

                    float childUp = transform.TransformPoint(childUpLeft).y;

                    if (childBottom <= scrollRectUp && childUp >= scrollRectBottom)
                    {
                        if (onShow != null)
                        {
                            onShow(children[childindex], true);
                        }
                    }
                    else
                    {
                        if (onShow != null)
                        {
                            onShow(children[childindex], false);
                        }
                    }
                }
            }
            else
            {
                RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();

                float scrollRectLeft = scrollRect.transform.TransformPoint(new Vector3(0-scrollRectTransform.rect.width*(scrollRectTransform.pivot.x), 0,0)).x;

                
                Vector3 scrollRectAnchorRight = new Vector3(scrollRectTransform.rect.width + gridLayoutGroup.spacing.x, 0, 0f);
                float scrollRectRight = scrollRect.transform.TransformPoint(scrollRectAnchorRight- new Vector3(scrollRectTransform.rect.width * (scrollRectTransform.pivot.x), 0)).x;


                for (int childindex = 0; childindex < children.Count; childindex++)
                {
                    Vector3 childBottomRight = new Vector3(children[childindex].anchoredPosition.x - (children[childindex].pivot.x )* children[childindex].rect.width + gridLayoutGroup.cellSize.x, children[childindex].anchoredPosition.y, 0f);
                    float childRight = transform.TransformPoint(childBottomRight).x;

                    Vector3 childUpLeft = new Vector3(children[childindex].anchoredPosition.x -(children[childindex].pivot.x) * children[childindex].rect.width, children[childindex].anchoredPosition.y, 0f);
                    float childLeft = transform.TransformPoint(childUpLeft).x ;

                    if (childRight >= scrollRectLeft && childLeft <= scrollRectRight)
                    {
                        if (onShow != null)
                        {
                            onShow(children[childindex], true);
                        }
                    }
                    else
                    {
                        if (onShow != null)
                        {
                            onShow(children[childindex], false);
                        }
                    }
                }
            }
        }
    }
}

