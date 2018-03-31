﻿using Smallgroup.Starport.Assets.Core.Generation;
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
        
        private GameObject _prefab;

        public CreateObjectAction(string prefabPath, Vector3 position, Quaternion rotation)
        {
            PrefabPath = prefabPath;
            Position = position;
            Rotation = rotation;
        }

        public CreateObjectAction(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            _prefab = prefab;
            Position = position;
            Rotation = rotation;
        }

        public override void Invoke(GenerationContext ctx)
        {
            try
            {
                if (PrefabPath != null)
                {
                    _prefab = Resources.Load<GameObject>(PrefabPath);
                }
                var instance = GameObject.Instantiate(_prefab);
                var specialCtx = ctx as Ctx;
                var scale = specialCtx.Map.CellWidth;
                instance.transform.localScale = new Vector3(instance.transform.localScale.x * scale, instance.transform.localScale.y *scale, instance.transform.localScale.z * scale);

           
                instance.transform.localPosition += Position;
                instance.transform.localRotation = Rotation;


                ctx.Ensure("all_generated_objects", new List<GameObject>()).Add(instance);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
