using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psychic
{
    /// <summary>
    /// Класс экстрасенса
    /// </summary>
    public class PsychicMan
    {
        // Имя экстрасенса
        public string Name { get; set; }
        // Достоверность экстрасенса
        public int Power { get; set; }
        // Количество правильных ответов экстрасенса
        public int CorrectAnswers { get; set; }
        // Текущий ответ экстрасенса
        public int Answer { get; set; }
        // Список ответов экстрасенса
        public List<int> AnswersList { get; set; } = new List<int>();
        public PsychicMan()
        {
        }
        public PsychicMan(string _name)
        {
            Name = _name;
            Power = 0;
        }

        /// <summary>
        /// Выбирает один из видов ответов для экстрасенса.
        /// 0 - диапазон от 10 до 99;
        /// 1 - диапазон от 10 до 49;
        /// 2 - диапазон от 50 до 99.
        /// </summary>
        /// <param name="_counter"></param>
        public void SetAnswer(int _counter)
        {
            Random rnd = new Random();
            int answer = _counter switch
            {
                0 => rnd.Next(10, 100),
                1 => rnd.Next(10, 50),
                2 => rnd.Next(50, 100),
                _ => 0,
            };
            AnswersList.Add(answer);
            Answer = answer;
        }

        /// <summary>
        /// Расчет достоверности экстрасенса
        /// </summary>
        /// <param name="_playerAnswers">Список загаданных игроком чисел</param>
        public void SetPower(List<int> _playerAnswers)
        {
            if(Answer == _playerAnswers.Last())
            {
                CorrectAnswers++;
            }
            Power = (int)(((double)CorrectAnswers / _playerAnswers.Count()) * 100);
        }
    }
}
