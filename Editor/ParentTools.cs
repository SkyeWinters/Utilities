using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utilities.Editor
{
    public class ParentTools : UnityEditor.Editor
    {
        [MenuItem("GameObject/Custom/Parent Tools/Center", priority = 0)]
        public static void AlignParentToCenterOfChildren()
        {
            foreach (var transform in Selection.transforms.Where(x => x.childCount > 0))
            {
                var childrenPositions = new Vector3[transform.childCount];
                
                var bounds = new Bounds(transform.GetChild(0).position, Vector3.zero);
                for (var index = 0; index < transform.childCount; index++)
                {
                    var child = transform.GetChild(index);
                    bounds.Encapsulate(child.position);
                    childrenPositions[index] = child.position;
                }

                var center = bounds.center;
                transform.position = center;
                
                for (var index = 0; index < transform.childCount; index++)
                {
                    transform.GetChild(index).position = childrenPositions[index];
                }
            }
        }
    }
}