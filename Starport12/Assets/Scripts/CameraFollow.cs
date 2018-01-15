using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform[] Following;
        public Camera Camera;

        public Vector3 Offset = new Vector3(0, 15, 0);

        [Header("Coefs")]
        public float AttractionCoef = .01f;
        public float FrictionCoef = .2f;
        public float Mass = 1.0f;

        private Vector3 _velocity;

        private void Update()
        {

            var target = GetTarget();
            var current = transform.position;

            var diff = target - current;

            var attractionForce = diff * AttractionCoef;
            var frictionForce = -_velocity * FrictionCoef;

            var acceleration = (attractionForce + frictionForce) / Mass;
            _velocity += acceleration;
            transform.position = transform.position + _velocity;

            Camera.transform.position = transform.position + Offset ;
        }


        public Vector3 GetTarget()
        {
            var sum = Following.Aggregate(Vector3.zero, (agg, curr) => agg + curr.position);
            var mag = sum.magnitude;
            var center = sum / Following.Length;


            return center;
        }

    }
}
