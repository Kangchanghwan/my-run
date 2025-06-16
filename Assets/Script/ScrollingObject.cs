using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public float speed = 10f;
    
 
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
