using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.HelperModules;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket.ParticleEngine
{
    [Serializable]
    public class ParticleEngineCore
    {
        private Random _random;
        public Vector2 EmitterLocation { get; set; }
        public List<Particle> _particles;
        
        private List<TextureType> _textureTypes;

        public bool IsExploding { set; get; }
        private int timeToVanish = 0;
        private int minTTL;
        private int maxNextTTL;
        public int MaxParticles;
        private Vector2 VelocityVec;

        public ParticleEngineCore(Vector2 location, int minTTL, int maxNextTTL, int maxParticles)
        {
            InitializeTextures();
            this.minTTL = minTTL;
            this.maxNextTTL = maxNextTTL;
            this.EmitterLocation = location;
            this._particles = new List<Particle>();
            this._random = new Random();
            this.MaxParticles = maxParticles;
        }

        private void InitializeTextures()
        {
            _textureTypes = new List<TextureType>();
            _textureTypes.Add(TextureType.CirclePE);
            _textureTypes.Add(TextureType.DiamondPE);
            _textureTypes.Add(TextureType.StarPE);
        }

        public void Update(Vector2 velocityVec)
        {
            this.VelocityVec = velocityVec;
            if (IsExploding)
            {
                timeToVanish++;
            }
            if (timeToVanish <= 75)
            {
                for (int i = 0; i < this.MaxParticles; i++)
                {
                    int ttl = minTTL + _random.Next(maxNextTTL);
                    _particles.Add(GenerateNewParticle(ttl));
                    if (IsExploding)
                    {
                        _particles[_particles.Count - 1].Velocity *= velocityVec;
                    }
                }
            }

            for (int particle = 0; particle < _particles.Count; particle++)
            {
                _particles[particle].Update();
                if (_particles[particle].TTL <= 0)
                {
                    _particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle(int ttl)
        {
            TextureType textureType = _textureTypes[_random.Next(_textureTypes.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(_random.NextDouble() ),
                                    1f * (float)(_random.NextDouble() ));
            velocity *= this.VelocityVec;
            float angle = 0;
            float angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
            Color color = new Color(
                        (float)_random.NextDouble(),
                        (float)_random.NextDouble(),
                        (float)_random.NextDouble());
            float size = (float)_random.NextDouble();


            return new Particle(textureType, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(GameTime gameTime)
        {
            //if (IsExploding)
            //{
            //    for (int index = 0; index < _particles.Count; index++)
            //    {
            //        _particles[index].Color = Color.Red;
            //    }
            //}
            StaticData.EngineManager.Game1.SpriteBatch.Begin();
            for (int index = 0; index < _particles.Count; index++)
            {
                _particles[index].Draw(gameTime);
            }
            StaticData.EngineManager.Game1.SpriteBatch.End();
        }
    }
}