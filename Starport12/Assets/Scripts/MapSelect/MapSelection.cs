using Smallgroup.Starport.Assets.Scripts;
using Smallgroup.Starport.Assets.Surface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Smallgroup.Starport.Assets.Scripts.MapSelect
{

    public class MapSelection : MonoBehaviour {


        public int x, y, width, height;

        public WorldAnchor World;
        public SelectorViz Viz;

   

        //public bool[][] ValidArray

        private bool _wasValid;
        private int _oldWidth = 0, _oldHeight = 0;

        public SelectionResult GetResult()
        {
            var result = new SelectionResult();
            result.X = x;
            result.Y = y;
            result.Width = width;
            result.Height = height;
            result.OverlappingObjects = Viz.CollidingWith.Distinct().ToList();
            result.World = World;
            return result;
        }

        //public MapSelection Clone()
        //{
        //    var n = new MapSelection();
        //    n.World = World;
        //    n.x = x;
        //    n.y = y;
        //    n.width = width;
        //    n.height = height;
        //    n.Viz = Viz;
        //    return n;
        //}

        public List<GridXY> GetCoordinates(int padding=0)
        {
            var output = new List<GridXY>();
            for (var i = x - padding; i <= x + width + padding; i++)
            {
                for (var j = y - padding; j <= y + height + padding; j++)
                {
                    output.Add(new GridXY(i, j));
                }
            }
            return output;
        }

        public List<GridXY> GetRelativeCoordinates()
        {
            var output = new List<GridXY>();
            for (var i = 0; i <= width ; i++)
            {
                for (var j =0; j <=  height; j++)
                {
                    output.Add(new GridXY(i, j));
                }
            }
            return output;
        }

        private void OnEnable()
        {
            _oldHeight = -1;
            _oldWidth = -1;
        }

        // Use this for initialization
        void Start () {
            Viz = Instantiate(Viz, transform);

            gameObject.SetActive(false);
	    }
	
	    // Update is called once per frame
	    void Update () {

            if (World == null)
            {
                return;
            }

            if (Viz == null)
            {
                return;
            }

            //width = Mathf.Max(width, 1);
            //height = Mathf.Max(height, 1);

            if ( (_oldHeight != height && _oldWidth != width)
                || (_oldHeight != height)
                || (_oldWidth != width)
                )
            {

                Viz.UpdateBoxes(this);
                _oldHeight = height;
                _oldWidth = width;
            }
      


            var topLeft = World.Map.TransformCoordinateToWorld(new GridXY(x, y));
            var lowRight = World.Map.TransformCoordinateToWorld(new GridXY(x + width, y + height));

            var center = (topLeft + lowRight) / 2;
            //center += World.Map.CellWidth * new Vector3(.5f, 0, .5f);
            var scale = lowRight - topLeft;
            Viz.transform.localScale = new Vector3(Mathf.Max(1, scale.x + 1) - .01f, 1.2f, Mathf.Max(1, scale.z + 1) - .01f);
            Viz.transform.position = center;
            Viz.transform.rotation = Quaternion.identity;


	    }
    }
}

