using UnityEngine;

public class BoundsColliders : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider = null;

    [Space(10)]
    [Header("Variables")]
    [SerializeField] private float colDepth = 4f;
    [SerializeField] private float zPosition = 0f;


    [Space(10)]
    [SerializeField] private bool generateWalls = false;
    [SerializeField] private int wallLayerInt = 12;
    [SerializeField] private Color spriteColor = Color.cyan;
    [SerializeField] private Sprite mySprite = null;

    Vector2 boxSize;
    Vector3 boxPosition;

  
    GameObject topGameObj;
    GameObject bottomGameObj;
    GameObject leftGameObj;
    GameObject rightGameObj;

    Transform topTransform;
    Transform bottomTransform;
    Transform leftTransform;
    Transform rightTransform;

    SpriteRenderer topSR;
    SpriteRenderer bottomSR;
    SpriteRenderer leftSR;
    SpriteRenderer rightSR;

    void Start()
    {
        if (generateWalls)
        {
            //Generate our empty objects
            topGameObj = new GameObject();
            bottomGameObj = new GameObject();
            leftGameObj = new GameObject();
            rightGameObj = new GameObject();

            //Set layers
            topGameObj.layer = wallLayerInt;
            bottomGameObj.layer = wallLayerInt;
            leftGameObj.layer = wallLayerInt;
            rightGameObj.layer = wallLayerInt;

            //Name our objects 
            topGameObj.name = "TopCollider";
            bottomGameObj.name = "BottomCollider";
            leftGameObj.name = "LeftCollider";
            rightGameObj.name = "RightCollider";

            //Set Transforms
            topTransform = topGameObj.transform;
            bottomTransform = bottomGameObj.transform;
            leftTransform = leftGameObj.transform;
            rightTransform = rightGameObj.transform;


            // Add the sprite renderers
            topSR = topGameObj.AddComponent<SpriteRenderer>();
            bottomSR = bottomGameObj.AddComponent<SpriteRenderer>();
            leftSR = leftGameObj.AddComponent<SpriteRenderer>();
            rightSR = rightGameObj.AddComponent<SpriteRenderer>();

            //Add the colliders
            topGameObj.AddComponent<BoxCollider2D>().size = Vector2.one;
            bottomGameObj.AddComponent<BoxCollider2D>().size = Vector2.one;
            leftGameObj.AddComponent<BoxCollider2D>().size = Vector2.one;
            rightGameObj.AddComponent<BoxCollider2D>().size = Vector2.one;

            //Set sprite renderers
            topSR.color = spriteColor;
            topSR.sprite = mySprite;
            bottomSR.color = spriteColor;
            bottomSR.sprite = mySprite;
            leftSR.color = spriteColor;
            leftSR.sprite = mySprite;
            rightSR.color = spriteColor;
            rightSR.sprite = mySprite;


            //Make them the child of whatever object this script is on, preferably on the Camera so the objects move with the camera without extra scripting
            topTransform.parent = transform;
            bottomTransform.parent = transform;
            rightTransform.parent = transform;
            leftTransform.parent = transform;

            //Generate world space point information for position and scale calculations
            boxPosition = this.transform.position + boxCollider.center;
            boxSize.x = boxCollider.size.x;
            boxSize.y = boxCollider.size.y;

            //Change our scale and positions to match the edges of the screen...   
            rightTransform.localScale = new Vector3(colDepth, boxSize.y + colDepth * 2, colDepth);
            rightTransform.position = new Vector3(boxPosition.x + (boxSize.x / 2) + (rightTransform.localScale.x * 0.5f), boxPosition.y, zPosition);

            leftTransform.localScale = new Vector3(colDepth, boxSize.y + colDepth * 2, colDepth);
            leftTransform.position = new Vector3(boxPosition.x - (boxSize.x / 2) - (leftTransform.localScale.x * 0.5f), boxPosition.y, zPosition);

            topTransform.localScale = new Vector3(boxSize.x + colDepth * 2, colDepth, colDepth);
            topTransform.position = new Vector3(boxPosition.x, boxPosition.y + (boxSize.y / 2) + (topTransform.localScale.y * 0.5f), zPosition);

            bottomTransform.localScale = new Vector3(boxSize.x + colDepth * 2, colDepth, colDepth);
            bottomTransform.position = new Vector3(boxPosition.x, boxPosition.y - (boxSize.y / 2) - (bottomTransform.localScale.y * 0.5f), zPosition);
        }
        

    }


}
