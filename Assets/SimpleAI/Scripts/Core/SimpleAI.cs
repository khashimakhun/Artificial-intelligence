using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace Rapadura
{
    #region Utils
    public static class AIBase
    {
        public enum MomentAction
        {
            idle, moving, ever, timer, cooldown
        }

        public enum AIConditions
        {
            detection, inTheAir, swimming, grounded, overPlatform, weak, takeDamage
        }

        public enum AIActions
        {
            move, jump, hunt, attack, dropThroughPlatform, heal, summon, custom
        }

        [System.Flags]
        public enum MonsterType
        {
            terrestrial = 1, flying = 2, aquatic = 4
        }

        [System.Flags]
        public enum LoseTargetConditions
        {
            never, maxSight, distanceToSpawnArea, both
        }

        public enum AIType
        {
            nothingSelected, platform, topdown, tactical
        }
    }

    public class AIActionCondition
    {
        public AIBase.AIConditions condition;

    }

    #endregion

    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class SimpleAI : MonoBehaviour
    {

        public GameObject traget;
        public float cooldown;
        public bool fixedTarget = false;
        public AIBase.LoseTargetConditions loseTargetCondition = AIBase.LoseTargetConditions.never;
        public float loseTargetSight = 1f;
        public float maxDistanceFromSpawn = 0.01f;
        public Vector2 spawnPoint;
        bool spawned = false;
        bool hasPrioritariesBehaviours = false;


        public AIBase.MonsterType monsterType = AIBase.MonsterType.terrestrial;


        public SimpleAIConfig gameConfig;


        #region movimentUtils
        public bool facingRight = true;

        public float jumpHeight = 10;
        public bool isGrounded = false;

        public float movimentSpeed = 0.1f;

        public int currentBehaviour;
        public Animator anim;

        #endregion

        #region Physics
        public Collider2D monsterCollider;
        public Rigidbody2D rBody2d;
        #endregion

        #region FlagsEditor
        public bool showAI = true;
        public bool showBehaviour = true;
        public bool showMoviment = true;
        public bool showStatus = true;
        public bool showAnimation = true;
        public bool showGenerals = false;
        #endregion

        public AIMoviment _controller;
        public Language.LangList languageHelper;

        public GameObject target;

        [SerializeField]
        public TextAsset aiJson;
        public AI ai = null;
        public SimpleStatus status;


        public Vector3 targetPosition { get { return m_TargetPosition; } set { m_TargetPosition = value; } }
        [SerializeField]
        private Vector3 m_TargetPosition = new Vector3(1f, 0f, 2f);


        private void Awake()
        {
            if (!gameConfig)
            {
                gameConfig = Resources.Load("Configs/GeneralConfigs") as SimpleAIConfig;
            }

            spawnPoint = transform.position;
            spawned = true;

        }


        // Use this for initialization
        void Start()
        {
            monsterCollider = GetComponent<Collider2D>();
            rBody2d = GetComponent<Rigidbody2D>();
            _controller = new TerrestrialMoviment(this);


            if (aiJson != null)
            {
                ai = new AI(AssetDatabase.GetAssetPath(aiJson));
                ai.load(aiJson);

                for (int i = 0; i < ai.actions.Length; i++)
                {

                    if (!hasPrioritariesBehaviours && ai.actions[i].prioritary)
                        hasPrioritariesBehaviours = true;

                    for (int c = 0; c < ai.actions[i].conditions.Length; c++)
                    {
                        ai.actions[i].conditions[c].start(this, transform);
                    }
                }
            }

        }

        bool acting = false;
        public int actualAction = -1;
        bool isActualActionPrioritary = false;
        Coroutine lastCoroutine;

        IEnumerator PerformingActions(AIAction behaviour)
        {

            for (int i = 0; i < behaviour.actions.Length; i++)
            {
                lastCoroutine = StartCoroutine(behaviour.actions[i].perform(this));
                yield return lastCoroutine;

            }

            actualAction = -2;

            yield return new WaitForSeconds(cooldown);
            actualAction = -1;
            acting = false;
        }


        int selectBehaviour(bool onlyPrioritaries)
        {
            isActualActionPrioritary = onlyPrioritaries;
            int totalChance = 0;
            Dictionary<int, int> listOfPerformingActions = new Dictionary<int, int>();
            bool _hasPrioBeh = false;

            for (int b = 0; b < ai.actions.Length; b++)
            {
                bool canPerform = true;
                if (!ai.actions[b].isAndGroup)
                    canPerform = false;

                for (int c = 0; c < ai.actions[b].conditions.Length; c++)
                {
                    ai.actions[b].conditions[c].update(this, transform);

                    if (ai.actions[b].isAndGroup)
                    {
                        if (!ai.actions[b].conditions[c].isSatisfied())
                        {
                            canPerform = false;
                            break;
                        }
                    }
                    else
                    {
                        if (ai.actions[b].conditions[c].isSatisfied())
                        {
                            canPerform = true;
                            break;
                        }
                    }


                }

                if (canPerform && (!onlyPrioritaries || (onlyPrioritaries && ai.actions[b].prioritary)))
                {
                    if (ai.actions[b].prioritary)
                        _hasPrioBeh = true;

                    if (actualAction != b)
                    {
                        totalChance += ai.actions[b].relevance;
                        listOfPerformingActions[b] = totalChance;
                    }
                }
            }

            //Corrigir os indices para selecionar uma ação prioritária caso tenha uma
            if (hasPrioritariesBehaviours && !onlyPrioritaries && _hasPrioBeh)
            {
                isActualActionPrioritary = true;
                Dictionary<int, int> prioBehHelper = new Dictionary<int, int>();
                totalChance = 0;
                foreach (var actualBehaviour in listOfPerformingActions)
                {
                    if (ai.actions[actualBehaviour.Key].prioritary && actualAction != actualBehaviour.Key)
                    {
                        totalChance += ai.actions[actualBehaviour.Key].relevance;
                        prioBehHelper[actualBehaviour.Key] = totalChance;
                    }
                }

                listOfPerformingActions.Clear();
                listOfPerformingActions = prioBehHelper;
            }


            /*Debug.Log(onlyPrioritaries.ToString() + " <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            foreach (var kvp in listOfPerformingActions)
            {
                Debug.Log("Key = "+ kvp.Key.ToString()+ ", Value = " + kvp.Value.ToString());
            }*/
           

            System.Random rd = new System.Random();
            int randomNumber = rd.Next(1, totalChance + 1);

            int actualVerifier = 1;
            int selectedBehaviour = -1;

            foreach (var actualBehaviour in listOfPerformingActions)
            {
                if (randomNumber >= actualVerifier && randomNumber < actualBehaviour.Value + 1)
                {
                    selectedBehaviour = actualBehaviour.Key;
                    break;
                }
                else
                {
                    actualVerifier = actualBehaviour.Value + 1;
                }
            }

            //Debug.Log(selectedBehaviour.ToString() + " >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            return selectedBehaviour;
        }


        // Update is called once per frame
        void Update()
        {

            //TODO: Verificar por stan ou impossibilitadores de movimento
            _controller.Update();

            //Lose target conditions
            if (target && loseTargetCondition != AIBase.LoseTargetConditions.never)
            {
                if (loseTargetCondition == AIBase.LoseTargetConditions.both)
                {
                    float distanceFromSpawn = Vector2.Distance(transform.position, spawnPoint);
                    float distanceFromEnemy = Vector2.Distance(transform.position, target.transform.position);

                    if (distanceFromSpawn > maxDistanceFromSpawn || distanceFromEnemy > loseTargetSight)
                    {
                        target = null;
                    }

                }
                else if (loseTargetCondition == AIBase.LoseTargetConditions.maxSight)
                {
                    float distanceFromEnemy = Vector2.Distance(transform.position, target.transform.position);

                    if (distanceFromEnemy > loseTargetSight)
                    {
                        target = null;
                    }
                }
                else if (loseTargetCondition == AIBase.LoseTargetConditions.distanceToSpawnArea)
                {
                    float distanceFromSpawn = Vector2.Distance(transform.position, spawnPoint);

                    if (distanceFromSpawn > maxDistanceFromSpawn)
                    {
                        target = null;
                    }
                }

            }


            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>> " + isActualActionPrioritary.ToString() + " <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

            //actualAction == -2 cooldown
            if (ai.actions.Length > 0 && actualAction != -2)
            {
                if (!acting)
                {
                    Debug.Log("1");
                    actualAction = selectBehaviour(false);

                    if (actualAction > -1)
                    {
                        acting = true;
                        StartCoroutine(PerformingActions(ai.actions[actualAction]));
                    }
                }
                else if (hasPrioritariesBehaviours && !isActualActionPrioritary)
                {
                    Debug.Log("2");
                    actualAction = selectBehaviour(true);

                    if (actualAction > -1)
                    {
                        StopCoroutine("PerformingActions");
                        StopCoroutine(lastCoroutine);

                        StartCoroutine(PerformingActions(ai.actions[actualAction]));
                    }
                }

            }

        }


        void OnDrawGizmos()
        {
            if (ai != null)
            {
                for (int i = 0; i < ai.actions.Length; i++)
                {
                    for (int c = 0; c < ai.actions[i].conditions.Length; c++)
                    {

                        ai.actions[i].conditions[c].gizmos(this, transform, ai.actions[i].color);
                    }
                }
            }
            else
            {
                if (aiJson != null)
                {
                    ai = new AI(AssetDatabase.GetAssetPath(aiJson));
                    ai.load(aiJson);
                }
            }

        }


        void OnDrawGizmosSelected()
        {
            if (!monsterCollider)
                monsterCollider = GetComponent<Collider2D>();

            Gizmos.color = Color.green;

            Vector2 pos = monsterCollider.bounds.center;
            pos.y -= monsterCollider.bounds.extents.y;

            Gizmos.DrawLine(pos, pos + (Vector2.down * gameConfig.groundingDistance));

            if ((!spawned || (spawned && target)) && (loseTargetCondition == AIBase.LoseTargetConditions.both || loseTargetCondition == AIBase.LoseTargetConditions.maxSight))
            {
                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, loseTargetSight);
            }

            if (spawned && (loseTargetCondition == AIBase.LoseTargetConditions.both || loseTargetCondition == AIBase.LoseTargetConditions.distanceToSpawnArea))
            {

                float distPercent = ((Vector2.Distance(transform.position, spawnPoint) * 100) / maxDistanceFromSpawn) / 100;
                if (distPercent < 0.5f)
                {
                    Gizmos.color = Color.green;
                }
                else if (distPercent >= 0.5f && distPercent < 0.8f)
                {
                    Gizmos.color = Color.yellow;
                }
                else if (distPercent >= 0.8f && distPercent < 1f)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.black;
                }


                UnityEditor.Handles.Label(spawnPoint, "Spawn");
                Gizmos.DrawLine(transform.position, spawnPoint);
            }

        }


        void drawBehaviourName()
        {

            Vector3 pos = monsterCollider.bounds.center;
            pos.y += monsterCollider.bounds.extents.y + 0.08f;
            pos.x -= monsterCollider.bounds.extents.x;

            Texture2D actionName = new Texture2D(1, 1);
            actionName.SetPixel(0, 0, Color.black);
            actionName.Apply();

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;
            style.fontSize = 15;
            style.normal.background = actionName;

            Handles.Label(pos, "Douglas", style);

        }

    }

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(SimpleAI)), CanEditMultipleObjects]
    public class SimpleAIEditor : Editor
    {

        public SimpleAI _simpleAI;
        string assetPath;

        Texture2D iconMonster;
        Texture2D iconMoviment;
        Texture2D iconSpeed;
        Texture2D iconLanding;
        Texture2D iconConfig;
        Texture2D iconHeart;
        Texture2D iconPlay;
        Texture2D iconAI;



        void OnEnable()
        {
            _simpleAI = (SimpleAI)target;

            if (!_simpleAI.gameConfig)
            {
                _simpleAI.gameConfig = Resources.Load("Configs/GeneralConfigs") as SimpleAIConfig;
            }

            iconMoviment = Resources.Load("Icons/2dsimpleai-moviment") as Texture2D;
            iconSpeed = Resources.Load("Icons/2dsimpleai-speed") as Texture2D;
            iconLanding = Resources.Load("Icons/2dsimpleai-landing") as Texture2D;
            iconConfig = Resources.Load("Icons/2dsimpleai-config") as Texture2D;
            iconHeart = Resources.Load("Icons/2dsimpleai-heart") as Texture2D;
            iconPlay = Resources.Load("Icons/2dsimpleai-play") as Texture2D;
            iconAI = Resources.Load("Icons/2dsimpleai-ai") as Texture2D;
        }



        public override void OnInspectorGUI()
        {

            //DrawDefaultInspector();
            SimpleAILayout.DrawBanner(_simpleAI.gameConfig.selectedLanguage);

            EditorGUILayout.Space();

            //Corrigir
            if (!_simpleAI.aiJson)
            {
                EditorGUILayout.HelpBox("Select a AI asset", MessageType.Warning);
                _simpleAI.aiJson = EditorGUILayout.ObjectField("AI asset", _simpleAI.aiJson, typeof(TextAsset), false) as TextAsset;
            }

            EditorGUILayout.Space();

            if (!_simpleAI.aiJson)
            {
                return;
            }


            if (_simpleAI.ai == null || assetPath != AssetDatabase.GetAssetPath(_simpleAI.aiJson))
            {
                assetPath = AssetDatabase.GetAssetPath(_simpleAI.aiJson);
                _simpleAI.ai = new AI(assetPath);
                _simpleAI.ai.load(_simpleAI.aiJson);
            }


            //DrawButtons
            AIButtons();
            SimpleAILayout.BehaviourButtons(_simpleAI);
            movimentButtons();
            StatusButtons();
            AnimationButtons();

            SimpleAILayout.generalsButtons(_simpleAI);

            EditorUtility.SetDirty(_simpleAI.gameConfig);
        }

        #region AI

        void AIButtons()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();

            //TODO: Tradução
            EditorGUILayout.LabelField(new GUIContent("A.I.", iconAI), EditorStyles.boldLabel);
            if (GUILayout.Button(((_simpleAI.showAI) ? Language.hide(_simpleAI.gameConfig.selectedLanguage) : Language.show(_simpleAI.gameConfig.selectedLanguage))))
            {
                _simpleAI.showAI = !_simpleAI.showAI;
            }
            EditorGUILayout.EndHorizontal();
            if (_simpleAI.showAI)
                showAIArea();
            EditorGUILayout.EndVertical();
        }

        void showAIArea()
        {
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.Space();

            //TODO: Tradução
            _simpleAI.target = (GameObject)EditorGUILayout.ObjectField("Target", _simpleAI.target, typeof(GameObject), true);
            _simpleAI.cooldown = EditorGUILayout.Slider(new GUIContent("Cooldown", "Cooldown between behaviours."), _simpleAI.cooldown, 0.1f, 10);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Actual behaviour");

            GUIStyle behaviourName = EditorTools.BorderLassButton();
            behaviourName.fontSize = 12;

            if (_simpleAI.actualAction > -1)
            {
                behaviourName.normal.textColor = _simpleAI.ai.actions[_simpleAI.actualAction].color;
                GUILayout.Button(_simpleAI.ai.actions[_simpleAI.actualAction].name, behaviourName);
            }
            else if (_simpleAI.actualAction == -1)
            {
                behaviourName.normal.textColor = Color.black;
                GUILayout.Button("------", behaviourName);
            }
            else
            {
                //TODO: Tradurção
                behaviourName.normal.textColor = Color.red;
                GUILayout.Button("Cooldown", behaviourName);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            _simpleAI.fixedTarget = EditorGUILayout.ToggleLeft(new GUIContent("Fixed tagert", "Once it have a target AI. will keep it never changing until it lost the current one."), _simpleAI.fixedTarget);
            _simpleAI.loseTargetCondition = (AIBase.LoseTargetConditions)EditorGUILayout.EnumPopup(new GUIContent("Lose target", "Condition that makes this AI lose a target."), _simpleAI.loseTargetCondition);

            if (_simpleAI.loseTargetCondition == AIBase.LoseTargetConditions.both || _simpleAI.loseTargetCondition == AIBase.LoseTargetConditions.maxSight)
            {
                //TODO: Tradução
                EditorGUILayout.LabelField(new GUIContent("Sight"), EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                float sightRadiusHelper = EditorGUILayout.FloatField("Sight radius", _simpleAI.loseTargetSight);
                if (sightRadiusHelper >= 0)
                {
                    _simpleAI.loseTargetSight = sightRadiusHelper;
                }
                EditorGUILayout.EndVertical();
            }

            if (_simpleAI.loseTargetCondition == AIBase.LoseTargetConditions.both || _simpleAI.loseTargetCondition == AIBase.LoseTargetConditions.distanceToSpawnArea)
            {
                //TODO: Tradução
                EditorGUILayout.LabelField(new GUIContent("Spawn"), EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                float distanceHelper = EditorGUILayout.FloatField("Max. Distance from spwan", _simpleAI.maxDistanceFromSpawn);
                if (distanceHelper >= 0)
                {
                    _simpleAI.maxDistanceFromSpawn = distanceHelper;
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Moviment
        void movimentButtons()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();


            Texture2D icon = Resources.Load("Icons/2dsimpleai-moviment") as Texture2D;
            EditorGUILayout.LabelField(new GUIContent(Language.movimentTitle(_simpleAI.gameConfig.selectedLanguage), icon), EditorStyles.boldLabel);
            if (GUILayout.Button(((_simpleAI.showMoviment) ? Language.hide(_simpleAI.gameConfig.selectedLanguage) : Language.show(_simpleAI.gameConfig.selectedLanguage))))
            {
                _simpleAI.showMoviment = !_simpleAI.showMoviment;
            }
            EditorGUILayout.EndHorizontal();
            if (_simpleAI.showMoviment)
                showMovimentArea();
            EditorGUILayout.EndVertical();
        }

        void showMovimentArea()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.Space();



            switch (_simpleAI.gameConfig.AIType)
            {
                case AIBase.AIType.platform:
                    {
                        MovimentDrawPlatform();
                        break;
                    }
                case AIBase.AIType.topdown:
                    {
                        MovimentDrawTopdown();
                        break;
                    }
                case AIBase.AIType.tactical:
                    {
                        MovimentDrawTactical();
                        break;
                    }
                default:
                    {
                        EditorGUILayout.HelpBox(Language.alertGameMovimentType(_simpleAI.gameConfig.selectedLanguage), MessageType.Warning);
                        EditorGUILayout.Space();
                        break;
                    }
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        void MovimentDrawPlatform()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(Language.platformSpritLookingLeft(_simpleAI.gameConfig.selectedLanguage) + " \n <--"))
            {
                _simpleAI.facingRight = false;

            }

            iconMonster = Resources.Load("Icons/" + ((_simpleAI.facingRight) ? "monster" : "monsterLeft")) as Texture2D;
            if (iconMonster)
            {
                Rect r;
                r = GUILayoutUtility.GetRect(iconMonster.width, iconMonster.height);
                EditorGUI.DrawTextureTransparent(r, iconMonster);
            }

            if (GUILayout.Button(Language.platformSpritLookingRight(_simpleAI.gameConfig.selectedLanguage) + " \n -->"))
            {
                _simpleAI.facingRight = true;
            }

            EditorGUILayout.EndHorizontal();


            EditorTools.DrawDivision();


            _simpleAI.monsterType = (AIBase.MonsterType)EditorGUILayout.EnumMaskField(new GUIContent(Language.movimentTypes(_simpleAI.gameConfig.selectedLanguage), iconMoviment, "which kind of movement or movements your AI can perform."), _simpleAI.monsterType);

            _simpleAI.movimentSpeed = EditorGUILayout.Slider(new GUIContent(Language.movimentSpeed(_simpleAI.gameConfig.selectedLanguage), iconSpeed, "How fast your monster is going to move."), _simpleAI.movimentSpeed, 0.01f, 100);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent(Language.isGrounded(_simpleAI.gameConfig.selectedLanguage), iconLanding));

            GUIStyle ButtonColor = EditorTools.BorderLassButton();

            string btnTxt;
            if (_simpleAI.isGrounded)
            {
                btnTxt = "YES";
                ButtonColor.normal.textColor = Color.blue;
            }
            else
            {
                btnTxt = "NO";
                ButtonColor.normal.textColor = Color.red;
            }

            GUILayout.Button(btnTxt, ButtonColor);
            EditorGUILayout.EndHorizontal();
        }

        void MovimentDrawTopdown()
        {
            EditorGUILayout.HelpBox(Language.alertNotImplementedYet(_simpleAI.gameConfig.selectedLanguage), MessageType.Warning);
            EditorGUILayout.Space();
        }

        void MovimentDrawTactical()
        {
            EditorGUILayout.HelpBox(Language.alertNotImplementedYet(_simpleAI.gameConfig.selectedLanguage), MessageType.Warning);
            EditorGUILayout.Space();
        }

        #endregion

        #region Status

        void StatusButtons()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(Language.statusTitle(_simpleAI.gameConfig.selectedLanguage), iconHeart), EditorStyles.boldLabel);
            if (GUILayout.Button(((_simpleAI.showStatus) ? Language.hide(_simpleAI.gameConfig.selectedLanguage) : Language.show(_simpleAI.gameConfig.selectedLanguage))))
            {
                _simpleAI.showStatus = !_simpleAI.showStatus;
            }
            EditorGUILayout.EndHorizontal();
            if (_simpleAI.showStatus)
                showStatusArea();
            EditorGUILayout.EndVertical();
        }

        void showStatusArea()
        {
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginVertical("Box");

            //AI.status.health = EditorGUILayout.IntField("Health", AI.status.health);

            EditorGUILayout.Space();

            /*float percent = ((AI.status.actualHealth * 100) / AI.status.health) / 100f;
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), percent, AI.status.actualHealth.ToString() + "/" + AI.status.health.ToString());*/

            //EditorTools.DrawProgress(EditorGUILayout.GetControlRect(), percent);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Animation

        void AnimationButtons()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();

            //TODO: Animation
            EditorGUILayout.LabelField(new GUIContent("Animation", iconPlay), EditorStyles.boldLabel);
            if (GUILayout.Button(((_simpleAI.showAnimation) ? Language.hide(_simpleAI.gameConfig.selectedLanguage) : Language.show(_simpleAI.gameConfig.selectedLanguage))))
            {
                _simpleAI.showAnimation = !_simpleAI.showAnimation;
            }
            EditorGUILayout.EndHorizontal();
            if (_simpleAI.showAnimation)
                showAnimationArea();
            EditorGUILayout.EndVertical();
        }

        void showAnimationArea()
        {
            EditorGUILayout.BeginVertical("Box");

            //TODO: Tradução
            EditorGUILayout.LabelField("Animation controller", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.Space();

            _simpleAI.anim = (Animator)EditorGUILayout.ObjectField(_simpleAI.anim, typeof(Animator), true);

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        #endregion

    }

#endif
    #endregion
}