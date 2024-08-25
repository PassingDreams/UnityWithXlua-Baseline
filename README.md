# Unity with xlua起点项目

做一些基础设施，男生自用



- unity版本 2022.3.23f1
- [xlua](https://github.com/Tencent/xLua/releases):xLua-2.1.16_with_ohos



## 待做列表

- 写一个系统脚本（.bat或者.sh都行），可以将指定文件的内容热更到项目中（IPC通知unity游戏进程去读取这个文件并hot load到lua env中）
  - 子任务：写一个vsc/sublime的快捷键绑定到这个系统脚本，从而实现一键将文件热更到运行的游戏中
- 输入系统搬迁