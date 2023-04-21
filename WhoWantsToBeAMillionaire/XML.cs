using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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

        /* 
                    questionIndex = #,
                    questionText = "Question",
                    options = new List<string> { "Answer1", "Answer2", "Answer3", "Answer4" },
                    difficulty = #,
                    correctIndex = #  
        */

        public void SetItems()
        {
            List<Question> questionsList = new List<Question>
            {
                new Question
                {
                    questionIndex = 0,
                    questionText = "What was the name of the largest religious center in ancient Sumeria?",
                    options = new List<string> { "Eridu", "Ur", "Lagash", "Kish" },
                    difficulty = 3,
                    correctIndex = 1
                },

                new Question
                {
                    questionIndex = 1,
                    questionText = "What is the name of the chemical process typically used to determine the pH-level of an acid?",
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

                new Question
                {
                    questionIndex = 3,
                    questionText = "What is the capital city of Australia?",
                    options = new List<string> { "Sydney", "Melbourne", "Canberra", "Brisbane" },
                    difficulty = 1,
                    correctIndex = 2
                },

                new Question
                {
                    questionIndex = 4,
                    questionText = "What is the largest planet in our solar system?",
                    options = new List<string> { "Venus", "Saturn", "Jupiter", "Mars" },
                    difficulty = 1,
                    correctIndex = 2
                },

                new Question
                {
                    questionIndex = 5,
                    questionText = "What is the highest mountain in Africa?",
                    options = new List<string> { "Mount Kilimanjaro", "Mount Everest", "Mount Aconcagua", "Mount Denali" },
                    difficulty = 1,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 6,
                    questionText = "What was the first Disney full-length animated feature to be produced in full color?",
                    options = new List<string> { "Sleeping Beauty", "Snow White", "Cinderella", "Fantasia" },
                    difficulty = 2,
                    correctIndex = 3
                },

                new Question
                {
                    questionIndex = 7,
                    questionText = "What is the smallest country in the world?",
                    options = new List<string> { "Vatican City", "Monaco", "San Marino", "Liechtenstein" },
                    difficulty = 1,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 8,
                    questionText = "Which country is both an island and a continent?",
                    options = new List<string> { "Madagascar", "New Zealand", "Indonesia", "Philippines" },
                    difficulty = 1,
                    correctIndex = 1
                },

                new Question
                {
                    questionIndex = 9,
                    questionText = "What is the name of the river that flows through Baghdad?",
                    options = new List<string> { "Nile", "Euphrates", "Tigris", "Jordan" },
                    difficulty = 2,
                    correctIndex = 2
                },

                new Question
                {
                    questionIndex = 10,
                    questionText = "Who painted the famous artwork 'The Starry Night'?",
                    options = new List<string> { "Vincent van Gogh", "Pablo Picasso", "Leonardo da Vinci", "Claude Monet" },
                    difficulty = 2,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 11,
                    questionText = "What is the capital city of Canada?",
                    options = new List<string> { "Toronto", "Ottawa", "Vancouver", "Montreal" },
                    difficulty = 2,
                    correctIndex = 1
                },

                new Question
                {
                    questionIndex = 12,
                    questionText = "Which element has the highest electron affinity?",
                    options = new List<string> { "Fluorine", "Chlorine", "Oxygen", "Sulfur" },
                    difficulty = 3,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 13,
                    questionText = "What is the name of the phenomenon where light is bent as it passes through a medium?",
                    options = new List<string> { "Refraction", "Reflection", "Diffraction", "Dispersion" },
                    difficulty = 3,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 14,
                    questionText = "Which element has the highest melting point?",
                    options = new List<string> { "Tungsten", "Platinum", "Carbon", "Osmium" },
                    difficulty = 2,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 15,
                    questionText = "What is the probability of rolling a sum of 7 with two fair six-sided dice?",
                    options = new List<string> { "1/6", "1/8", "1/12", "1/36" },
                    difficulty = 3,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 16,
                    questionText = "What is the sum of the first 50 positive integers?",
                    options = new List<string> { "1225", "1275", "2500", "2550" },
                    difficulty = 3,
                    correctIndex = 2
                },

                new Question
                {
                    questionIndex = 17,
                    questionText = "Who developed the first successful vaccine in history?",
                    options = new List<string> { "Dr Edward Jenner", "Hilary Koprowski", "Jonas Salk", "World Health Organization" },
                    difficulty = 3,
                    correctIndex = 0
                },

                new Question
                {
                    questionIndex = 18,
                    questionText = "What is the name of the chemical element with the symbol 'I'?",
                    options = new List<string> { "Silicon", "Iridium", "Iron", "Iodine" },
                    difficulty = 2,
                    correctIndex = 3
                },

                new Question
                {
                    questionIndex = 19,
                    questionText = "What is the name of the first woman to win a Nobel Prize?",
                    options = new List<string> { "Rosalind Franklin", "Marie Curie", "Jane Goodall", "Dorothy Hodgkin" },
                    difficulty = 2,
                    correctIndex = 1
                }
            };

            XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
            using (TextWriter writer = new StreamWriter("questions.xml"))
            {
                serializer.Serialize(writer, questionsList);
            }
        }
    }
}
