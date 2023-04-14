using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static WhoWantsToBeAMillionaire.WWTBAM;

namespace WhoWantsToBeAMillionaire
{
    public class XMLEditor
    {
        public class Question
        {
        public int questionIndex { get; set; }
        public string questionText { get; set; }
        public List<string> options { get; set; }
        public int difficulty { get; set; } // 1 = easy, 2 = medium, 3 = hard
        public int correctIndex { get; set; }
        }

        public void AddItems()
        {
            List<Question> questionsList = new List<Question>
            {
                new Question
                {
                    questionIndex = 0,
                    questionText = "What was the name of the religious center in ancient Sumeria?",
                    options = new List<string> { "Eridu", "Ur", "Lagash", "Kish" },
                    difficulty = 1,
                    correctIndex = 1
                },

                new Question
                {
                    questionIndex = 1,
                    questionText = "What is the name of the chemical process used to determine the pH-level of an acid?",
                    options = new List<string> { "Titration", "Electrolysis", "Oxidation", "Sedimentation" },
                    difficulty = 2,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 2,
                    questionText = "Who won the nobel prize in physics the year 1922?",
                    options = new List<string> { "Albert Einstein", "Max Planck", "Philipp Lenard", "Niels Bohr" },
                    difficulty = 2,
                    correctIndex = 3
                },
            };

            XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
            using (TextWriter writer = new StreamWriter("questions.xml"))
            {
                serializer.Serialize(writer, questionsList);
            }
        }
    }
}
