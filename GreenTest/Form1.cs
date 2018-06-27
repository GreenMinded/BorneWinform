
using GreenTest.WebService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Timer timer = new Timer();


                timer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
                timer.Interval = (500) * (1);              // Timer will tick every second
                timer.Enabled = true;                       // Enable the timer
                timer.Start();                              // Start the timer

                label8.Text = String.Empty;


            // Change la question
            void timer_Tick(object sender, EventArgs e)
            {
                label8.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                ListQuestions.Clear();
                AccessStation();
                //label1.Text = String.Empty;
                //label2.Text = String.Empty;
                //label3.Text = String.Empty;
                //label6.Text = String.Empty;
                //label7.Text = String.Empty;
                NotifyPropertyChanged("ListQuestions");
                if ( ListQuestions == null)
                {
                    label1.Text = "";
                    label2.Text = "";
                    label3.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                } else { 
                label1.Text = Question;
                label2.Text = Response_left;
                label3.Text = Response_right;
                label6.Text = CountLeft;
                label7.Text = CountRight;
                
                }
            }


        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public List<SPI_QuestionAnswer_Result> ListQuestions { get => listQuestions; set { listQuestions = value; NotifyPropertyChanged("ListQuestions"); } } // Ajout d'un OnPropertyChange
        public IList<SPI_QuestionAnswer_Result> ListQuestionAnswer { get => listQuestionAnswer; set => listQuestionAnswer = value; }
        public IList<stations> ListStation { get => listStation; set => listStation = value; }

        private List<SPI_QuestionAnswer_Result> listQuestions = new List<SPI_QuestionAnswer_Result>();
        private IList<SPI_QuestionAnswer_Result> listQuestionAnswer;
        private IList<stations> listStation;


        // Les properties bindé au label du Form1.cs[Design]
        #region Properties Question
        public string _Question;
        public string Question
        {
            get
            {
                if (_Question == null && ListQuestions != null) // Null exception à gérrer
                {
                    _Question = listQuestions[0].label;
                }
                return _Question;
            }
        }

        public string _Response_left;
        public string Response_left
        {
            get
            {
                if (_Response_left == null && ListQuestions != null)
                {
                    _Response_left = listQuestions[0].response_left;
                }
                return _Response_left;
            }
        }

        public string _Response_right;
        public string Response_right
        {
            get
            {
                if (_Response_right == null && ListQuestions != null) 
                {
                    _Response_right = listQuestions[0].response_right;
                }
                return _Response_right;
            }
        }

        public string _CountLeft;
        public string CountLeft
        {
            get
            {
                if (_CountLeft == null && ListQuestions != null)
                {
                    _CountLeft = listQuestions[0].count_left.Value.ToString();
                }
                return _CountLeft;
            }
        }
        public string _CountRight;
        public string CountRight
        {
            get
            {
                if (_CountRight == null && ListQuestions != null)
                {
                    _CountRight = listQuestions[0].count_right.Value.ToString();
                }
                return _CountRight;
            }
        }

        #endregion


        // Ajout d'une vérification pour les questions d'une borne en particulier, si l'id de la station et égal à station_id de la table questions, alors charger sa liste de questions.
        public void AccessStation()
        {
            
            using (Service1Client api = new Service1Client())
            {
                listQuestionAnswer = api.GetQuestionAnswer();
            }
            using (Service1Client api = new Service1Client())
            {
                ListStation = api.GetStation();
            }
            //foreach (stations sta in ListStation)
            //{
            //    if (sta.mac_address == GetMacAddress().ToString())
            //    {
            if(listQuestionAnswer != null) {
                foreach (SPI_QuestionAnswer_Result queans in listQuestionAnswer)
                {

                    if (DateTime.Now >= queans.date_start && DateTime.Now <= queans.date_end)
                    {
                        ListQuestions.Add(queans);
                    }
                }
               }else {

                return;
               }

                //    }
                //}
        }

        public static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }




    }

}

