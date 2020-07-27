using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Psychic.Pages
{
    public class IndexModel : PageModel
    {
        // Переменная для проверки открытой сессии
        const string SessionKeyName = "MySession";

        // Переменная для числа, загаданного игроком
        [BindProperty(Name = "digit")]
        public int playerNumber { get; set; }

        // Три экстрасенса
        public PsychicMan psy1, psy2, psy3;

        // true - игрок должен загадать число (режим "загадывания")
        // false - игрок должен ввести загаданное число (режим ввода)
        public bool isPlay = true;

        // Список загаданных игроком чисел
        public List<int> playerNumbersList;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            
        }

        public void OnGet()
        {
            // Проверяем, была ли открыта сессия
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                // Если сессия не открыта, то открываем ее через ключевую строку
                HttpContext.Session.SetString(SessionKeyName, "Ok");

                // Отображаем данные об экстрасенсах по умолчанию
                psy1 = new PsychicMan("Петр Иванович");
                psy2 = new PsychicMan("Иван Сидорович");
                psy3 = new PsychicMan("Сидор Петрович");

                // Создаем список загаданных игроком чисел
                playerNumbersList = new List<int>();

                // Записываем данные об экстрасенсах в сессию
                WriteInSession_Psy(psy1, psy2, psy3);

                // Записываем список загаданных игроком чисел в сессию
                HttpContext.Session.Set<List<int>>("playerNumbersList", playerNumbersList);

                // Устанавливаем режим "загадывания" и записываем его в сессию
                isPlay = true;
                HttpContext.Session.Set("isPlay", isPlay);
            }
            else
            {
                // Если сессия уже была открыта, то считываем все данные из нее
                WriteFromSession_Psy(ref psy1, ref psy2, ref psy3);
                playerNumbersList = HttpContext.Session.Get<List<int>>("playerNumbersList");
                isPlay = HttpContext.Session.Get<bool>("isPlay");
            }
        }
        public ActionResult OnPost()
        {
            // Считываем данные из сессии об экстрасенсах
            WriteFromSession_Psy(ref psy1, ref psy2, ref psy3);

            // Проверяем режим - "загадывание" или ввод
            if (HttpContext.Session.Get<bool>("isPlay"))
            {
                // Каждый экстрасенс выбирает свой ответ
                psy1.SetAnswer(0);
                psy2.SetAnswer(1);
                psy3.SetAnswer(2);

                // Меняем режим на "ввод"
                isPlay = false;
               
            }
            else
            {
                // Считываем список загаданных игроком чисел из сессии
                playerNumbersList = HttpContext.Session.Get<List<int>>("playerNumbersList");

                // Добавляем в список текущее загаданное число 
                playerNumbersList.Add(playerNumber);

                // Вычисляем достоверность каждого экстрасенса
                psy1.SetPower(playerNumbersList);
                psy2.SetPower(playerNumbersList);
                psy3.SetPower(playerNumbersList);

                // Меняем режим на "загадывание"
                isPlay = true;

                // Записываем список загаданных игроком чисел в сессию
                HttpContext.Session.Set<List<int>>("playerNumbersList", playerNumbersList);
            }

            // Записываем данные об экстрасенсах в сессию
            WriteInSession_Psy(psy1, psy2, psy3);

            // Записываем текущий режим игры в сессию
            HttpContext.Session.Set("isPlay", isPlay);

            // Реализуем паттерн PRG (Post - Redirect - Get), чтобы при обновлении страницы не срабатывал Post 
            return RedirectToPage();
        }

        /// <summary>
        /// Записать данные об экстрасенсах из сессии
        /// </summary>
        /// <param name="_psy1">Первый экстрасенс</param>
        /// <param name="_psy2">Второй экстрасенс</param>
        /// <param name="_psy3">Третий экстрасенс</param>
        public void WriteFromSession_Psy(ref PsychicMan _psy1, ref PsychicMan _psy2, ref PsychicMan _psy3)
        {
            _psy1 = HttpContext.Session.Get<PsychicMan>("psy1");
            _psy2 = HttpContext.Session.Get<PsychicMan>("psy2");
            _psy3 = HttpContext.Session.Get<PsychicMan>("psy3");
        }

        /// <summary>
        /// Записать данные об экстрасенсах в сессию
        /// </summary>
        /// <param name="_psy1">Первый экстрасенс</param>
        /// <param name="_psy2">Второй экстрасенс</param>
        /// <param name="_psy3">Третий экстрасенс</param>
        public void WriteInSession_Psy(PsychicMan _psy1, PsychicMan _psy2, PsychicMan _psy3)
        {
            HttpContext.Session.Set<PsychicMan>("psy1", _psy1);
            HttpContext.Session.Set<PsychicMan>("psy2", _psy2);
            HttpContext.Session.Set<PsychicMan>("psy3", _psy3);
        }
    }
}
