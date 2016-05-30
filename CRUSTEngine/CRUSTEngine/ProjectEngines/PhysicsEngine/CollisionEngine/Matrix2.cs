using System;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.PhysicsEngine
{
    [Serializable]
    static class Matrix2
    {
        public static Vector3 Mul_Matrix_vector(Matrix a, Vector3 b)
        {
            return new Vector3(0, 0, a.M33 * b.Z);
        }

        public static Vector3 M_V(Vector3 x, float angle)
        {
            Vector3 temp = new Vector3(0, 0, 0);

            temp.X = (float)Math.Cos(angle) * x.X - (float)Math.Sin(angle) * x.Y;
            temp.Y = (float)Math.Sin(angle) * x.X + (float)Math.Cos(angle) * x.Y;
            temp.Z = 0;

            return temp;
        }

        public static Vector3 transform(Matrix a, Vector3 v)
        {
            return new Vector3(a.M11 * v.X + a.M12 * v.Y, a.M21 * v.X + a.M22 * v.Y, a.M33 * v.Z);
        }

        public static Vector3 transformTranspose(Matrix aa, Vector3 v)
        {
            Matrix a = Matrix.Transpose(aa);
            return new Vector3(a.M11 * v.X + a.M12 * v.Y, a.M21 * v.X + a.M22 * v.Y, a.M33 * v.Z);
        }

        public static Matrix setSkewSymmetric(Vector3 v)
        {
            return new Matrix(0, -v.Z, v.Y, 0, v.Z, 0, -v.X, 0, -v.Y, v.X, 0, 0, 0, 0, 0, 1);
        }
    }
}
