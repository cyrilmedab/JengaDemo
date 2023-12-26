namespace JengaDemo
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using TMPro;
    using UnityEngine;

    public class JengaStack : MonoBehaviour
    {
        [SerializeField]
        private string id;

        [SerializeField]
        private TextMeshPro stackDisplayText;

        [SerializeField]
        private List<JengaBlock> listOfBlocks = new();
        [SerializeField]
        private List<BlockData> listOfBlockData = new();

        [SerializeField] 
        const int BLOCKS_IN_ROW = 3;
        [SerializeField]
        const float INSTANTIATION_DELAY = 0.1f;

        [SerializeField]
        private JengaBlock[] blockPrefabs;

        private float _blockHeight;
        private float _blockWidth;

        private void Start()
        {
            
            StartCoroutine(WaitForLoad());
        }

        private IEnumerator WaitForLoad()
        {
            yield return new WaitForSeconds(3f);

            DisplayName();

            PopulateStack();
            AppManager.Instance.stackForGrade.Add(id, this);


        }

        private void DisplayName()
        {
            stackDisplayText.text = id;
        }

        public void TestStack()
        {
            foreach (JengaBlock block in listOfBlocks)
            {
                block.TestBlock();
            }
        }

        #region Stack Creation 

        public void ResetStack()
        {
            ClearStack();
            PopulateStack();
            Debug.Log("Successfully Resetted");
        }

        private void ClearStack()
        {
            foreach (JengaBlock block in listOfBlocks)
            {
                Destroy(block.gameObject);
            }
            listOfBlocks.Clear();
        }

        private void PopulateStack()
        {
            StartCoroutine(PopulateStackCoroutine());
        }

        private IEnumerator PopulateStackCoroutine()
        {
            listOfBlockData = AppManager.Instance.blocksInGrade[id];

            foreach (BlockData blockData in listOfBlockData)
            {
                InstantiateBlock(blockData);
                yield return new WaitForSeconds(INSTANTIATION_DELAY);
            }
        }

        private void InstantiateBlock(BlockData blockData)
        {
            JengaBlock prefab = blockPrefabs[blockData.mastery];
            _blockHeight = prefab.GetHeight();
            _blockWidth = prefab.GetWidth();

            // TODO: Instantiate block
            JengaBlock block = Instantiate(prefab, GetSpawnPosition(), GetSpawnRotation());//, GetSpawnPosition(), GetSpawnRotation());
            block.data = blockData;

            // Associate block with current stack
            block.transform.parent = transform;
            listOfBlocks.Add(block);
        }

        Vector3 GetSpawnPosition()
        {
            

            int row = listOfBlocks.Count / BLOCKS_IN_ROW;
            Vector3 direction = (row % 2 == 0 ? Vector3.right : Vector3.forward);

            return transform.position -
                (direction * _blockWidth + (Vector3.up * (_blockHeight / 2.0f))) +

                (direction * _blockWidth * (listOfBlocks.Count % BLOCKS_IN_ROW)) +

                (Vector3.up * _blockHeight * row);
        }

        Quaternion GetSpawnRotation()
        {
            if ((listOfBlocks.Count / BLOCKS_IN_ROW) % 2 == 0)
                return Quaternion.identity;
            else
                return Quaternion.Euler(0, 90, 0);
        }

        #endregion

    }
}
