using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEditor;
using UnityEngine;


namespace Rapadura
{

    [System.Serializable]
    public class Detection : SimpleAI_Condition
    {

        [SerializeField]
        public string name = "Detection";
        string description = "Condition´s Description.";

        public override string getName()
        {
            return name;
        }

        //[Range(1, 360)]
        public int angle = 11;
        //[Range(0.01f, 10)]
        public double radius = 1f;
        //[Range(0, 360)] 
        public double rotation = 0f;

        public LayerMask target;
        List<GameObject> visibleTargets = new List<GameObject>();


        public override bool isSatisfied()
        {
            return visibleTargets.Count > 0;
        }

        public override void update(SimpleAI _simpleAI, Transform transform)
        {
            findVisibleTargets(_simpleAI, transform);
        }

        void findVisibleTargets(SimpleAI _simpleAI, Transform transform)
        {
            visibleTargets.Clear();
            Collider2D[] targetsInTheRadius = Physics2D.OverlapCircleAll(transform.position, (float)radius, target);
            

            for (int i = 0; i < targetsInTheRadius.Length; i++)
            {
                Vector2 dirToTarget = (targetsInTheRadius[i].transform.position - transform.position).normalized;
                dirToTarget = ((!_simpleAI.facingRight) ? dirToTarget * -1 : dirToTarget);

                float targetDistance = Vector2.Distance(transform.position, targetsInTheRadius[i].transform.position);

                if (Vector2.Angle(DirFromAngle(getMiddleAngle(_simpleAI), 90), dirToTarget) < angle / 2)
                {
                    if (!Physics2D.Raycast(transform.position, dirToTarget * ((_simpleAI.facingRight) ? 1 : -1 ), targetDistance, _simpleAI.gameConfig.obistacleLayer + _simpleAI.gameConfig.groundLayer))
                    {
                        visibleTargets.Add(targetsInTheRadius[i].gameObject);
                    }
                    
                }else
                {

                    bool didItHit = false;

                    float detectionAngle = ((_simpleAI.facingRight)? getFinalAngle(_simpleAI) : getStartAngle(_simpleAI));
                    Vector3 pos = new Vector3(Mathf.Cos(detectionAngle * Mathf.Deg2Rad), Mathf.Sin(detectionAngle * Mathf.Deg2Rad), 0f);
                    //Debug.DrawRay(transform.position, pos * getRadius(_simpleAI), Color.yellow);
                    RaycastHit2D[] hitTopList = Physics2D.RaycastAll(transform.position, pos, getRadius(_simpleAI), target);
                    
                    for (int hitIndex = 0; i < hitTopList.Length; i++)
                    {
                        if (hitTopList[hitIndex].collider.Equals(targetsInTheRadius[i]))
                        {
                            didItHit = true;
                            break;
                        }
                    }


                    if (!didItHit)
                    {
                        detectionAngle = getMiddleAngle(_simpleAI);
                        pos = new Vector3(Mathf.Cos(detectionAngle * Mathf.Deg2Rad), Mathf.Sin(detectionAngle * Mathf.Deg2Rad), 0f);
                        //Debug.DrawRay(transform.position, pos * getRadius(_simpleAI), Color.yellow);
                        hitTopList = Physics2D.RaycastAll(transform.position, pos, getRadius(_simpleAI), target);

                        for (int hitIndex = 0; i < hitTopList.Length; i++)
                        {
                            if (hitTopList[hitIndex].collider.Equals(targetsInTheRadius[i]))
                            {
                                didItHit = true;
                                break;
                            }
                        }


                        if (!didItHit)
                        {
                            detectionAngle = ((_simpleAI.facingRight) ? getStartAngle(_simpleAI) : getFinalAngle(_simpleAI));
                            pos = new Vector3(Mathf.Cos(detectionAngle * Mathf.Deg2Rad), Mathf.Sin(detectionAngle * Mathf.Deg2Rad), 0f);
                            //Debug.DrawRay(transform.position, pos * getRadius(_simpleAI), Color.yellow);
                            hitTopList = Physics2D.RaycastAll(transform.position, pos, getRadius(_simpleAI), target);

                            for (int hitIndex = 0; i < hitTopList.Length; i++)
                            {
                                if (hitTopList[hitIndex].collider.Equals(targetsInTheRadius[i]))
                                {
                                    didItHit = true;
                                    break;
                                }
                            }
                        }

                    }

                    if (didItHit)
                    {
                        if (!Physics2D.Raycast(transform.position, pos, getRadius(_simpleAI), _simpleAI.gameConfig.obistacleLayer + _simpleAI.gameConfig.groundLayer))
                        {
                            visibleTargets.Add(targetsInTheRadius[i].gameObject);
                        }
                    }

                }

            }

        }




