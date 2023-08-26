using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TestAStart : MonoBehaviour
{
    public int beginX = -3;
    public int beginY = 5;

    public int offsetX = 2;
    public int offsetY = 2;
    public int mapW = 15;
    public int mapH = 15;

    private Dictionary<string,GameObject> cubes = new Dictionary<string, GameObject>();
    public Material red;
    public Material yellow;
    public Material normal;

    public Material blue;

    private List<AStarNode> list;
    private Vector2 beginPos = Vector2.right * -1;

    private GameObject beginNodeObj = null;
    //private AStarNode endNode = null;
    // Start is called before the first frame update
    void Start()
    {
        AStarMgr.Instance.InitMapInfo(mapW,mapH);
        for (int i = 0; i < mapW; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i*offsetX,beginY + j*offsetY,0);
                obj.name = i + "_" + j;
                cubes.Add(obj.name,obj);

                AStarNode node = AStarMgr.Instance.nodes[i,j];
                if(node.type == E_Node_Type.Stop)
                {
                    
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray,out info,1000))
            {
               if(beginPos == Vector2.right * -1)
               {

                    if(list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            cubes[list[i].x + "_" + list[i].y].GetComponent<MeshRenderer>().material = normal;
                        }
                    }

                    string[] strs = info.collider.gameObject.name.Split('_');
                    beginPos = new Vector2(int.Parse(strs[0]),int.Parse(strs[1]));
                    beginNodeObj = info.collider.gameObject;
                    info.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
               }
                else
                {
                    string[] strs = info.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(strs[0]),int.Parse(strs[1]));
                    GameObject endNodeObj = info.collider.gameObject;

                    list = AStarMgr.Instance.FindPath(beginPos,endPos);
                    cubes[beginPos.x +"_" + beginPos.y].GetComponent<MeshRenderer>().material = normal;
                    if(list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            cubes[list[i].x +"_" + list[i].y].GetComponent<MeshRenderer>().material = blue;
                        }
                    }
                    //else
                    //{
                        //if(beginNodeObj != null )
                        //{
                        //    string[] strs1 = info.collider.gameObject.name.Split('_');
                        //    if(AStarMgr.Instance.nodes[int.Parse(strs1[0]), int.Parse(strs1[1])].type == E_Node_Type.Stop)
                        //    {
                        //        beginNodeObj.GetComponent<MeshRenderer>().material = red;
                        //    }
                        //}
                        //if (endNodeObj != null)
                        //{
                        //    string[] strs1 = info.collider.gameObject.name.Split('_');
                        //    if (AStarMgr.Instance.nodes[int.Parse(strs1[0]), int.Parse(strs1[1])].type == E_Node_Type.Stop)
                        //    {
                        //        endNodeObj.GetComponent<MeshRenderer>().material = red;
                        //    }
                        //}
                    //}
                    beginPos = Vector2.right * -1;
                    beginNodeObj = null;
                }
            }
           
        }
    }
}
