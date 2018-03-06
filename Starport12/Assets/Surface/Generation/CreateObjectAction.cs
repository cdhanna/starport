using Smallgroup.Starport.Assets.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Surface.Generation
{
    public class CreateObjectAction : GenerationAction
    {
        public string PrefabPath { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public CreateObjectAction(string prefabPath, Vector3 position, Quaternion rotation)
        {
            PrefabPath = prefabPath;
            Position = position;
            Rotation = rotation;
        }

        public override void Invoke(GenerationContext ctx)
        {
            try
            {
                var prefab = Resources.Load<GameObject>(PrefabPath);
                var instance = GameObject.Instantiate(prefab);
                instance.transform.localPosition += Position;
                instance.transform.localRotation = Rotation;
                var scale = World.Map.CellWidth;

                instance.transform.localScale = new Vector3(instance.transform.localScale.x * scale, instance.transform.localScale.y, instance.transform.localScale.z * scale);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
