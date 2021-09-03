using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    //the speed to move at
    public float speed = 500f;

    private float originY;

    void Start()
    {
        this.originY = transform.position.y;
    }

    void Update()
    {
        Vector3 targetPos = GetPosition(Input.mousePosition);
        //transform.position = Vector3.MoveTorwards(ShpereTransform.position, tempPos, speed * Time.DeltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private Vector3 GetPosition(Vector2 pos)
    {
        Vector3 tempPos = transform.position;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            tempPos = hit.point;
        }
        return new Vector3(tempPos.x, originY, tempPos.z);
    }
}
