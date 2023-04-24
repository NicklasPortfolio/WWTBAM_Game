using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Xml.Serialization;
using Question = WhoWantsToBeAMillionaire.XMLEditor.Question;

namespace WhoWantsToBeAMillionaire
{
    public partial class WWTBAM : Form
    {
        // random
        Random rand = new Random();

        // listor
        private List<Button> buttons;
        private List<Label> labels;
        private List<int> usedQuestions = new List<int>();
      
        // ints
        private int questionCounter = 14;
        private int secondsCounter = 0;
        private int animCounter = 0;
        private int colorSwitch = 0;
        private int iterator = 0;

        // datum
        private string date = DateTime.Now.Date.ToShortDateString();


        // Metoden GetItems, returnerar en instans av klassen Question utifrån den tillfrågade svårighetsgraden
        private Question GetItems(int requestedDifficulty)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
            List<Question> items;

            using (Stream reader = new FileStream("questions.xml", FileMode.Open))
            {
                items = (List<Question>)serializer.Deserialize(reader);
            }

            List<Question> questions = items.FindAll(item => item.difficulty == requestedDifficulty);

            // Ser till att metoden returnerar en fråga som inte har returnerats förrut.
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
            return null; // All codepaths return a value
        }

        // ChangeButtons ändrar texten på knapparna man svarar med samt den label som innehåller frågan.
        private void ChangeButtons(Question question)
        {
            lblQst.Text = question.questionText;

            // Ett HashSet är som en lista, fast den kan inte innehålla dubletter av föremål.
            HashSet<int> generatedNumbers = new HashSet<int>(); 

            // Ser till så att de olika svarsalternativen placeras ut på knapparna slumpmässigt.
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
                if (question.correctIndex == i)
                {
                    buttons[randomNumber].Tag = "correct";
                }
            }
        }

        // Flyttar pekaren till nästa label.
        private void NextLabel()
        {
            labels[questionCounter - 2].Text = labels[questionCounter - 2].Text.Trim(new char[] { '<' });
            labels[questionCounter - 1].Text += " <";
        }

        // Gör det enklare att spela ljud och musik.
        private void PlayAudio(string fileName, bool loop)
        {
            SoundPlayer audio = new SoundPlayer(fileName);

            if (loop)
            {
                audio.PlayLooping();
            }
            else
            {
                audio.Play();
            }
        }
        public WWTBAM()
        {
            InitializeComponent();

            // Det är lättare att hantera många objekt som ett stort objekt genom listor.
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
            XMLEditor program = new XMLEditor(); // Init XMLEditor klassen
            program.SetItems(); // Init SetItems metoden i XMLEditor.
            ChangeButtons(GetItems(1)); // Generera den första frågan.
            PlayAudio(@"Audio\To1000.wav", true);
        }

        // Hanterar knapptryck
        private void AnsButtonClicked(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            Question question;

            button.BackColor = Color.Gold;
            button.Update();

            PlayAudio(@"Audio\FinalAnswer.wav", false);
            System.Threading.Thread.Sleep(4000);

            // Typ hela den här tolkar bara vad som händer beroende på vilken fråga man är.
            if (button.Tag as string == "correct")
            {
                button.BackColor = Color.ForestGreen;
                button.Update();

                if (questionCounter < 15) 
                {
                    PlayAudio(@"Audio\Correct.wav", false);
                    System.Threading.Thread.Sleep(4000);
                    questionCounter++;

                    if (questionCounter < 5) // Lätt
                    {
                        question = GetItems(1);
                        PlayAudio(@"Audio\To1000.wav", true);
                    }
                    else if (questionCounter > 4 && questionCounter < 10) // Medium
                    {
                        question = GetItems(2);
                        PlayAudio(@"Audio\To32000.wav", true);
                    }
                    else // Svår
                    {
                        question = GetItems(3);
                        PlayAudio(@"Audio\To1M.wav", true);
                    }

                    button.Tag = null;
                    ChangeButtons(question);
                    NextLabel();
                }

                else // Om man vinner
                {
                    lblQst.Text = null;
                    lblWin.Visible = true;

                    foreach (Button buttonObject in buttons)
                    {
                        buttonObject.Text = null;
                        buttonObject.Enabled = false;
                    }

                    foreach (Label labelObject in labels)
                    {
                        labelObject.ForeColor = Color.ForestGreen;
                    }

                    PlayAudio(@"Audio\Win.wav", false);
                    timerWin.Start();
                    timerLabels.Start();
                }
            }

            else // Om man förlorar
            {
                button.BackColor = Color.Firebrick;
                button.Update();

                PlayAudio(@"Audio\Incorrect.wav", false);
                System.Threading.Thread.Sleep(4100);

                MessageBox.Show($"You lost! See you next time!", "You lost!");
                this.Close();
            }

            button.BackColor = Color.MidnightBlue;
            button.Update();

            foreach (Button buttonObject in buttons)
            {
                buttonObject.Enabled = true;
            }
        }

        // Hanterar 50/50 livlinan
        private void btn5050_Click(object sender, EventArgs e)
        {
            List<Button> tobeDisabled = new List<Button>();

            foreach (Button button in buttons)
            {
                if (tobeDisabled.Count == 2)
                {
                    break;
                }
                else if ((string)button.Tag != "correct")
                {
                    tobeDisabled.Add(button);
                }
            }

            foreach (Button button in tobeDisabled)
            {
                button.Enabled = false;

            }
            SoundPlayer sound = new SoundPlayer(@"Audio\5050Sound.wav");
            sound.Play();

            btn5050.Enabled = false;
            btn5050.BackgroundImage = null;
        }

        // Hanterar timern i hörnet
        private void timerTime_Tick(object sender, EventArgs e)
        {
            secondsCounter++;
            lblDate.Text = $"{date} | {secondsCounter} seconds";
        }

        // Win animationen
        private void timerWin_Tick(object sender, EventArgs e)
        {
            if (animCounter < 68) // Använder ternary switch för rolig (statement ? om true : om false)
            {
                lblWin.ForeColor = (colorSwitch == 0) ? Color.White : Color.Gold;
                buttons[0].BackColor = (colorSwitch == 0) ? Color.Gold : Color.White;
                buttons[1].BackColor = (colorSwitch == 0) ? Color.White : Color.Gold;
                buttons[2].BackColor = (colorSwitch == 0) ? Color.White : Color.Gold;
                buttons[3].BackColor = (colorSwitch == 0) ? Color.Gold : Color.White;
                colorSwitch = 1 - colorSwitch;
            }

            else if (animCounter == 72)
            {
                timerWin.Stop();
                MessageBox.Show($"Congratulations! You've won the million!", "You won!");
                this.Close();
            }

            else
            {
                if (iterator < buttons.Count)
                {
                    buttons[iterator].BackColor = Color.ForestGreen;
                    iterator++;
                }
            }
            animCounter++;
        }

        Color[] colors = { Color.Gold, Color.ForestGreen };
        int colorIndex = 0;

        // Labels animationen
        private void timerLabels_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].ForeColor = colors[colorIndex];
                colorIndex = (colorIndex + 1) % colors.Length;
            }

            if (animCounter == 72)
            {
                timerLabels.Stop();
            }
        }
    }
}
