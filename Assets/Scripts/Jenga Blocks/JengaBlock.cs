namespace JengaDemo
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class JengaBlock : MonoBehaviour
    {
        [SerializeField]
        private GameObject jengaBlock;

        public MasteryMaterial masteryMaterial;

        public BlockData data;

        [HideInInspector]
        public MeshRenderer meshRenderer;

        [HideInInspector]
        public BoxCollider boxCollider;

        [HideInInspector]
        public Material material;

        [HideInInspector]
        public Rigidbody rigidBody;



        // Start is called before the first frame update
        private void Awake()
        {
            
            boxCollider = jengaBlock.GetComponentInChildren<BoxCollider>();
            material = meshRenderer.material;
            rigidBody = boxCollider.GetComponentInChildren<Rigidbody>();
        }

        public void EnablePhysics(bool state)
        {
            boxCollider.enabled = state;
            rigidBody.isKinematic = !state;
            meshRenderer.enabled = state;
        }

        public float GetHeight()
        {
            meshRenderer = jengaBlock.GetComponentInChildren<MeshRenderer>();
            return meshRenderer.bounds.extents.y * 2;
        }

        public float GetWidth()
        {
            meshRenderer = jengaBlock.GetComponentInChildren<MeshRenderer>();
            return meshRenderer.bounds.extents.x * 2 + 0.1f;
        }
    }
}
