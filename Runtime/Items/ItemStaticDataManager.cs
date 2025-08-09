using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mirror;

public class ItemStaticDataManager : MonoBehaviour
{
    // Загружаемые из bundled asset предметы могут спавниться
    private NetworkManager _netManager;
    public string bundleName = "itemdata";
    
    private Dictionary<string, ItemStaticData> _namesAndData;
    public Dictionary<string, ItemStaticData> NamesAndData => _namesAndData;

    public ItemStaticData GetStaticDataByName(string name) => _namesAndData[name];

    private void Awake()
    {
        _netManager = FindAnyObjectByType<NetworkManager>();
    }

    private void Start() {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(
            System.IO.Path.Combine(Application.streamingAssetsPath, bundleName)
        );

        if (localAssetBundle is null) {
            Debug.LogError($"Failed to load {bundleName} AssetBundle");
            return;
        }

        ItemStaticData[] assets = localAssetBundle.LoadAllAssets<ItemStaticData>();
        CreateItemDataDictionary(assets);
        // Debug.Log("ItemData dictionary is initialized");
        foreach (var pair in _namesAndData) {
            string typeName = pair.Value.GetType().Name;
            // Debug.Log($"\t{pair.Key}: {pair.Value}. ItemData type: " + typeName);
        }

        localAssetBundle.Unload(false);
    }

    private void CreateItemDataDictionary(ItemStaticData[] arr) {
        _namesAndData = new Dictionary<string, ItemStaticData>();
        foreach (ItemStaticData itemData in arr) {
            _namesAndData.Add(itemData.name, itemData);

            // Item item = itemData.Item;
            _netManager.spawnPrefabs.Add(itemData.ItemPrefab);
        }
    }
}
