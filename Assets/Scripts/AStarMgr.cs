using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMgr
{
    private static AStarMgr instance;
    public static AStarMgr Instance
    {
       get
       {
            if (instance == null)
            {
                instance = new AStarMgr();
            }
            return instance;
       }
    }

    private int mapW;
    private int mapH;
    public AStarNode[,] nodes;
    private List<AStarNode> openList = new List<AStarNode>();
    private List<AStarNode> closeList = new List<AStarNode>();

    //初始化格子信息
    public void InitMapInfo(int w,int h)
    {
        nodes = new AStarNode[w,h];
        this.mapW = w;
        this.mapH = h;
        //根据宽高 创建格子  随机阻挡
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                AStarNode node = new AStarNode(i,j,Random.Range(0,100) < 50 ? E_Node_Type.Stop : E_Node_Type.Walk);
                nodes[i,j] = node;
            }
        }
    }

    //寻路方法
    public List<AStarNode> FindPath(Vector2 startPos,Vector2 endPos)
    {
        //先判断 传入的两个点是否合法
        //1.首先 地图范围内
        if (startPos.x < 0 || startPos.x >= mapW ||
            startPos.y < 0 || startPos.y >= mapH || 
            endPos.x < 0 || endPos.x >= mapW ||
            endPos.y < 0 || endPos.y >= mapH )
        {
            Debug.Log("开始或者结束点再地图格子范围外");
            return null;
        }
        //2.要不是阻挡
        //如果不合法 直接返回null 意味着不能寻路
        AStarNode start = nodes[(int)startPos.x,(int)startPos.y];
        AStarNode end = nodes[(int)endPos.x,(int)endPos.y];
        if (start.type == E_Node_Type.Stop ||
            end.type == E_Node_Type.Stop)
        {
            Debug.Log("开始或者结束点是阻挡");
            return null;
        }

        //情空上一次数据
        openList.Clear();
        closeList.Clear();

        //应该得到起点和终点 对应的格子
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);

        //从起点开始找周围点 放入开启列表钟

        while(true)
        {
                //左上
            FindeNearlyNodeToOpenList(start.x - 1,start.y - 1 ,1.4f,start,end);
            //上
            FindeNearlyNodeToOpenList(start.x ,start.y - 1 ,1,start,end);
            //右上
            FindeNearlyNodeToOpenList(start.x + 1,start.y - 1 ,1.4f,start,end);
            //左
            FindeNearlyNodeToOpenList(start.x - 1,start.y ,1,start,end);
            //右
            FindeNearlyNodeToOpenList(start.x + 1,start.y ,1,start,end);
            //左下
            FindeNearlyNodeToOpenList(start.x - 1,start.y + 1 ,1.4f,start,end);
            //下
            FindeNearlyNodeToOpenList(start.x ,start.y + 1 ,1,start,end);
            //右下
            FindeNearlyNodeToOpenList(start.x + 1,start.y + 1 ,1.4f,start,end);
            //判断这些点 是否边界 阻挡 是否在开启或者关闭列表中 如果都不是才放入开启列表

            if(openList.Count == 0)
            {
                Debug.Log("死路");
                return null;
            }
            //选出开启列表中 寻路消耗最小的点
            openList.Sort(SortOpenList);
            //放入关闭列表中 然后再从开启列表中移除
            closeList.Add(openList[0]);
            start = openList[0];
            openList.RemoveAt(0);

            //如果这个点是终点 得到最终结果返回出去
            //如果这个点 不是终点 那么继续寻路
            if(start == end)
            {
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while(end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                //列表反转
                path.Reverse();
                return path;
            }
        }
    }

    private int SortOpenList(AStarNode a,AStarNode b)
    {
        if(a.f >= b.f)
            return 1;
        else
            return -1;
    }

    private void FindeNearlyNodeToOpenList(int x, int y,float g ,AStarNode father,AStarNode end)
    {
        if(x< 0 || x>= mapW ||
            y< 0 || y >= mapH)
            return;

        AStarNode node = nodes[x,y];
        if(node == null || node.type == E_Node_Type.Stop || 
            closeList.Contains(node) ||
            openList.Contains(node))
            return;
        
        //计算f值
        //寻路消耗 f = 离起点的距离 g + 离终点的距离 h
        node.father = father;
        node.g = father.g + g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;

        openList.Add(node);
    }

}
