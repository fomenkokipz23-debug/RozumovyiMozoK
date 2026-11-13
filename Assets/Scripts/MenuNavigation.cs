using UnityEngine;
// Обов'язковий простір імен для роботи з менеджером сцен в Unity
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    // Публічний метод для кнопки "ГРАТИ" в головному меню.
    // Має бути public, щоб його можна було прив'язати до UI кнопки в інспекторі.
    public void PlayGame()
    {
        // Завантажує ігрову сцену за її текстовим ім'ям
        SceneManager.LoadScene("GameScene");
    }

    // Нове: Для кнопки "МАГАЗИН" в головному меню
    public void OpenShop()
    {
        // Переводить гравця на сцену магазину (наприклад, для вибору скінів пташок)
        SceneManager.LoadScene("ShopScene"); 
    }

    // Нове: Для кнопки-стрілочки в магазині (повернення назад)
    public void BackToMenu()
    {
        // Завантажує сцену головного меню
        SceneManager.LoadScene("MainMenu");
    }

    // Для кнопки "ВИХІД" в головному меню
    public void ExitGame()
    {
        // Виводить повідомлення в консоль редактора Unity, 
        // оскільки метод Application.Quit() не закриває гру всередині самого редактора
        Debug.Log("Гра закривається!"); 
        
        // Повністю закриває скомпільовану гру (працює на ПК, Android чи iOS)
        Application.Quit();
    }
}