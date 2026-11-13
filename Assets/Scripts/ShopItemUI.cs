using UnityEngine;
using UnityEngine.UI; // Обов'язковий простір імен для роботи зі стандартними елементами UI (Image, Button)
using TMPro; // Простір імен для роботи з сучасними текстовими елементами TextMeshPro

public class ShopItemUI : MonoBehaviour
{
    [Header("UI Елементи")]
    // Посилання на UI компонент картинки, де буде відображатися аватарка пташки
    public Image birdImage;       
    
    // Посилання на UI кнопку для вибору/екіпірування пташки
    public Button selectButton;  
    
    // Посилання на текстове поле TextMeshPro для відображення імені пташки
    public TextMeshProUGUI birdNameText; 

    // Приватне посилання на ScriptableObject з даними поточної пташки, яка призначена цій картці
    private BirdData currentBirdData;

    // Публічний метод для початкового налаштування (ініціалізації) картки товару.
    // Викликається з головного менеджера магазину при створенні сітки товарів.
    public void SetupItem(BirdData data)
    {
        // Запам'ятовуємо передані дані пташки у внутрішню змінну
        currentBirdData = data;
        
        // Якщо текстове поле підключене, записуємо туди назву пташки з файлу даних
        if (birdNameText != null)
        {
            birdNameText.text = data.birdName; 
        }

        // Встановлюємо картинку пташки з її файлу даних
        birdImage.sprite = data.birdSprite;
        
        // Гарантовано вмикаємо ігровий об'єкт картинки, щоб він став видимим
        birdImage.gameObject.SetActive(true);

        // Зчитуємо з локальної пам'яті пристрою найкращий рекорд гравця (HighScore)
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Логіка перевірки: чи достатньо рекорду гравця для розблокування цієї пташки
        if (highScore >= data.scoreRequired)
        {
            // Якщо рекорд більший або рівний вимогам:
            selectButton.interactable = true; // Робимо кнопку натискабельною
            birdImage.color = Color.white;    // Повертаємо картинці оригінальні яскраві кольори
        }
        else
        {
            // Якщо гравець ще не набрав потрібної кількості балів:
            selectButton.interactable = false; // Робимо кнопку неактивною (сірою)
            
            // Затемнюємо та робимо напівпрозорою картинку пташки, створюючи ефект "замкненого" товару.
            // Новий колір задається через конструктор Color у форматі RGBA з альфа-каналом (прозорістю) 0.6f.
            birdImage.color = new Color(0.2f, 0.2f, 0.2f, 0.6f);
        }
    }

    // Публічний метод, який прив'язується до події OnClick нашої UI кнопки вибору
    public void OnItemClick()
    {
        // Зберігаємо унікальний ідентифікатор (birdId) поточної пташки в пам'ять пристрою.
        // Завдяки цьому скрипт пташки (Birdscript) під час старту гри зможе дізнатися, який скін увімкнути.
        PlayerPrefs.SetString("SelectedBird", currentBirdData.birdId);
        
        // Виводимо повідомлення про успішний вибір у консоль для розробника
        Debug.Log("Вибрано пташку: " + currentBirdData.birdName);
    }
}