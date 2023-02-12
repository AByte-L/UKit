using System.Collections.Generic;
using AByte.UKit.Utilities;
using UnityEngine;
using Sirenix.OdinInspector;


public class Node
{

    public int ID;
    public string Name;

    public List<Node> Childs;

}
public class NodesTest : MonoBehaviour
{

    List<Node> nodeList = new List<Node>();
    void Start()
    {

        //构建Nodes
        for (int i = 0; i < 10; i++)
        {

            var node = new Node { ID = i, Name = "Name :" + i };
            node.Childs = new List<Node>();
            for (int j = 0; j < 3; j++)
            {
                node.Childs.Add(new Node { ID = j, Name = $"Child: {i} : {j}" });
            }
            nodeList.Add(node);

        }

    }

    [ShowInInspector]
    public void Test1()
    {
        //获取所有 ID == 2
        var re = NodesHelper.GetNodesByCondtion(nodeList, (x) => x.Childs, (x) => x.ID == 2);

        foreach (var item in re)
        {
            Debug.Log(item.Name);
        }

    }
    [ShowInInspector]

    public void Test2()
    {
        //获取所有 没有子对象的
        var re = NodesHelper.GetNodesByCondtion(nodeList, (x) => x.Childs, (x) => x.Childs == null);

        foreach (var item in re)
        {
            Debug.Log(item.Name);
        }

    }




}
