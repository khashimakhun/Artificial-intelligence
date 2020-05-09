using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rapadura;

namespace Rapadura
{
    public class AIMoviment
    {
        public SimpleAI AI;

        public AIMoviment(SimpleAI aiT)
        {
            AI = aiT;
        }

        public virtual void Update() { }

        public virtual void Move(float hAxis)
        {
            if (hAxis < 0)
                hAxis = -1;
            else if (hAxis > 0)
                hAxis = 1;

            flipIfItNeed(hAxis);
        }

        public virtual void Move(float hAxis, float yAxis)
        {
            if (hAxis < 0)
                hAxis = -1;
            else if (hAxis > 0)
                hAxis = 1;

            flipIfItNeed(hAxis);

            if (yAxis < 0)
                yAxis = -1;
            else if (yAxis > 0)
                yAxis = 1;
        }

        public bool verifyGround(Collider2D col)
        {
            Vector2 pos = col.bounds.center;
            pos.y -= col.bounds.extents.y;

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, AI.gameConfig.groundingDistance, AI.gameConfig.groundLayer);
            return hit.collider != null;
        }

        public void flipIfItNeed(float hAxis)
        {
            if (hAxis > 0 && !AI.facingRight)
            {
                AI.facingRight = true;
                AI.transform.localScale = new Vector3(-AI.transform.localScale.x, AI.transform.localScale.y, AI.transform.localScale.z);
            }
            else if (hAxis < 0 && AI.facingRight)
            {
                AI.facingRight = false;
                AI.transform.localScale = new Vector3(-AI.transform.localScale.x, AI.transform.localScale.y, AI.transform.localScale.z);
            }

        }

    }

    public class TerrestrialMoviment : AIMoviment
    {

        public TerrestrialMoviment(SimpleAI aiT) : base(aiT)
        {

        }

        public override void Move(float hAxis)
        {
            base.Move(hAxis);

            Vector2 vel = AI.rBody2d.velocity;
            vel.x = hAxis * AI.movimentSpeed;
            AI.rBody2d.velocity = vel;
        }

        public override void Update()
        {
            base.Update();

            AI.isGrounded = verifyGround(AI.monsterCollider);

        }
    }

    public class FlyingMoviment : AIMoviment
    {

        public FlyingMoviment(SimpleAI aiT) : base(aiT)
        {

        }

        public override void Update()
        {
            base.Update();
        }

        bool canILand()
        {

            return true;
        }

    }


    public class AquaticMoviment : AIMoviment
    {
        public AquaticMoviment(SimpleAI aiT) : base(aiT)
        {

        }

        public override void Update()
        {
            base.Update();
        }
    }

}