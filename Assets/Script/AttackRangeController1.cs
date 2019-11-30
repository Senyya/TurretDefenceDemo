using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the class to display the attack range
public class AttackRangeController1 : MonoBehaviour
{
    public static AttackRangeController1 Instance;
    void Awake()
    {
        Instance = this;
    }

    // method to set the object this script attached on
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    // method to define the property of the line shown the attack range 
    private LineRenderer DefineAttackRangeProperty(Transform obj)
    {
        LineRenderer lineRender = obj.gameObject.GetComponent<LineRenderer>();
        if (lineRender == null)
        {
            lineRender = obj.gameObject.AddComponent<LineRenderer>();
        }
        lineRender.startWidth = 0.1f;
        lineRender.endWidth = 0.1f;
        return lineRender;
    }

    // method to draw the attack range 
    public void DrawAttackRange (float radius)
    {
        Transform obj = this.transform;
        Vector3 center = obj.position;
        LineRenderer lineRender = DefineAttackRangeProperty(obj);
        int pointAmountUsed = 100;
        float perAngle = 360.0f / pointAmountUsed;
        Vector3 forward = obj.forward;
        lineRender.positionCount = (pointAmountUsed + 1);
        for (int i = 0; i <= pointAmountUsed; i++)
        {
            Vector3 position = Quaternion.Euler(0f, perAngle * i, 0f) * forward * radius + center;
            lineRender.SetPosition(i, position);
        }
    }
}
