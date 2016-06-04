# AutoHideGridLayoutGroup

判断一个GameObject是否在 ScrollRect范围内，通过回调 OnScroll ，bool 参数表示是否在ScrollRect范围内。



如果不在范围内表示这个GameObject可以做SetActive false 处理，减少顶点 push 以及渲染。

注意

Unity SetActive 是没有内部判断的。

就是说，如果这个GameObject 当前  是active 的，我们再次调用 SetActive(true) ，仍然会去执行 SetActive 的操作，SetActive

这个操作是很耗时的！


所以在对一个GameObject 进行 SetActive 的时候，如果当前状态已经是目标状态，就请跳过。

if(child.activeSelf ==  show) return;


异步创建的支持，请查看 Tutorials/testAsync.cs

