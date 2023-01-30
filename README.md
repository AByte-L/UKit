# UKit
个人开发常用功能汇总的工具集

## 条件

- Unity 2019.1
- 依赖 UniRX
- 依赖 Odin
- 依赖 UniTask


## 使用说明

### 查找场景中挂载某个组件的对象

** 功能描述：**

根据某个组件的属性或者字段的值，查找挂载了这个组件的游戏对象，用于快速定位某些配置错误的组件



** 使用方法：**

- 菜单栏: UKit->查找场景中组件，打开窗口

- 配置文件： 这个配置文件是我们自己定义的需要查找的合集，需要我们自己创建，在Project窗口中，右键->Create->UKit->组件查找集合，创建资源，并命名为FindComponentColl，并放在 Resources 根目录下

- 配置文件的作用是配置好我们经常要查找的组件（以及它的字段或者属性），需要维护一个集合，但是我们不要手动的向集合中填数据，而是通过AddItem来选择类型，选择成员类型，选择成员，点击添加按钮，向集合里面添加

- 查找时，我们选择查找组件，就在这个集合里面选择，并根据我们的配置自动显示条件（字段或者属性）

- 点击Find 查找即可




### 修改场景对象名

简单略

### 修改资源对象名


同修改场景对象名