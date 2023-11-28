using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIlevelsManager : MonoBehaviour
{
    [SerializeField] private AllLevels allLevels;
    [SerializeField] private int batchSize = 6;
    [SerializeField] private GameObject BatchPrefab;
    
    [SerializeField] private List<GameObject> batches;
    [SerializeField] private List<UILevelElement> levelElements;
    private int currentBatchIndex = 0;

    public void Start()
    {
        GameData currentGameData = SaveSystem.LoadData();

        for (int i = 0; i < currentGameData.levels.Count; i++)
        {
            LevelData currentLevelData = currentGameData.levels[i];
            levelElements[i].SetButtonState(currentLevelData.isUnlocked);
            levelElements[i].SetPerfectState(currentLevelData.didPerfect);
        }
    } 
    
#if  UNITY_EDITOR
    
    [Button]
    public void SpawnLevelBatches()
    {
        int amountOfLevels = allLevels.levels.Count;
        int amountOfBatches = (amountOfLevels / batchSize) + (amountOfLevels % batchSize > 0 ? 1 : 0);
        int currentLevelIndex = 0;

        if (batches.Count > 0)
        {
            foreach (GameObject batchGO in batches)
            {
                DestroyImmediate(batchGO);
            }    
        }
        
        batches = new List<GameObject>();
        levelElements = new List<UILevelElement>();

        for (int i = 0; i < amountOfBatches; i++)
        {
            //GameObject batchGO = Instantiate(BatchPrefab, BatchPrefab.transform.position, Quaternion.identity, this.transform);
            GameObject batchGO = PrefabUtility.InstantiatePrefab(BatchPrefab) as GameObject;
            batchGO.transform.SetParent(this.transform);
            batchGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            
            UILevelsBatch batch = batchGO.GetComponent<UILevelsBatch>();
            bool isLastBatch = (i == amountOfBatches - 1);

            foreach (UILevelElement levelElement in batch.levelElements)
            {
                if (currentLevelIndex < amountOfLevels)
                {
                    levelElement.SetLevel(allLevels.levels[currentLevelIndex]);
                    levelElements.Add(levelElement);
                    currentLevelIndex++;
                }
                else
                {
                    levelElement.gameObject.name = "nonActive";
                    levelElement.gameObject.SetActive(false);
                }
            }

            if (i == 0)
            {
                batch.leftArrow.gameObject.SetActive(false);
            }

            if (isLastBatch)
            {
                batch.rightArrow.gameObject.SetActive(false);
            }
            
            batches.Add(batchGO);
        }
        
        ShowCurrentBatch();
    }
#endif

    public void NextBatch()
    {
        currentBatchIndex++;
        ShowCurrentBatch();
    }

    public void PreviousBatch()
    {
        currentBatchIndex--;
        ShowCurrentBatch();
    }

    public void ShowCurrentBatch()
    {
        for (int i = 0; i < batches.Count; i++)
        {
            if (i == currentBatchIndex)
            {
                batches[i].SetActive(true);
            }
            else
            {
                batches[i].SetActive(false);
            }
        }
    }
}