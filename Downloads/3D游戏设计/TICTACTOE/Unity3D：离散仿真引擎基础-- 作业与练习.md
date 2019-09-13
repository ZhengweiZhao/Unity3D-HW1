@[TOC](Unity3D：离散仿真引擎基础-- 作业与练习)

# 3D Game Programming & Design
这是本学期3D游戏编程与设计的第一次作业，内容主要是熟悉Unity3D的基本原理和使用方法，以及C#的编程语法。

##  1、简答题
###  1）解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系。
 在Unity的官方介绍文档当中，可以找到对Assets的定义如下：
>An Asset is a representation of any item you can use in your game or Project. An Asset may come from a file created outside of Unity, such as a 3D Model, an audio file, an image, or any of the other file types that Unity supports. There are also some Asset types that you can create in Unity, such as a ProBuilder Mesh, an Animator Controller, an Audio Mixer, or a Render Texture.
>
资源是在游戏设计过程中你可以使用的一切物体和属性。Unity中有很多中资源， GameObject、Component理论上也都是资源，但我们通常说的资源指的是：材质、网格、动画和给一个物体插入一段代码等等。

而对GameObjects的定义如下：
>GameObjects are the fundamental objects in Unity that represent characters, props and scenery. They do not accomplish much in themselves but they act as containers for Components, which implement the real functionality.
For example, a Light object is created by attaching a Light component to a GameObject.

>A GameObject always has a Transform component attached (to represent position and orientation) and it is not possible to remove this. The other components that give the object its functionality can be added from the editor’s Component menu or from a script. There are also many useful pre-constructed objects (primitive shapes, Cameras, etc) available on the GameObject > 3D Object menu, see Primitive Objects.

游戏对象则是游戏当中呢的玩家，物体或者环境等等，它主要是代表一个角色。游戏对象可以由很多的资源组成，比如在meterial可以更改物体的颜色属性，还可以增加代码脚本，增加动画声音等等。

