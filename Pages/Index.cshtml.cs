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
        // Перечисление для определения действия в сессии
        enum SessionActions
        {
            Write,
            Read
        }

        // Переменная для проверки открытой сессии
        const string SessionKeyName = "MySession";

        // Переменная для числа, загаданного игроком
        [BindProperty(Name = "digit")]
        public int PlayerNumber { get; set; }

        // Список экстрасенсов
        public List<PsychicMan> psyList = new List<PsychicMan>()
        {
            new PsychicMan("Петр Иванович"),
            new PsychicMan("Иван Сидорович"),
            new PsychicMan("Сидор Петрович")
        };

        // true - игрок должен загадать число (режим "загадывания")
        // false - игрок должен ввести загаданное число (режим ввода)
        public bool isPlay = true;
        bool isSessionOpen;

        // Список загаданных игроком чисел
        public List<int> playerNumbersList = new List<int>();

        public void OnGet()
        {
            isSessionOpen = string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName));

            // Проверяем, была ли открыта сессия
            if (isSessionOpen)
            {
                // Если сессия не открыта, то открываем ее через ключевую строку
                HttpContext.Session.SetString(SessionKeyName, "Ok");

                // Записываем данные в сессию
                ChangeSession_Psy(psyList, (int)SessionActions.Write);
                HttpContext.Session.Set<List<int>>("playerNumbersList", playerNumbersList);
                HttpContext.Session.Set("isPlay", isPlay);
            }
            else
            {
                // Если сессия уже была открыта, то считываем все данные из нее
                ChangeSession_Psy(psyList, (int)SessionActions.Read);
                playerNumbersList = HttpContext.Session.Get<List<int>>("playerNumbersList");
                isPlay = HttpContext.Session.Get<bool>("isPlay");
            }
        }
        public ActionResult OnPost()
        {
            // Считываем данные из сессии
            ChangeSession_Psy(psyList, (int)SessionActions.Read);
            playerNumbersList = HttpContext.Session.Get<List<int>>("playerNumbersList");
            isPlay = HttpContext.Session.Get<bool>("isPlay");

            // Проверяем режим - "загадывание" или ввод
            if (isPlay)
            {
                // Каждый экстрасенс выбирает свой ответ
                for (int i = 0; i < psyList.Count; i++)
                {
                    psyList[i].SetAnswer(i);
                }

                // Меняем режим на "ввод"
                isPlay = false;
            }
            else
            {
                // Добавляем в список текущее загаданное число 
                playerNumbersList.Add(PlayerNumber);

                // Вычисляем достоверность каждого экстрасенса
                foreach (PsychicMan i in psyList)
                {
                    i.SetPower(playerNumbersList);
                }

                // Меняем режим на "загадывание"
                isPlay = true;
            }

            // Записываем данные об в сессию
            ChangeSession_Psy(psyList, (int)SessionActions.Write);
            HttpContext.Session.Set<List<int>>("playerNumbersList", playerNumbersList);
            HttpContext.Session.Set("isPlay", isPlay);

            // Реализуем паттерн PRG (Post - Redirect - Get), чтобы при обновлении страницы не срабатывал Post 
            return RedirectToPage();
        }

        /// <summary>
        /// Метод для работы с сессией.
        /// </summary>
        /// <param name="_psyList">Список экстрасенсов</param>
        /// <param name="_actionType">Тип действия, 0 - записать в сессию, 1 - считать из сессии</param>
        public void ChangeSession_Psy(List<PsychicMan> _psyList, int _actionType)
        {
            switch (_actionType)
            {
                case 0:
                    foreach (PsychicMan i in _psyList)
                    {
                        HttpContext.Session.Set<PsychicMan>(i.Name, i);
                    }
                    break;
                case 1:
                    for (int i = 0; i < _psyList.Count(); i++)
                    {
                        _psyList[i] = HttpContext.Session.Get<PsychicMan>(_psyList[i].Name);
                    }
                    break;
            }
        }
    }
}
