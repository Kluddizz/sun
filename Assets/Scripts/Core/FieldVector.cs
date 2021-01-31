using UnityEngine;

namespace Core
{
    public class FieldVector
    {
        public float strength { get; set; }
        public Vector3 position { get; set; }
        public Vector3 direction { get; set; }

        public FieldVector(Vector3 position, Vector3 direction, float strength)
        {
            this.position = position;
            this.direction = direction;
            this.strength = strength;
        }
    }
}