        public override void drawGUI(AI _ai, int actionID, int conditionID)
        {
            EditorGUILayout.Space();

            //TODO: Tradução
            angle = EditorGUILayout.IntSlider("Angle", angle, 1, 360);
            float radiusHelper = EditorGUILayout.FloatField("Radius", (float)radius);
            if (radiusHelper >= 0)
                radius = radiusHelper;
            rotation = EditorGUILayout.Slider("Rotation", (float)rotation, 0, 360);
            target = EditorTools.LayerMaskField("Target list", target);

            EditorGUILayout.Space();
        }

        public override Texture2D BtnIcon()
        {
            return Resources.Load("Icons/2dsimpleai-ai") as Texture2D;
        }

        public override void fillData(JsonData json)
        {
            angle = (int)json["angle"];
            radius = (double)json["radius"];
            rotation = (double)json["rotation"];
            open = (bool)json["open"];
            target.value = (int)json["target"]["value"];
        }

        public override void gizmos(SimpleAI _simpleAI, Transform transform, Color color)
        {
            
            Gizmos.color = color;
            Create2DArc(transform, getRadius(_simpleAI), angle, _simpleAI);
        }


        float getFinalAngle(SimpleAI _simpleAI)
        {
            return (angle / 2) + ((!_simpleAI.facingRight) ? (float)rotation * -1 : (float)rotation);
        }

        float getStartAngle(SimpleAI _simpleAI)
        {
            return (-angle / 2) + ((!_simpleAI.facingRight) ? (float)rotation * -1 : (float)rotation);
        }

        float getMiddleAngle(SimpleAI _simpleAI)
        {
            return 0 - ((!_simpleAI.facingRight) ? (float)rotation * -1 : (float)rotation);
        }

        float getRadius(SimpleAI _simpleAI)
        {
            return ((!_simpleAI.facingRight) ? (float)radius * -1 : (float)radius);
        }

        public Vector3 DirFromAngle(float angleInDegrees, float angleCorrection)
        {
            angleInDegrees += angleCorrection; //Para iniciar a direita
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
        }

        void Create2DArc(Transform transform, float radius, float angle, SimpleAI _simpleAI)
        {
            DrawArc(angle, getRadius(_simpleAI), getStartAngle(_simpleAI), getFinalAngle(_simpleAI), transform);
        }


        void DrawArc(float angle, float radius, float initialAngle, float finalAngle, Transform transform)
        {
            float steps = angle / Mathf.Deg2Rad;
            steps /= 10;

            Vector3 oldPos = transform.position;
            for (int i = 0; i < steps; i++)
            {
                if (finalAngle <= initialAngle)
                {
                    break;
                }
                Vector3 pos = new Vector3(Mathf.Cos(initialAngle * Mathf.Deg2Rad), Mathf.Sin(initialAngle * Mathf.Deg2Rad), 0f) * radius;
                
                Gizmos.DrawLine(oldPos, transform.position + pos);
                oldPos = transform.position + pos;

                initialAngle += Mathf.Deg2Rad * 10;
            }

            if (angle < 360)
            {
                Gizmos.DrawLine(oldPos, transform.position);
            }

        }


    }

}