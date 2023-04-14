using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static WhoWantsToBeAMillionaire.XMLEditor;
using Question = WhoWantsToBeAMillionaire.XMLEditor.Question;

namespace WhoWantsToBeAMillionaire
{
    public partial class WWTBAM : Form
    {
        Random rand = new Random();

        private List<Button> buttons;
        private List<Label> labels;
        private List<int> usedQuestions = new List<int>();
      
        private int questionCounter = 1;
        private int secondsCounter = 0;
        private string date = DateTime.Now.Date.ToShortDateString();

        private Question GetItems(int requestedDifficulty)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
            List<Question> items;

            using (Stream reader = new FileStream("questions.xml", FileMode.Open))
            {
                items = (List<Question>)serializer.Deserialize(reader);
            }

            List<Question> questions = items.FindAll(item => item.difficulty == requestedDifficulty);

            if (questions.Count > 0)
            {
                int randomIndex;
                Question selectedQuestion;
                do
                {
                    randomIndex = rand.Next(0, questions.Count);
                    selectedQuestion = questions[randomIndex];
                }
                while (usedQuestions.Contains(selectedQuestion.questionIndex));

                usedQuestions.Add(selectedQuestion.questionIndex);
                return selectedQuestion;
            }
            
            return null;
        }

        private void ChangeButtons(Question question)
        {
            lblQst.Text = question.questionText;

            HashSet<int> generatedNumbers = new HashSet<int>(); 

            for (int i = 0; i < 4; i++)
            {
                int randomNumber;
                do
                {
                    randomNumber = rand.Next(0, 4);
                }
                while (generatedNumbers.Contains(randomNumber)); 

                generatedNumbers.Add(randomNumber); 
                buttons[randomNumber].Text = question.options[i];
            }
        }

        private void NextLabel()
        {
            labels[questionCounter - 2].Text = labels[questionCounter - 2].Text.Trim(new char[] { '<' });
            labels[questionCounter - 1].Text += " <";
        }
        public WWTBAM()
        {
            InitializeComponent();

            buttons = new List<Button>
            {
                btnAns1,
                btnAns2, 
                btnAns3,
                btnAns4,
            };

            labels = new List<Label>
            {
                lbl1,
                lbl2,
                lbl3,
                lbl4,
                lbl5,
                lbl6,
                lbl7,
                lbl8,
                lbl9,
                lbl10,
                lbl11,
                lbl12,
                lbl13,
                lbl14,
                lbl15,
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XMLEditor program = new XMLEditor();
            program.SetItems();
            ChangeButtons(GetItems(1));
        }

        private void AnsClicked(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            Question question;

            if (questionCounter < 5)
            {
                question = GetItems(1);
            }
            else if (questionCounter > 5 && questionCounter <= 10)
            {
                question = GetItems(2);
            }
            else
            {
                question = GetItems(3);
            }

            questionCounter++;
            ChangeButtons(question);
            NextLabel();
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            secondsCounter++;
            lblDate.Text = $"{date} | {secondsCounter} seconds";
        }
    }
}
