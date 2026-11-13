using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicScript : MonoBehaviour
{
    // Текстове поле (Legacy UI) для відображення прогресу проходження рівня у відсотках
    public Text progressText;
    
    // Ігровий об'єкт (панель) екрану програшу (Game Over)
    public GameObject gameOverScreen;
    
    // Загальна тривалість рівня в секундах (за замовчуванням 60 секунд)
    public float levelDuration = 60f;
    
    // Внутрішній лічильник поточного часу, що минув від старту рівня
    private float currentTime = 0f;
    
    // Прапорець стану гри (true — гра триває, false — завершилась програшем або перемогою)
    private bool isGameActive = true;
    
    // Поточний рахунок гравця (кількість набраних балів)
    public int playerScore;

    // Посилання на ScriptableObject конфігурації рівня
    public LevelConfig config;

    // Текстовий компонент TextMeshPro для красивого відображення поточного рахунку на екрані
    public TextMeshProUGUI scoreText;

    [Header("Game Over UI")]
    // Текстові поля TextMeshPro на екрані програшу для відображення фінального та найкращого результатів
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverBestText;
    
    // Внутрішня змінна для збереження найкращого рекорду гравця
    private int bestScore;

    // Метод Start викликається один раз під час ініціалізації гри
    void Start()
    {
        // Завантажуємо з локальної пам'яті пристрою найкращий рекорд за ключем "HighScore".
        // Якщо гра запускається вперше і запису немає, повертається значення за замовчуванням: 0.
        bestScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Метод Update викликається кожного кадру гри
    void Update()
    {
        // Якщо ігровий процес зараз активний
        if (isGameActive)
        {
            // Накопичуємо поточний час за допомогою Time.deltaTime
            currentTime += Time.deltaTime;
            
            // Розраховуємо відсоток прогресу рівня (поточний час ділимо на загальну тривалість і множимо на 100)
            // Функція Mathf.Clamp обмежує значення строго в діапазоні від 0 до 100, щоб уникнути багів
            float progress = Mathf.Clamp((currentTime / levelDuration) * 100f, 0, 100);

            // Якщо текстовий компонент підключений — виводимо відсотки. 
            // Формат "F0" округлює число з плаваючою крапкою до цілого числа (без знаків після коми)
            if (progressText != null)
                progressText.text = progress.ToString("F0") + "%";

            // Якщо прогрес досяг 100%, викликаємо метод перемоги у рівні
            if (progress >= 100f) WinLevel();
        }
    }

    // Публічний метод для додавання балів (викликається тригерами труб або ворогів)
    public void addScore(int scoreToAdd)
    {
        // Збільшуємо поточний рахунок на вказану кількість балів
        playerScore += scoreToAdd;
        
        // Оновлюємо текст рахунку на ігровому екрані, переводячи число в рядок (ToString)
        scoreText.text = playerScore.ToString();
    }

    // Публічний метод, який запускає логіку програшу гри (викликається зі скрипту пташки при зіткненні)
    public void gameOver()
    {
        // Перевіряємо, чи гра ще активна, щоб уникнути подвійного виклику логіки кінця гри
        if (isGameActive)
        {
            // Вимикаємо активність гри
            isGameActive = false;

            // Якщо поточний рахунок перевищив старий рекорд
            if (playerScore > bestScore)
            {
                // Оновлюємо значення рекорду
                bestScore = playerScore;
                
                // Зберігаємо новий рекорд у пам'ять пристрою за допомогою PlayerPrefs
                PlayerPrefs.SetInt("HighScore", bestScore);
                
                // Примусово записуємо дані на диск (внутрішнє сховище пристрою)
                PlayerPrefs.Save();
            }

            // Виводимо фінальний рахунок на екран програшу
            if (gameOverScoreText != null)
                gameOverScoreText.text = playerScore.ToString();

            // Виводимо найкращий рекорд на екран програшу
            if (gameOverBestText != null)
                gameOverBestText.text = bestScore.ToString();

            // Активуємо (робимо видимою) панель Game Over UI
            gameOverScreen.SetActive(true);
            
            // Зупиняємо час у грі, що заморожує рух усіх об'єктів та фізику
            Time.timeScale = 0;
            
            // Лог у консоль (через биті символи у твоєму файлі тут був текст на кшталт "Гра завершена!")
            Debug.Log(" !  .");
        }
    }

    // Публічний метод для кнопки перезапуску гри
    public void restartGame()
    {
        // Повертаємо швидкість плину часу в нормальний стан (1 = 100%)
        Time.timeScale = 1;
        
        // Перезавантажуємо поточну активну сцену за її ім'ям через SceneManager
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Приватний метод для логіки успішного проходження рівня (перемоги)
    void WinLevel()
    {
        isGameActive = false; // Зупиняємо активність гри
        Time.timeScale = 0;   // Повністю зупиняємо час
        
        // Виводимо напис про успішне завершення рівня
        if (progressText != null) progressText.text = "100% - !";
    }

    // Публічний метод для кнопки виходу в головне меню
    public void MoveToMenu()
    {
        // Обов'язково повертаємо нормальний плин часу перед зміною сцени
        Time.timeScale = 1;
        
        // Завантажуємо сцену головного меню за її назвою "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }
}