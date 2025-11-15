using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public int price;
    public string description;
    public bool available;
}

[System.Serializable]
public class GetItemsResponse
{
    public int coins;
    public List<Item> items;
}

[System.Serializable]
public class BuyItemResponse
{
    public string status;
    public Item item;
    public int coins;
    public string message;
}

public class UIManager : MonoBehaviour
{
    private const string URL = "http://localhost/store/";

    [SerializeField] private UIDocument _document;
    private VisualElement _root;

    private List<Label> _names;
    private List<Label> _descriptions;
    private List<Label> _availableStatuses;
    private List<Button> _buyButtons;
    private Label _playerCoinsLabel;

    private int _currency;
    private Dictionary<int, Item> _items = new Dictionary<int, Item>();

    private Coroutine _fetchCoroutine;
    private Coroutine _buyCoroutine;

    void Start()
    {
        _root = _document.rootVisualElement;

        _names = _root.Query<Label>(className: "item-name").ToList();
        _descriptions = _root.Query<Label>(className: "item-description").ToList();
        _availableStatuses = _root.Query<Label>(className: "item-available").ToList();
        _buyButtons = _root.Query<Button>(className: "item-button").ToList();

        _playerCoinsLabel = _root.Q<Label>("currency-text");

        for (int i = 0; i < _buyButtons.Count(); i++)
        {
            int itemId = i + 1;

            _buyButtons[i].RegisterCallback<ClickEvent>((ClickEvent evt) =>
            {
                InitBuy(itemId);
            });
        }

        InitFetch();
    }

    private void InitFetch()
    {
        if (_fetchCoroutine != null) StopCoroutine(_fetchCoroutine);
        _fetchCoroutine = StartCoroutine(Fetch());
    }

    private IEnumerator Fetch()
    {
        UnityWebRequest request = UnityWebRequest.Get($"{URL}getItems.php");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log($"Fetch Response: {jsonResponse}");

            GetItemsResponse response = JsonUtility.FromJson<GetItemsResponse>(jsonResponse);

            _currency = response.coins;

            _items.Clear();
            foreach (var item in response.items)
            {
                _items[item.id] = item;
            }

            UpdateUI();
        }
        else
            Debug.LogError(request.error);
    }

    private void InitBuy(int id)
    {
        if (_buyCoroutine != null) StopCoroutine(_buyCoroutine);
        _buyCoroutine = StartCoroutine(Buy(id));
    }

    private IEnumerator Buy(int id)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{URL}item.php?id={id}&coins={_currency}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log($"Buy Response: {jsonResponse}");

            BuyItemResponse response = JsonUtility.FromJson<BuyItemResponse>(jsonResponse);

            if (response.status == "ok")
            {
                Debug.Log($"Compra realizada: {response.item.name}. Moedas restantes: {response.coins}");
                InitFetch();
            }
            else
            {
                Debug.LogError($"Falha na compra: {response.message}");
            }
        }
        else
            Debug.LogError(request.error);
    }

    private void UpdateUI()
    {
        if (_playerCoinsLabel != null)
        {
            _playerCoinsLabel.text = $"Coins: {_currency}";
        }

        for (int i = 0; i < _names.Count; i++)
        {
            int currentItemId = i + 1;

            if (_items.TryGetValue(currentItemId, out Item item))
            {
                _names[i].text = item.name;
                _descriptions[i].text = $"{item.description} (Price: {item.price})";

                if (item.available)
                {
                    _availableStatuses[i].text = "Available";
                    _buyButtons[i].SetEnabled(true);
                }
                else
                {
                    _availableStatuses[i].text = "Not Available";
                    _buyButtons[i].SetEnabled(false);
                }
            }
            else
            {
                _names[i].text = "";
                _descriptions[i].text = "";
                _availableStatuses[i].text = "";
                _buyButtons[i].SetEnabled(false);
            }
        }
    }
}