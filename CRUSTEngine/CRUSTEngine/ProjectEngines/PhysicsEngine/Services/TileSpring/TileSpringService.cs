using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs
{
    [Serializable]
    public class TileSpringService : IUpdatableComponent
    {
        public List<List<RigidBody>> Masses;
        public List<List<Spring>> Springs;
        public float SpringConstant { private set; get; }
        public float SpringLength { private set; get; }
        public float SpringFrictionConstant { private set; get; }
        public int NumRows { get; private set; }
        public int NumCols { get; private set; }

        public TileSpringService(float springConstant, float springLength, float springFrictionConstant,
            int numRows, int numCols, RigidType rigidType) 
            //: base(springConstant, springLength, springFrictionConstant, masses, type)
        {
            this.SpringConstant = springConstant;
            this.SpringLength = springLength;
            this.SpringFrictionConstant = springFrictionConstant;
            this.NumRows = numRows;
            this.NumCols = numCols;
            BuildMasses(numRows, numCols, rigidType);
            BuildSprings(this.SpringConstant, this.SpringLength, this.SpringFrictionConstant);
        }

        private void BuildMasses(int numRows, int numCols, RigidType rigidType)
        {
            Vector3 pos = new Vector3(50, 50, 0);
            Masses = new List<List<RigidBody>>();
            for (int i = 0; i < numRows; i++)
            {
                this.Masses.Add(new List<RigidBody>());
                this.Masses[i] = new List<RigidBody>();
                for (int j = 0; j < numCols; j++)
                {
                    pos.X += 30;
                    RigidBody newRigid = GetNewRopeRigid(rigidType, pos);
                    StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(newRigid);
                    this.Masses[i].Add(newRigid);
                }
                pos.X = 50;
                pos.Y += 30;
            }
        }

        private RigidBody GetNewRopeRigid(RigidType rigidType, Vector3 pos)
        {
            switch(rigidType)
            {
                case RigidType.BoxRigid:
                    return DefaultAdder.GetDefaultBox(pos, Material.Wood, new Vector3(5, 5, 0), null, null,
                                                      null);
                    break;
                case RigidType.SphereRigid:
                    return DefaultAdder.GetDefaultSphere(pos, Material.Wood, 5, new Vector3(0, -0f, 0), null, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("rigidType");
            }
        }

        private void BuildSprings(float springConstant,							
                                float springLength,
                                float springFrictionConstant)
        {
            Springs = new List<List<Spring>>();	
            for (var i = 0; i < Masses.Count; ++i)			
            {
                Springs.Add(new List<Spring>());
                for (int j = 0; j < Masses[i].Count; j++)
                {
                    try
                    {
                        Springs[i].Add(new Spring(Masses[i - 1][j], Masses[i][j], springConstant, springLength,
                                                  springFrictionConstant));
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        Springs[i].Add(new Spring(Masses[i][j], Masses[i + 1][j], springConstant, springLength, springFrictionConstant));
                    }
                    catch (Exception)
                    {}
                    try
                    {
                        Springs[i].Add(new Spring(Masses[i][j - 1], Masses[i][j], springConstant, springLength, springFrictionConstant));
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        Springs[i].Add(new Spring(Masses[i][j], Masses[i + 1][j + 1], springConstant, springLength, springFrictionConstant));
                    }
                    catch (Exception)
                    { }                    
                    // Diagonals
                    // Upper Right to Lower Left
                    try
                    {
                        Springs[i].Add(new Spring(Masses[i - 1][j - 1], Masses[i][j], springConstant, springLength, springFrictionConstant));
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        Springs[i].Add(new Spring(Masses[i][j], Masses[i + 1][j + 1], springConstant, springLength, springFrictionConstant));
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateStrings(gameTime);
        }

        public void UpdateStrings(GameTime gameTime)
        {
            for (int i = 0; i < Springs.Count - 1; ++i)
            {
                for (int j = 0; j < Springs[i].Count; j++)
                {
                    Springs[i][j].Update(gameTime);    
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < this.Masses.Count - 1; i++)
            {
                //Vector3 posXna1 = Masses[i].Visual2DPositionCenterEngine;
                //Vector3 posXna2 = Masses[i + 1].Visual2DPositionCenterEngine;
                //Visual2DRotatable visual2DRotatable = new Visual2DRotatable(posXna1, posXna2, 1, TextureType.DefaultBox);
                //visual2DRotatable.Draw(gameTime);
            }
        }
    };
}