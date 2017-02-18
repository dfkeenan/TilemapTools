using System;
using System.ComponentModel;
using System.Linq;
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Animations;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Xenko.Rendering.Sprites;

namespace PhysicsSample
{
    /// <summary>
    /// This script will interface the Physics character controller of the entity to move the character around
    /// Will also animate the sprite of the entity, between run and idle.
    /// </summary>
    public class CharacterScript : SyncScript
    {
        [Flags]
        enum PlayerState
        {
            Idle = 0x0,
            Run = 0x1,
            Jump = 0x2
        }

        private const float speed = 10f;

        private CharacterComponent playerController;
        private SpriteComponent playerSprite;


        [DataMember(Mask = LiveScriptingMask)] // keep the value when reloading the script (live-scripting)
        private PlayerState oldState = PlayerState.Idle;

        [DataMember(Mask = LiveScriptingMask)] // keep the value when reloading the script (live-scripting)
        private Vector3 oldDirection = Vector3.Zero;


        //void PlayIdle()
        //{
        //    var sheet = ((SpriteFromSheet)playerSprite.SpriteProvider).Sheet;
        //    SpriteAnimation.Play(playerSprite, sheet.FindImageIndex("idle0"), sheet.FindImageIndex("idle4"), AnimationRepeatMode.LoopInfinite, 7);
        //}

        //void PlayRun()
        //{
        //    var sheet = ((SpriteFromSheet)playerSprite.SpriteProvider).Sheet;
        //    SpriteAnimation.Play(playerSprite, sheet.FindImageIndex("run0"), sheet.FindImageIndex("run4"), AnimationRepeatMode.LoopInfinite, 12);
        //}

        public override void Start()
        {
            playerSprite = Entity.Get<SpriteComponent>();
            playerController = Entity.Get<CharacterComponent>();

            //Please remember that in the GameStudio element the parameter Step Height is extremely important, it not set properly it will cause the entity to snap fast to the ground
            playerController.JumpSpeed = 15.0f;
            playerController.Gravity = -10.0f;
            playerController.FallSpeed = 20.0f;
            playerController.ProcessCollisions = true;

        }

        public override void Update()
        {
            var playerState = PlayerState.Idle;
            var playerDirection = Vector3.Zero;

            // -- Keyboard Inputs

            // Space bar = jump
            if (Input.IsKeyDown(Keys.Space))
            {
                playerState |= PlayerState.Jump;
            }

            // Left - right = run
            if (Input.IsKeyDown(Keys.Right))
            {
                playerState |= PlayerState.Run;
                playerDirection = Vector3.UnitX * speed;
            }
            else if (Input.IsKeyDown(Keys.Left))
            {
                playerState |= PlayerState.Run;
                playerDirection = -Vector3.UnitX * speed;
            }
           
            // -- Logic
          
            // did we start jumping?
            if (playerState.HasFlag(PlayerState.Jump) && !oldState.HasFlag(PlayerState.Jump))
            {
                playerController.Jump();
            }

            // did we just land?
            if (oldState.HasFlag(PlayerState.Jump))
            {
                if (!playerController.IsGrounded)
                {
                    //force set jump flag
                    if (!playerState.HasFlag(PlayerState.Jump))
                    {
                        playerState |= PlayerState.Jump;
                        // Mantain motion 
                        playerDirection = oldDirection;
                    }
                }
                else if (playerController.IsGrounded)
                {
                    //force clear jump flag
                    if (playerState.HasFlag(PlayerState.Jump))
                    {
                        playerState ^= PlayerState.Jump;
                    }
                }
            }

            // did we start running?
            if (playerState.HasFlag(PlayerState.Run) && !oldState.HasFlag(PlayerState.Run))
            {
               // PlayRun();
            }
            // did we stop running?
            else if (!playerState.HasFlag(PlayerState.Run) && oldState.HasFlag(PlayerState.Run))
            {
               // PlayIdle();
            }

            // movement logic
            if (oldDirection != playerDirection)
            {
                playerController.SetVelocity(playerDirection);

                if (playerState.HasFlag(PlayerState.Run))
                {
                    if ((playerDirection.X > 0 && Entity.Transform.Scale.X < 0) ||
                        (playerDirection.X < 0 && Entity.Transform.Scale.X > 0))
                    {
                        Entity.Transform.Scale.X *= -1.0f;
                    }
                }
            }

            // Store current state for next frame
            oldState = playerState;
            oldDirection = playerDirection;
        }
    }
}