资源可以被多个对象使用，只需要把要用的资源呢拖到对应游戏对象处就行了，资源作为模版，可以实例化游戏中具体的对象。
###  2）下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）
![一个Unity3D 的游戏项目文件夹](https://img-blog.csdnimg.cn/20190911171355907.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70) ![assets子目录](https://img-blog.csdnimg.cn/20190911171306876.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
 
一个用Unity3d做的游戏项目中会包含一个Assets文件夹。资源文件夹的内容呈现在项目视图。这里存放着游戏的所有资源，在资源文件夹中，通常包括材质Materials、图片Images、预制件Prefabs、场景Scences、声音Sounds、脚本Scripts等等，根据游戏具体的需求和设计稍有不同，在这些文件夹下可以继续进行划分。 

游戏对象一般包括玩家，敌人，环境，摄像机等虚拟父类，对象按照用途进行组织，执行相同操作的对象放在一起。在unity3d的操作界面左边拖动不同的游戏对象，可以调整它们直接的层次关系，使得一个成为另一个的子类或父类，子类会继承父类的属性。
###  3）编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件。
 
 基本行为包括  `Awake() Start() Update() FixedUpdate() LateUpdate() `
 常用事件包括  `OnGUI() OnDisable() OnEnable() `
```javascript
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	void Awake()
	{
		Debug.Log("Awake");
	}
	// Use this for initialization
	void Start()
	{
		Debug.Log("Start");
	}
	// Update is called once per frame
	void Update()
	{
		Debug.Log("Update");
	}
	void FixedUpdate()
	{
		Debug.Log("FixedUpdate");
	}
	void LateUpdate()
	{
		Debug.Log("LateUpdate");
	}
	void OnGUI()
	{
		Debug.Log("OnGUI");
	}
	void OnDisable()
	{
		Debug.Log("OnDisable");
	}
	void OnEnable()
	{
		Debug.Log("OnEnable");
	}
}
```
运行的结果和顺序如下：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190911180117915.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
###  4）查找脚本手册，了解 GameObject，Transform，Component 对象

- **分别翻译官方对三个对象的描述（Description）**

>GameObject ：GameObjects are the fundamental objects in Unity that represent characters, props and scenery. They do not accomplish much in themselves but they act as containers for Components, which implement the real functionality.

翻译：游戏对象是Unity中表示游戏角色，游戏道具和游戏场景的基本对象。它们本身并不能完成很多功能，但是它们充当了那些给予他们实体功能的组件的容器。

>Transform ：The Transform component determines the Position, Rotation, and Scale of each object in the scene. Every GameObject has a Transform.

翻译：转换决定游戏对象的位置，以及它是如何旋转和缩放的。每个游戏对象都有一个转换。

>Component ：Components are the nuts & bolts of objects and behaviors in a game. They are the functional pieces of every GameObject.
>
翻译: 组件是游戏中对象和行为的细节，它是每个游戏对象的功能部分。

- **描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件.**
   本题目要求是把可视化图形编程界面与 Unity API 对应起来，当你在 Inspector 面板上每一个内容，应该知道对应 API。
    例如：table 的对象是 GameObject，第一个选择框是 activeSelf 属性。
    
    - table对象的属性：activeInHierarchy（表示GameObject是否在场景中处于active状态）、activeSelf（GameObject的本地活动状态）、isStatic（仅编辑器API，指定游戏对象是否为静态）、layer（游戏对象所在的图层。图层的范围为[0 … 31]）、scene（游戏对象所属的场景）、tag（游戏对象的标签）、transform（附加到这个GameObject的转换）
    - Table的Transform属性：Position、Rotation、Scale
    - Table的部件：Mesh Filter、Box Collider、Mesh Renderer
    
 - **用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）**
 ![在这里插入图片描述](https://img-blog.csdnimg.cn/20190911211106681.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
###  5）整理相关学习资料，编写简单代码验证以下技术的实现：
- 查找对象
    - 通过名字查找： 
    `public static GameObject Find(string name);`
This function only returns active GameObjects. If no GameObject with name can be found, null is returned. If name contains a '/' character, it traverses the hierarchy like a path name.
    - 通过标签查找单个对象：
    `public static GameObject FindWithTag(string tag);`
Returns one active GameObject tagged tag. Returns null if no GameObject was found.
Tags must be declared in the tag manager before using them. A UnityException will be thrown if the tag does not exist or an empty string or null is passed as the tag.
    - 通过标签查找多个对象：
    `public static GameObject[] FindGameObjectsWithTag(string tag);`
Returns a list of active GameObjects tagged tag. Returns empty array if no GameObject was found. Tags must be declared in the tag manager before using them. A UnityException will be thrown if the tag does not exist or an empty string or null is passed as the tag.

- 添加子对象
    `public static GameObject CreatePrimitive(PrimitiveType type);`
    Creates a game object with a primitive mesh renderer and appropriate collider.

- 遍历对象树
```javascript
foreach (Transform child in transform) {  
    Debug.Log(child.gameObject.name);  
} 
```
- 清除所有子对象
```javascript
foreach (Transform child in transform) {  
    Destroy(child.gameObject);  
} 
```


###  6）资源预设（Prefabs）与 对象克隆 (clone)
- **预设（Prefabs）有什么好处？**
预设体可以理解成是模版，它是可以反复利用的游戏对象，储存了完整储存了对象的组件和属性。预设使得修改的复杂度降低，，在后续创建相似的游戏对象的时候更方便，只需要修改预设即可，所有通过预设实例化的对象都会做出相应变化。
- **预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？**
修改预设会使通过预设实例化的所有对象都做出相应变化，而对象克隆出来后修改其中任何一个不会对其他的产生影响。
- **制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象**
创建完一个table实例后，将该对象拖到Assets池里就变成了perfabs预设体了。
```javascript
 void Start () {  
        GameObject anotherTable = (GameObject)Instantiate(table.gameObject);  
    }
```

## 2、 编程实践，小游戏

- 游戏内容： 井字棋 或 贷款计算器 或 简单计算器 等等
- 技术限制： 仅允许使用 IMGUI 构建 UI
- 作业目的：
了解 OnGUI() 事件，提升 debug 能力
提升阅读 API 文档能力
###  1）实验前基础知识储备
####  理解OnGUI() 事件

“Immediate Mode” GUI系统(也称为IMGUI)是Unity的一个独立于图形化视图的代码特性系统，完全通过代码来实现基于游戏对象的UI设计和其他功能的设计。IMGUI是一个代码驱动的GUI系统，主要用于作为程序员的工具。它是由对任何实现它的脚本的OnGUI函数的调用驱动的。

要创建IMGUI元素，必须编写代码进入一个名为OnGUI的特殊函数。代码需要在每一帧显示这个接口并绘制到屏幕上。除了你的代码附载的那个游戏对象以外，没有别的一直存在的或是和代码所依附的对象有继承关系的的对象了。

IMGUI允许您使用代码创建各种各样的功能GUI。与其创建gameobject(游戏对象)，手动定位它们，然后编写一个脚本来处理其功能，您可以用几行代码立即完成所有事情。代码生成GUI控件，这些控件通过一个函数调用来绘制和处理。

####  基本函数操作
- Label
 ```javascript
/* GUI.Label example */

using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour 
{
                    
    void OnGUI () 
    {
        GUI.Label (new Rect (25, 25, 100, 30), "Label");
    }

}
```

The Label is non-interactive. It is for display only. It cannot be clicked or otherwise moved. It is best for displaying information only.

- Button
  
 ```javascript
/* GUI.Button example */

using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour 
{
                    
    void OnGUI () 
    {
        if (GUI.Button (new Rect (25, 25, 100, 30), "Button")) 
        {
            // This code is executed when the Button is clicked
        }
    }

}
```

In UnityGUI, Buttons will return true when they are clicked. To execute some code when a Button is clicked, you wrap the the GUI.Button function in an if statement. Inside the if statement is the code that will be executed when the Button is clicked.

###  2）实验代码
 ```javascript
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Chess : MonoBehaviour
{
    private int[,] chessBoard = new int[3, 3];//初始化井字棋棋盘
    private int turn = 1;

    // Use this for initialization
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Reset()
    {
        turn = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                chessBoard[i, j] = 0;
            }
        }
    }

    void OnGUI()
    {
        // GUIStyle design.
        GUIStyle Style = new GUIStyle();
        Style.normal.background = null;
        Style.normal.textColor = new Color(0, 5, 1);
        Style.fontSize = 35;
        Style.fontStyle = FontStyle.Bold;
        
        //Type (Position, Content)
        //Rect() defines four properties: left-most position, top-most position, total width, total height.
        //You can use the Screen.width and Screen.height properties to get the total dimensions of the screen space available in the player.
        GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 250, 150, 40), "Tic Tac Toe");

        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 -200, 150, 50), "RESET GAME"))//displays a Box Control with the header text “RESET GAME”.
        {
            Reset();
        }

        //First judge the result.
        int State = check();
        //The second argument for a GUI Control is the actual content to be displayed with the Control. Most often you will want to display some text or an image on your Control.
        if (State == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 +50, 50, 50), "X Win!", Style);
        }
        else if (State == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 +50, 50, 50), "O Win!", Style);
        }
        else if (State == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 +50, 50, 50), "Equally", Style);
        }

        //The Game is not over,continue.
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //If the number of the chessboard is 1, it represents O, else 2 represents X.
                if (chessBoard[i, j] == 1)
                {
                    GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "O");
                }
                else if (chessBoard[i, j] == 2)
                {
                    GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "X");
                }

                //Click operation.
                if (GUI.Button(new Rect(Screen.width / 2 - 75 +50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), ""))
                {
                    if (State == 3)
                    {
                        if (turn == 1)
                        {
                            chessBoard[i, j] = 1;
                        }
                        else if (turn == -1)
                        {
                            chessBoard[i, j] = 2;
                        }
                        turn = -turn;
                    }
                }
            }
        }
    }

    int check()
    {
        //Check the rows.
        for (int i = 0; i < 3; i++)
        {
            if (chessBoard[i, 0] == chessBoard[i, 1] && chessBoard[i, 0] == chessBoard[i, 2] && chessBoard[i, 0] != 0)
            {
                return chessBoard[i, 0]; //1
            }
        }

        //Check the columns.
        for (int j = 0; j < 3; j++)
        {
            if (chessBoard[0, j] == chessBoard[1, j] && chessBoard[0, j] == chessBoard[2, j] && chessBoard[0, j] != 0)
            {
                return chessBoard[0, j]; //2
            }
        }

        //Check the diagonals.
        if (chessBoard[0, 0] == chessBoard[1, 1] && chessBoard[0, 0] == chessBoard[2, 2] && chessBoard[0, 0] != 0) return chessBoard[0, 0];

        if (chessBoard[0, 2] == chessBoard[1, 1] && chessBoard[0, 2] == chessBoard[2, 0] && chessBoard[0, 2] != 0) return chessBoard[0, 2];

        //Cteate count to find out who win the game.
        //If count == 9, then nobody win the game, the game go equally.
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (chessBoard[i, j] != 0)// some player put chess on that button.
                {
                    count++;
                }
            }
        }
        if (count == 9)
        {
            return 0;
        }
        return 3; //3
    }

}
```


###  3）实验结果
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190911220103370.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190911220117371.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190911220130544.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
[游戏实操视频](https://pan.baidu.com/s/14kD2WGfnXe-yqgyQLnxSDQ)

[Github代码传送门](https://github.com/ZhengweiZhao/Unity3D)


