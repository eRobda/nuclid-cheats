using System;

namespace our_first_menu
{
    struct Vector3
    {
        public float x,y, z;

        public Vector3(float x = 0f, float y = 0f, float z = 0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // operator overloads
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
        }
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }
        public static Vector3 operator *(Vector3 vec, float factor)
        {
            return new Vector3(vec.x * factor, vec.y * factor, vec.z * factor); 
        }
        public static Vector3 operator /(Vector3 vec, float factor)
        {
            return new Vector3(vec.x / factor, vec.y / factor, vec.z / factor);
        }

        public Vector3 ToAngle()
        {
            return new Vector3(
                (float)((float)Math.Atan2(-z, Hypot(x,y)) * (180f / Math.PI)),
                (float)(Math.Atan2(y,x) * (180f / Math.PI)),
                0f
            );
        }

        public static float Hypot(float x, float y)
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        public bool IsZero()
        {
            return x == 0 && y == 0 && z == 0;
        }

        public static Vector3 CalculateAngle(Vector3 localPosition, Vector3 enemyPosition, Vector3 viewAngles)
        {
            return ((enemyPosition - localPosition).ToAngle() - viewAngles);
        }
    }
}
