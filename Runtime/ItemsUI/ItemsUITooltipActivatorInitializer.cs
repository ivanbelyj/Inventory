using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Система всплывающих подсказок сама по себе независима, поэтому для специфической
/// инициализации (а именно для установки ItemsUI в качестве родительского объекта для
/// всплывающих подсказок) используется отдельный компонент
/// </summary>
public class ItemsUITooltipActivatorInitializer : MonoBehaviour
{
    public void Awake() {
        Initialize();
    }

    public void Initialize() {
        // ItemsUI может быть неактивен в момент поиска
        var itemsUIController = FindObjectOfType<ItemsUIControllerBase>(true);
        GetComponent<TooltipActivator>().Initialize(itemsUIController.ParentUI);
    }
}
