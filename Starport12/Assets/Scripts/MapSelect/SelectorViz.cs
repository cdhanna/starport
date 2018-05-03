using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.MapSelect
{


    public class SelectorViz : MonoBehaviour
    {

        public Color ValidColor = new Color(.25f, .5f, .8f, .7f);
        public Color InValidColor = new Color(.8f, .5f, .25f, .7f);

        private bool _anyValid;
        private bool _wasValid;

        private Material _material;

        public List<GameObject> CollidingWith { get; set; } = new List<GameObject>();

        private GameObject _colliderObject;
        private List<GameObject> _vizBoxes = new List<GameObject>();

        public IsCellValidFunc Validator;

        public void SetValid(bool valid)
        {
            if (_wasValid != valid || !_anyValid)
            {
                _anyValid = true;
                _wasValid = valid;
                if (valid)
                {
                    SetIsValid();
                }
                else
                {
                    SetIsNotValid();
                }
            }
        }

        public void UseValidator(IsCellValidFunc validator)
        {
            Validator = validator;
        }

        public void UpdateBoxes(MapSelection selection)
        {
            _vizBoxes.ForEach(v => DestroyImmediate(v));
            _vizBoxes.Clear();

            //var width = Mathf.Max(1, selection.width);
            //var height = Mathf.Max(1, selection.height);
            var width = selection.width + 1;
            var height = selection.height + 1;


            var w = (1f / width);
            var h = (1f / height);

            var startX = w / 2 + ((-w * width) / 2);
            var startY = h / 2 + ((-h * height) / 2);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    box.transform.SetParent(transform, false);

                    DestroyImmediate(box.GetComponent<BoxCollider>());

                    box.transform.localScale = new Vector3(w, 2f, h);
                    box.transform.localPosition = new Vector3(startX + x * w, 0, startY + y * h);

                    var render = box.GetComponent<MeshRenderer>();
                    render.material = _material;
                    if (Validator == null)
                    {
                        render.material.color = InValidColor;

                    }
                    else
                    {


                        var cellValid = Validator(selection, x, y);
                        if (cellValid)
                        {
                            render.material.color = ValidColor;
                        }
                        else
                        {
                            render.material.color = InValidColor;
                        }
                    }

                    _vizBoxes.Add(box);
                }
            }

        }


        private void OnEnable()
        {
            CollidingWith.Clear();
        }

        private void SetIsValid()
        {
            //EnsureRenderer();

            //_vizBoxes.ForEach(v => v.GetComponent<MeshRenderer>().material.color = ValidColor);

            //_renderer.material.color = ValidColor;

        }

        private void SetIsNotValid()
        {
            //EnsureRenderer();
            //_renderer.material.color = InValidColor;
            //_vizBoxes.ForEach(v => v.GetComponent<MeshRenderer>().material.color = InValidColor);

        }

        // Use this for initialization
        void Start()
        {
            //_renderer = GetComponentInChildren<MeshRenderer>();


        }

        // Update is called once per frame
        void Update()
        {
            if (_colliderObject == null)
            {
                _colliderObject = transform.GetChild(0).gameObject;
                var renderer = _colliderObject.GetComponent<MeshRenderer>();
                _material = new Material(renderer.material);
                Destroy(renderer);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            CollidingWith.Add(collider.gameObject);
        }
        private void OnTriggerExit(Collider collider)
        {
            CollidingWith.Remove(collider.gameObject);
        }
    }

    public delegate bool IsCellValidFunc(MapSelection selection, int relativeX, int relativeY);

}