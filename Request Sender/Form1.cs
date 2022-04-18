using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;

namespace Request_Sender
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// text boxes names values
        /// </summary>
        ///
        
       private TextBox[] textBoxesNamesVaritables = new TextBox[4];
        /// <summary>
        /// text boxes values items
        /// </summary>
       private TextBox[] textBoxesValuesVaritables = new TextBox[4];

        public Form1()
        {
            
            InitializeComponent();
            int iOne = 0;
            int iTwo = 0;
            Text = Application.ProductName;
            // find TextBoxes
            foreach (Control control in Controls)
            {
                bool isTextBox = control is TextBox;
                if (isTextBox)
                {
if (control.Name.Contains("Value"))
                {
                        textBoxesValuesVaritables[iOne] = (TextBox)control;
                        iOne++;
                }

                    if (control.Name.Contains("Name"))
                    {
                        textBoxesNamesVaritables[iTwo] = (TextBox)control;
                        iTwo++;
                    }
                }
              
            }

        }


        private void buttonSendingForm_Click(object sender, EventArgs e)
        {

            if (TargetURLPole.Text.Length == 0)
            {
                MessageBox.Show("Target URL null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (boxRequestTypeList.SelectedIndex == -1 ) {
                MessageBox.Show("Not selected request type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

          
            int u = 0;

            for (int i = 0; i < textBoxesValuesVaritables.Length; i++)
            {
                if (textBoxesValuesVaritables[i].Text.Length == 0)
                {
                    u++;
                }
            }

            if (u == textBoxesValuesVaritables.Length)
            {
                MessageBox.Show("Values varitables is null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            u = 0;

            for (int i = 0; i < textBoxesNamesVaritables.Length; i++)
            {
                if (textBoxesNamesVaritables[i].Text.Length == 0)
                {
                    u++;
                }
            }

            if (u == textBoxesNamesVaritables.Length)
            {
                MessageBox.Show("Names varitables is null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (textBoxesNamesVaritables[i].Text.Length > 0)
                {
                    if (textBoxesValuesVaritables[i].Text.Length == 0)
                    {
                        MessageBox.Show("Not currrent form!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (textBoxesValuesVaritables[i].Text.Length > 0)
                {
                    if (textBoxesNamesVaritables[i].Text.Length == 0)
                    {
                        MessageBox.Show("Not currrent form!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            // check type response
            

            if (boxRequestTypeList.SelectedIndex == 0)
            {
                // POST
                CallPostAsync();
            }

            if (boxRequestTypeList.SelectedIndex == 1)
            {
                // GET
               GETRequest();
            }
        }

        private  void POSTRequest()
        {
            // ini form
            RequestForm form = new RequestForm();
            for (int i = 0; i < 4; i++)
            {
                if (textBoxesNamesVaritables[i].Text.Length > 0)
                {
                    if (textBoxesValuesVaritables[i].Text.Length > 0)
                    {
                        // generating form data
                        RequestFormElement formElement = new RequestFormElement(textBoxesNamesVaritables[i].Text, textBoxesValuesVaritables[i].Text);
                        form.Add(formElement);
                        formElement.Dispose();
                    }
                }
            }
            // start sending response
            Stopwatch sw = new Stopwatch();
            ECHOBox.Text = "WAIT...";
            sw.Start();
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(form.GetData());
            var response = client.PostAsync(TargetURLPole.Text, content).Result;

            // reading body site
            var contents = response.Content.ReadAsStringAsync().Result;
           // get echo
            ECHOBox.Text = contents;
            sw.Stop();
            TimeWaitText.Text = "Time milliseconds: " + sw.ElapsedMilliseconds;
            form.Dispose();


        }

        private void GETRequest ()
        {
            // ini form
            RequestForm form = new RequestForm();
            for (int i = 0; i < 4; i++)
            {
                if (textBoxesNamesVaritables[i].Text.Length > 0)
                {
                    if (textBoxesValuesVaritables[i].Text.Length > 0)
                    {
                        // generating form data
                        RequestFormElement formElement = new RequestFormElement(textBoxesNamesVaritables[i].Text, textBoxesValuesVaritables[i].Text);
                        form.Add(formElement);
                        formElement.Dispose();
                    }
                }
            }

            // start sending response
            Stopwatch sw = new Stopwatch();
            ECHOBox.Text = "WAIT...";
            sw.Start();
            string html = string.Empty;
           
            string url = form.GenerateGETRequest(TargetURLPole.Text);



            HttpWebRequest request = null;
            try
            {
                 request = (HttpWebRequest)WebRequest.Create(url);

            }

            catch (Exception e)
            {
                MessageBox.Show("Ошибка отправки запроса: Текст ошибки: " + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            sw.Stop();
            TimeWaitText.Text = "Time milliseconds: " + sw.ElapsedMilliseconds;
            ECHOBox.Text = html;
            form.Dispose();

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => Application.Exit();

        private void CallPostAsync() => POSTRequest();
       

    }
}
