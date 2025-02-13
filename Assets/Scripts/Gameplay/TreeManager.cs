using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [System.Serializable]
    public class TreeType
    {
        public int id;
        public GameObject prefab;
        public float growthTime; // Growth time in seconds
        public Vector3 startScale = new Vector3(0.3f, 0.3f, 0.8f);
        public Vector3 fullScale = new Vector3(1f, 1f, 1f);
        public float sizeVariation = 0.5f; // Controls how much the size can vary
    }

    public List<TreeType> treeTypes = new List<TreeType>();
    public Transform treeParent;

    public void PlantTree(Vector3 position, ShopItemData tree)
    {
        if (tree.type != ShopItemType.TreeSeed)
        {
            return;
        }

        TreeType selectedTreeType = treeTypes.Find(t => t.id == tree.treeTypeId);

        if (selectedTreeType != null && InventoryManager.Instance.RemoveItem(tree))
        {
            GameObject newTree = Instantiate(selectedTreeType.prefab, treeParent);
            position.y = 0f;
            newTree.transform.localPosition = position;

            // Generate random size multiplier
            float randomSize = Random.Range(1f - selectedTreeType.sizeVariation, 1f + selectedTreeType.sizeVariation);
            Vector3 randomizedStartScale = selectedTreeType.startScale * randomSize;
            Vector3 randomizedFullScale = selectedTreeType.fullScale * randomSize;

            TreeObject treeComponent = newTree.GetComponent<TreeObject>();
            treeComponent.InitializeTree(tree.treeTypeId, selectedTreeType.growthTime, randomizedStartScale, randomizedFullScale);

            Debug.Log($"Tree of type {tree} planted with size multiplier: {randomSize}");
        }
        else
        {
            Debug.Log("Invalid tree type or insufficient resources to plant!");
        }
    }
}
