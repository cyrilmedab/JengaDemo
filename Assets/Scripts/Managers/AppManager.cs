namespace JengaDemo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Networking;

    public class AppManager : MonoBehaviour
    {
        public static AppManager Instance { get; private set; }

        [SerializeField]
        private const string URI = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

        private string json;

        private BlockData[] blockData;

       
        public Dictionary<string, List<BlockData>> blocksInGrade = new();
        public Dictionary<string, JengaStack> stackForGrade = new();

        public bool hasMadeList = false;
        public LinkedList<JengaStack> stacks = new();
        public LinkedListNode<JengaStack> currStack = null;

        [Header("Selected Block Display")]
        private RaycastHit _raycastHit;
        private JengaBlock _currentBlock;
        [SerializeField]
        private TextMeshProUGUI blockDisplay;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(LoadApiData(URI));
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
                RaycastToBlock();
        }

        #region UI

        private void RaycastToBlock()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out _raycastHit, float.MaxValue);

            if (isHit)
            {
                JengaBlock newBlock = _raycastHit.collider.transform.parent.GetComponent<JengaBlock>();
                _currentBlock = newBlock;
                UpdateBlockDisplay(newBlock);
            }
            else
                _currentBlock = null;

            //if (_currentBlock) Debug.Log(_currentBlock.data.standardid);

        }

        private void UpdateBlockDisplay(JengaBlock block)
        {
            if (block == null)
                blockDisplay.text = string.Empty;
            else
                blockDisplay.text = string.Format("{0}: {1}\n - {2}\n {3}\n {4}",
                    block.data.grade, block.data.domain, block.data.cluster, block.data.standardid, block.data.standarddescription);
        }
        #endregion

        #region Loading and Storing Data

        private IEnumerator LoadApiData(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page
                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError("Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError("HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        string jsonToBe = webRequest.downloadHandler.text;
                        json = Utilities.WrapJson(jsonToBe);
                        blockData = Utilities.FromJson<BlockData>(json);

                        Debug.Log("Received: " + webRequest.downloadHandler.text);
                        Debug.Log("Received: " + json);
                        break;
                }
            }

            StoreApiData();
        }

        private void StoreApiData()
        {
            List<BlockData> listBlockData = new List<BlockData>(blockData);
            List<BlockData> sortedList = listBlockData
                .OrderBy(i => i.domain)
                .ThenBy(i => i.cluster)
                .ThenBy(i => i.standardid)
                .ToList();

            foreach (BlockData data in sortedList)
            {
                List<BlockData> list;
                if (blocksInGrade.TryGetValue(data.grade, out list))
                {
                    list.Add(data);
                }
                else
                {
                    blocksInGrade.Add(data.grade, new List<BlockData> { data });
                }
            }

            /*
            foreach (KeyValuePair<string, List<BlockData>> kvp in AppManager.Instance.blocksInGrade)
            {
                foreach (BlockData data in kvp.Value)
                {
                    Debug.Log(data.standarddescription);
                }
                Debug.Log(kvp.Key);
            }
                
            //Debug.Log("Key = {0} + Value = {1}" + kvp.Key + kvp.Value);
            */
        }

        public void StoreLinkedList()
        {
            stacks.AddLast(stackForGrade["6th Grade"]);
            stacks.AddLast(stackForGrade["7th Grade"]);
            stacks.AddLast(stackForGrade["8th Grade"]);
            stacks.AddLast(stackForGrade["Algebra I"]);

            currStack = stacks.First;
            hasMadeList = true;
        }
        #endregion

    }
}

