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

        // 子节点是否可见 子节点 -- 是否可见
        Dictionary<Transform, bool> childShowStatus = new Dictionary<Transform, bool>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            rectTransform = GetComponent<RectTransform>();

            gridLayoutGroup = GetComponent<GridLayoutGroup>();



            //注册ScrollRect滚动回调;
            scrollRect = transform.parent.GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener((data) => { ScrollCallback(data); });

            //获取所有child;
            for (int index = 0; index < transform.childCount; index++)
            {
                children.Add(transform.GetChild(index).GetComponent<RectTransform>());
            }

            UpdateChildren();
        }


        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            if (scrollRect!=null)
            {
                scrollRect.onValueChanged.RemoveAllListeners();
            }

            if (children != null)
            {
                children.Clear();
            }

            onShow = null;

            childShowStatus.Clear();
        }

        /// <summary>
        /// 动态添加，异步创建的时候，创建一个，调用一次Add;
        /// </summary>
        public void Add()
        {
            if (scrollRect != null)
            {
                scrollRect.onValueChanged.RemoveAllListeners();
            }

            if (children != null)
            {
                children.Clear();
            }

            childShowStatus.Clear();

            Init();
        }

        void ScrollCallback(Vector2 data)
        {
            UpdateChildren();
        }

        void UpdateChildren()
        {
            if (scrollRect.vertical)
            {
                RectTransform scrollRectTrans = scrollRect.GetComponent<RectTransform>();
                float scrollRectUp = scrollRect.transform.TransformPoint(new Vector3(0, scrollRectTrans.rect.height * (1 - scrollRectTrans.pivot.y))).y;

                RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();
                Vector3 scrollRectAnchorBottom = new Vector3(0, -scrollRectTransform.rect.height - gridLayoutGroup.spacing.y, 0f);
                float scrollRectBottom = scrollRect.transform.TransformPoint(scrollRectAnchorBottom + new Vector3(0, scrollRectTrans.rect.height * (1 - scrollRectTrans.pivot.y))).y;

                for (int childindex = 0; childindex < children.Count; childindex++)
                {
                    RectTransform child = children[childindex];

                    Vector3 childBottomLeft = new Vector3(child.anchoredPosition.x, child.anchoredPosition.y + (1 - child.pivot.y) * child.rect.height - gridLayoutGroup.cellSize.y, 0f);
                    float childBottom = transform.TransformPoint(childBottomLeft).y;

                    Vector3 childUpLeft = new Vector3(child.anchoredPosition.x, child.anchoredPosition.y + (1 - child.pivot.y) * child.rect.height, 0f);

                    float childUp = transform.TransformPoint(childUpLeft).y;

                    if (childBottom <= scrollRectUp && childUp >= scrollRectBottom)
                    {
                        if (onShow != null)
                        {
                            if (childShowStatus.ContainsKey(child.transform))
                            {
                                if (childShowStatus[child.transform] == true)
                                {
                                    continue;
                                }
                                else
                                {
                                    childShowStatus[child.transform] = true;
                                    onShow(child.transform, true);
                                }
                            }
                            else
                            {
                                childShowStatus.Add(child.transform, true);
                                onShow(child.transform, true);
                            }
                        }
                    }
                    else
                    {
                        if (onShow != null)
                        {
                            if (childShowStatus.ContainsKey(child.transform))
                            {
                                if (childShowStatus[child.transform] == false)
                                {
                                    continue;
                                }
                                else
                                {
                                    onShow(child.transform, false);
                                    childShowStatus[child.transform] = false;
                                }
                            }
                            else
                            {
                                onShow(child.transform, false);
                                childShowStatus.Add(child.transform, false);
                            }
                        }
                    }
                }
            }
            else
            {
                RectTransform scrollRectTransform = scrollRect.GetComponent<RectTransform>();

                float scrollRectLeft = scrollRect.transform.TransformPoint(new Vector3(0 - scrollRectTransform.rect.width * (scrollRectTransform.pivot.x), 0, 0)).x;


                Vector3 scrollRectAnchorRight = new Vector3(scrollRectTransform.rect.width + gridLayoutGroup.spacing.x, 0, 0f);
                float scrollRectRight = scrollRect.transform.TransformPoint(scrollRectAnchorRight - new Vector3(scrollRectTransform.rect.width * (scrollRectTransform.pivot.x), 0)).x;


                for (int childindex = 0; childindex < children.Count; childindex++)
                {
                    RectTransform child = children[childindex];

                    Vector3 childBottomRight = new Vector3(child.anchoredPosition.x - (child.pivot.x) * child.rect.width + gridLayoutGroup.cellSize.x, child.anchoredPosition.y, 0f);
                    float childRight = transform.TransformPoint(childBottomRight).x;

                    Vector3 childUpLeft = new Vector3(child.anchoredPosition.x - (child.pivot.x) * child.rect.width, child.anchoredPosition.y, 0f);
                    float childLeft = transform.TransformPoint(childUpLeft).x;

                    if (childRight >= scrollRectLeft && childLeft <= scrollRectRight)
                    {
                        if (onShow != null)
                        {
                            if (childShowStatus.ContainsKey(child.transform))
                            {
                                if (childShowStatus[child.transform] == true)
                                {
                                    continue;
                                }
                                else
                                {
                                    childShowStatus[child.transform] = true;
                                    onShow(child.transform, true);
                                }
                            }
                            else
                            {
                                childShowStatus.Add(child.transform, true);
                                onShow(child.transform, true);
                            }
                        }
                    }
                    else
                    {
                        if (onShow != null)
                        {
                            if (childShowStatus.ContainsKey(child.transform))
                            {
                                if (childShowStatus[child.transform] == false)
                                {
                                    continue;
                                }
                                else
                                {
                                    childShowStatus[child.transform] = false;
                                    onShow(child.transform, false);
                                }
                            }
                            else
                            {
                                childShowStatus.Add(child.transform, false);
                                onShow(child.transform, false);
                            }
                        }
                    }
                }
            }
        }
    }
}

