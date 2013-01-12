using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SHMUP
{
    public enum ParticleEffect
    {
        pbExplosion,
        enemyExplosion,
        enemyDamage,
        pbDamage,
        seekTrail,
        enemyTeleport,
        missileBar,
        enemyBreach,
        missileSplosion,
        pickUp
    }
    class Particles
    {
            // This is the original X,Y placement of the particle. 
            //This is used for drawing the particle and seeing if the particle belongs 
            //to a specific emitter in this tutorial.

            public Vector2 initPos;
            public Vector2 position;

            // This is used to move the particle up, down, 
            //and to the sides, with relevance to xyorg.
            public Vector2 velocity;

            // Checks whether or not the particle is in use.
            //This is so we can reuse old particles rather than waste memory creating
            //and deleting new and old particles.
            public bool used = false;

            // The type. In this sample there will only be two types of
            // particles, but you can program in your own if you want.
            public ParticleEffect particleEffectType;

            // This determines how long to particle will live before it
            // is no longer used.
            public int lifespan;

            public int halfLife;
            public Vector2 aimFrom;

            public int timeAlive;

            public float rot;

            public float rotspeed;

            public Color color;

            public Vector2 size;
            public float growSpeed;

    }
}
