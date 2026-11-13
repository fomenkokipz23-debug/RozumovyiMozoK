using UnityEngine;

public class ScoreDetectorScript : MonoBehaviour
{
    // Посилання на менеджер логіки гри для додавання очок до загального рахунку
    public LogicScript logic;

    // Метод Start викликається один раз під час ініціалізації об'єкта
    void Start()
    {
        // Автоматично шукаємо на сцені об'єкт з унікальним тегом "Logic",
        // витягуємо з нього компонент LogicScript та записуємо у змінну логіки.
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Вбудований метод Unity, який спрацьовує при перетині тригерної (невидимої) зони
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Перевіряємо, чи має об'єкт, який увійшов у зону тригера, тег "Enemy" (Ворог)
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Якщо це ворог, звертаємося до менеджера логіки та додаємо 1 бал до поточного рахунку
            logic.addScore(1); 
        }
    }
}