using UnityEngine;

public class ItemsUIControllerCore : MonoBehaviour
{
    [System.Flags]
    public enum ItemsUI
    {
        PlayersInventory,
        OtherInventory,
        PlayerLootsInventory = PlayersInventory | OtherInventory

        // Todo:
        // Персонаж игрока,
        // Компоненты торговли
    }

    [SerializeField]
    protected GameObject _parentUI;
    public RectTransform ParentUI { get => (RectTransform)(_parentUI.transform); }

    [SerializeField]
    protected InventoryUI _playersInventoryUI;

    [SerializeField]
    protected InventoryUI _otherInventoryUI;

    [SerializeField]
    protected GameObjectView _characterView;

    /// <summary>
    /// Кнопка для взятия всех предметов из стороннего инвентаря
    /// </summary>
    [SerializeField]
    protected TakeAllButton _takeAllButton;

    protected bool _isUIOpened;
    public bool IsUIOpened => _isUIOpened;

    protected ItemsUI _openedComponents;

    private IGridSectionInventory _playerInventory;

    public void ToggleUI()
    {
        if (_isUIOpened)
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }

    public virtual void OpenUI()
    {
        // _playersInventoryUI.gameObject.SetActive(true);
        _parentUI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        _isUIOpened = true;
    }

    public virtual void CloseUI()
    {
        // _playersInventoryUI.gameObject.SetActive(false);
        _parentUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        _isUIOpened = false;
    }

    /// <summary>
    /// Помимо собственного инвентаря игрок может просматривать и сторонний. Предполагается,
    /// что в ходе игры таких временных инвентарей может быть много (ящики, схроны, трупы...),
    /// поэтому открытие нового инвентаря с точки зрения пользователей ItemsUIController
    /// тождественно его установке (в отличие от инвентаря игрока, который постоянен).
    /// </summary>
    public void ShowOtherInventory(
        IInventoryInfoProvider inventoryInfo,
        IGridSectionInventory inventory)
    {
        SetOtherInventory(inventoryInfo, inventory);
        Show(ItemsUI.PlayerLootsInventory);
    }

    public void ShowPlayersInventory()
    {
        Show(ItemsUI.PlayersInventory);
    }

    /// <summary>
    /// Устанавливает игрока, информация которого будет отображаться
    /// </summary>
    public void SetPlayer(
        GameObject playerGO,
        IGridSectionInventory inventory,
        IInventoryInfoProvider inventoryInfoProvider)
    {
        _playerInventory = inventory;
        _characterView.SetGameObject(playerGO);

        _playersInventoryUI.SetAsPlayersInventory(inventory, inventoryInfoProvider);
        _takeAllButton.SetRecipient(inventory);
    }

    /// <summary>
    /// Делает видимыми компоненты, соответствующие установленным флагам видимости.
    /// Помимо видимости отдельно взятого компонента имеет значение, показан ли весь интерфейс
    /// </summary>
    private void Show(ItemsUI visibleComponents)
    {
        SetActiveByVisibilityFlag(
            _playersInventoryUI.gameObject,
            visibleComponents,
            ItemsUI.PlayersInventory);
        SetActiveByVisibilityFlag(
            _otherInventoryUI.gameObject,
            visibleComponents,
            ItemsUI.OtherInventory);
    }

    private void SetActiveByVisibilityFlag(
        GameObject go,
        ItemsUI visibleComponents,
        ItemsUI bit)
    {
        bool active = (visibleComponents & bit) == bit;
        // Debug.Log($"Setting active GO {go.name}. {active}");
        go.SetActive(active);
    }

    private void SetOtherInventory(
        IInventoryInfoProvider inventoryInfo,
        IGridSectionInventory inventory)
    {
        _otherInventoryUI.SetAsOtherInventory(_playerInventory, inventory, inventoryInfo);
        _takeAllButton.SetSupplier(inventory);
    }
}
