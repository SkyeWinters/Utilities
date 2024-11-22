using UnityEngine;

namespace Utilities
{
    public class Holster : MonoBehaviour
    {
        [Tooltip("The head that the holster will follow.")]
        [SerializeField] private GameObject _head;
        [Tooltip("The horizontal offset from the head.")]
        [SerializeField] private Vector3 _offset = new(0.18f, -0.69f, 0.15f);

        private void Update()
        {
            RefreshPosition();
        }

        private void RefreshPosition()
        {
            RefreshYPosition();
            RefreshRotationPosition();
        }
        
        private void RefreshYPosition()
        {
            var headPosition = _head.transform.position;
            var holsterPosition = transform.position;
            holsterPosition.y = headPosition.y + _offset.y;
            transform.position = holsterPosition;
        }
        
        private void RefreshRotationPosition()
        {
            var headHorizontalPosition = new Vector3(_head.transform.position.x, 0, _head.transform.position.z);
            var xOffsetDirection = new Vector3(_head.transform.right.x, 0, _head.transform.right.z).normalized;
            var zOffsetDirection = new Vector3(_head.transform.forward.x, 0, _head.transform.forward.z).normalized;
            
            var horizontalOffset = xOffsetDirection * _offset.x + zOffsetDirection * _offset.z;
            var newHorizontalPosition = headHorizontalPosition + horizontalOffset;
            
            var newVerticalPosition = _head.transform.position.y + _offset.y;
            
            transform.position = new Vector3(newHorizontalPosition.x, newVerticalPosition, newHorizontalPosition.z);
        }
    }
}