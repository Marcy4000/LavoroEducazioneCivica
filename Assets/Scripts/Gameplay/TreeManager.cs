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
        public Vector3 startScale = new Vector3(0.5f, 0.5f, 1f);
        public Vector3 fullScale = new Vector3(1f, 1f, 1f);
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

            Tree treeComponent = newTree.GetComponent<Tree>();
            treeComponent.InitializeTree(tree.treeTypeId, selectedTreeType.growthTime, selectedTreeType.startScale, selectedTreeType.fullScale);

            Debug.Log($"Tree of type {tree} planted!");
        }
        else
        {
            Debug.Log("Invalid tree type or insufficient resources to plant!");
        }
    }
}
