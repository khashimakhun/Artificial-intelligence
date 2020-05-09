using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Collisions : MonoBehaviour
{

    [Range(1, 360)]
    public int angle;
    [Range(0.01f, 10)]
    public float radius;
    [Range(0, 360)]
    public float rotation;

    public bool invertRotation = false;

    public Color32 color;

    public LayerMask masks;
    public List<Transform> visibleTargets = new List<Transform>();

    private void Start()
    {
        StartCoroutine(findTargets(1));
    }

    IEnumerator findTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            findVisibleTargets();
        }

    }

    void findVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInTheRadius = Physics2D.OverlapCircleAll(transform.position, radius, masks);

        for (int i = 0; i < targetsInTheRadius.Length; i++)
        {
            Vector2 dirToTarget = (targetsInTheRadius[i].transform.position - transform.position).normalized;

            dirToTarget = ((invertRotation) ? dirToTarget * -1 : dirToTarget);

            if (Vector2.Angle(DirFromAngle(getMiddleAngle(), 90), dirToTarget) < angle / 2)
            {
                visibleTargets.Add(targetsInTheRadius[i].transform);
            }
            
        }

    }

    float getFinalAngle()
    {
        return (angle / 2) + ((invertRotation) ? rotation * -1: rotation );
    }

    float getStartAngle()
    {
        return (-angle / 2) + ((invertRotation) ? rotation * -1 : rotation);
    }

    float getMiddleAngle()
    {
        return  0 - ((invertRotation) ? rotation * -1 : rotation);
    }

    float getRadius()
    {
        return ((invertRotation) ? radius * -1 : radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform visibleTarget in visibleTargets)
            Gizmos.DrawLine(transform.position, visibleTarget.position);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 viewAngleA = DirFromAngle(getStartAngle(), 90);
        Vector3 viewAngleB = DirFromAngle(getFinalAngle(), 90);   
        Create2DArc(viewAngleA, viewAngleB, transform.position, getRadius(), angle);

        
    }

    public Vector3 DirFromAngle(float angleInDegrees, float angleCorrection)
    {
        angleInDegrees += angleCorrection; //Para iniciar a direita

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    void Create2DArc(Vector3 viewAngleA, Vector3 viewAngleB, Vector3 from, float radius, float angle)
    {
        /*
        Gizmos.color = Color.white;
        Gizmos.DrawLine(from, from + viewAngleA * radius);
        */
        Gizmos.color = color;
        DrawArc(angle, getRadius(), getStartAngle());
        /*
        Gizmos.color = Color.white;
        Gizmos.DrawLine(from, from + viewAngleB * radius);
        */
    }


    void DrawArc(float angle, float radius, float initialAngle)
    {
        float steps = angle / Mathf.Deg2Rad;

        Vector3 oldPos = transform.position;
        for (int i = 0; i < steps + 1; i++)
        {
            Vector3 pos = new Vector3(Mathf.Cos(initialAngle * Mathf.Deg2Rad), Mathf.Sin(initialAngle * Mathf.Deg2Rad), 0f) * radius;
            Gizmos.DrawLine(oldPos, transform.position + pos);
            oldPos = transform.position + pos;

            initialAngle += Mathf.Deg2Rad;
        }

        if (angle < 360)
        {
            Gizmos.DrawLine(oldPos, transform.position);
        }

    }
}