using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow = null;


    [Space(10)]
    [Header("Variables")]
    [SerializeField] private float cameraSpeed = 3f;




    private void Update()
    {
        Vector3 followPosition = objectToFollow.position;
        Vector3 myPosition = this.transform.position;

        float interpolation = cameraSpeed * Time.deltaTime;

        myPosition.x = Mathf.Lerp(myPosition.x, followPosition.x, interpolation);
        myPosition.y = Mathf.Lerp(myPosition.y, followPosition.y, interpolation);

        this.transform.position = myPosition;

    }


}
