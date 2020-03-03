using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float width;
    public float height;
    public float minSplitArea;
    public float minRoomArea;
    public GameObject wall;

    private List<Node> leafList = new List<Node>();

    private class Node
    {
        public Vector2 bottomLeft;
        public Vector2 topRight;

        public Node left;
        public Node right;

        public Node(float blX, float blY, float trX, float trY)
        {
            bottomLeft = new Vector2(blX, blY);
            topRight = new Vector2(trX, trY);

            left = null;
            right = null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Node head = new Node(0, 0, width, height);
        SplitGenerate(head);

        //TODO: Tidy up (non repeating code, better var names)
        foreach (Node leaf in leafList)
        {
            // Generate two points of the room in the split section
            //TODO: Generate in a smarter way (keeps generating until a room matches the area)
            float area = 0;
            Vector3 p1 = new Vector3();
            Vector3 p2 = new Vector3();
            while (area < minRoomArea)
            {
                p1 = new Vector2(Mathf.Lerp(leaf.bottomLeft.x, leaf.topRight.x, Random.Range(0f, 1f)), Mathf.Lerp(leaf.bottomLeft.y, leaf.topRight.y, Random.Range(0f, 1f)));
                p2 = new Vector2(Mathf.Lerp(leaf.bottomLeft.x, leaf.topRight.x, Random.Range(0f, 1f)), Mathf.Lerp(leaf.bottomLeft.y, leaf.topRight.y, Random.Range(0f, 1f)));

                area = Mathf.Abs(p1.x - p2.x) * Mathf.Abs(p1.y - p2.y);
            }

            Vector3 p3 = new Vector2(p2.x, p1.y);
            Vector3 p4 = new Vector2(p1.x, p2.y);
            BuildWall(p1, p3);
            BuildWall(p3, p2);
            BuildWall(p2, p4);
            BuildWall(p4, p1);

            // Testing for correct splitting
            //Instantiate(wall, leaf.bottomLeft, Quaternion.identity);
            //Instantiate(wall, leaf.topRight, Quaternion.identity);
            //Debug.Log("Left: " + leaf.bottomLeft.x + ", " + leaf.bottomLeft.y + " Right: " + leaf.topRight.x + ", " + leaf.topRight.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildWall(Vector3 p1, Vector3 p2)
    {
        Vector3 between = p2 - p1;
        float distance = between.magnitude;
        Vector3 midPoint = p1 + (between / 2);

        var obj = Instantiate(wall, midPoint, Quaternion.identity);
        obj.transform.localScale = new Vector3(1, 1, distance);
        obj.transform.LookAt(p2);
    }

    private void SplitGenerate(Node target)
    {
        // Base Case
        float width = target.topRight.x - target.bottomLeft.x;
        float height = target.topRight.y - target.bottomLeft.y;
        if (width * height < minSplitArea)
        {
            leafList.Add(target);
            return;
        }

        
        if (Random.Range(0 , 2) == 0)
        {
            // Vertical
            float cutPointX = Mathf.Lerp(target.bottomLeft.x, target.topRight.x, Random.Range(.25f, .75f));
            target.left = new Node(target.bottomLeft.x, target.bottomLeft.y, cutPointX, target.topRight.y);
            target.right = new Node(cutPointX, target.bottomLeft.y, target.topRight.x, target.topRight.y);

        }
        else
        {
            // Horizontal
            float cutPointY = Mathf.Lerp(target.bottomLeft.y, target.topRight.y, Random.Range(.25f, .75f));
            target.left = new Node(target.bottomLeft.x, cutPointY, target.topRight.x, target.topRight.y);
            target.right = new Node(target.bottomLeft.x, target.bottomLeft.y, target.topRight.x, cutPointY);
        }

        SplitGenerate(target.left);
        SplitGenerate(target.right);
    }
}
