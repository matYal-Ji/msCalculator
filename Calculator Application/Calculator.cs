using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathLibrary;

namespace Calculator_Application
{

    public partial class Calculator : Form
    {
        private readonly List<Button> buttons;
        private readonly Label ExpressionDisplayBox;
        private readonly TextBox ResultDisplayBox;
        private readonly ListView MemoryListView;
        private readonly Label ApplicationLabel, ListLabel;
        private readonly Evaluator Evaluator;
        private string Result;
        private string Operator;
        private string PreviousOperator;
        private int MemoryIndex;
        public Calculator()
        {
            buttons = new List<Button>();
            ExpressionDisplayBox = new Label();
            ResultDisplayBox = new TextBox();
            MemoryListView = new ListView();
            ApplicationLabel = new Label();
            ListLabel = new Label();
            Evaluator = new Evaluator();
            MemoryIndex = MemoryOperation.ViewStack().Count - 1;
            InitializeComponent();
            InitializeLayout();
            InitializeDisplay();
            InitializeButton();
            InitializeMemoryList();
        }

        private void InitializeLayout()
        {

        }

        private void InitializeMemoryList()
        {
            calculatorLayout.Controls.Add(MemoryListView, 1, 1);
            MemoryListView.Dock = DockStyle.Fill;
            MemoryListView.BackColor = SystemColors.Control;
            MemoryListView.Font = new Font("Roboto", 22F, FontStyle.Regular);
            MemoryListView.Name = "MemoryList";
            MemoryListView.View = View.Details;
            MemoryListView.Columns.Add("MEMORY", MemoryListView.Width - 20, HorizontalAlignment.Right);
            MemoryListView.HeaderStyle = ColumnHeaderStyle.None;
            List<double> memoryList = MemoryOperation.ViewStack();
            MemoryListView.Items.Clear();
            foreach (double value in memoryList) MemoryListView.Items.Insert(0, value.ToString());
        }

        private void InitializeButton()
        {
            List<Buttons> buttonInfo = new List<Buttons>();
            using (StreamReader content = new StreamReader("ButtonInfo.json"))
            {
                buttonInfo = JsonSerializer.Deserialize<List<Buttons>>(content.ReadToEnd());
            }
            // 
            // buttons
            // 
            for (int a = 0; a < buttonInfo.Count; a++) buttons.Add(new Button());
            int i = 0;
            foreach (Button button in buttons)
            {
                button.Dock = DockStyle.Fill;
                button.FlatStyle = FlatStyle.Flat;
                button.Name = buttonInfo[i].ButtonName;
                button.Text = buttonInfo[i].ButtonName;
                button.Tag = buttonInfo[i].Operator;
                if (buttonInfo[i].Panel == "Memory") memoryKeyPanel.Controls.Add(button, buttonInfo[i].IndexX, buttonInfo[i].IndexY);
                if (buttonInfo[i].Panel == "StandardCalculator") operatorOperandKeyPanel.Controls.Add(button, buttonInfo[i].IndexX, buttonInfo[i].IndexY);
                button.Click += new EventHandler(Button_Click);
                i++;
            }

        }

        private void InitializeDisplay()
        {
            displayKeyPanel.Controls.Add(ExpressionDisplayBox, 0, 0);
            ExpressionDisplayBox.BackColor = SystemColors.Control;
            ExpressionDisplayBox.BorderStyle = BorderStyle.None;
            ExpressionDisplayBox.Dock = DockStyle.Fill;
            ExpressionDisplayBox.Font = new Font("Roboto", 14F, FontStyle.Regular);
            ExpressionDisplayBox.Name = "ExpressionDisplayBox";
            ExpressionDisplayBox.TextAlign = ContentAlignment.MiddleRight;

            displayKeyPanel.Controls.Add(ResultDisplayBox, 0, 1);
            ResultDisplayBox.BackColor = SystemColors.Control;
            ResultDisplayBox.BorderStyle = BorderStyle.None;
            ResultDisplayBox.Dock = DockStyle.Fill;
            ResultDisplayBox.Font = new Font("Roboto", 24F, FontStyle.Regular);
            ResultDisplayBox.Name = "ResultDisplayBox";
            ResultDisplayBox.Text = "0";
            ResultDisplayBox.TextAlign = HorizontalAlignment.Right;

            calculatorLayout.Controls.Add(ApplicationLabel, 0, 0);
            ApplicationLabel.BackColor = SystemColors.Control;
            ApplicationLabel.BorderStyle = BorderStyle.None;
            ApplicationLabel.Dock = DockStyle.Fill;
            ApplicationLabel.Font = new Font("Segoe UI", 20F, FontStyle.Regular);
            ApplicationLabel.Name = "ApplicationLabel";
            ApplicationLabel.Text = "Standard Calculator";
            ApplicationLabel.TextAlign = ContentAlignment.BottomLeft;

            calculatorLayout.Controls.Add(ListLabel, 1, 0);
            ListLabel.BackColor = SystemColors.Control;
            ListLabel.BorderStyle = BorderStyle.None;
            ListLabel.Dock = DockStyle.Fill;
            ListLabel.Font = new Font("Segoe UI", 14F, FontStyle.Regular);
            ListLabel.Name = "HistoryLabel";
            ListLabel.Text = "History";
            ListLabel.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Tag != null)
            {
                string tag = (string)button.Tag, error;
                MemoryIndex = MemoryOperation.ViewStack().Count - 1;

                if (tag == "inv" || tag == "neg" || tag == "sq" || tag == "root")
                {
                    ResultDisplayBox.Text = tag + ResultDisplayBox.Text;
                }

                else if (tag == "/" || tag == "*" || tag == "-" || tag == "+" || tag == "mod")
                {
                    Operator = tag;

                    if (ResultDisplayBox.Text != null && ResultDisplayBox.Text.Length > 0 && ResultDisplayBox.Text[ResultDisplayBox.Text.Length - 1] == '!')
                    {
                        ExpressionDisplayBox.Text = string.Empty;
                        ResultDisplayBox.Text = string.Empty;
                        Result = string.Empty;
                    }
                    if (ResultDisplayBox.Text != string.Empty)
                    {
                        if (Get(Result) == null)
                        {
                            try
                            {
                                Result = Evaluator.Evaluate(ResultDisplayBox.Text).ToString();
                            }
                            catch { }
                        }
                        else
                        {
                            ExpressionDisplayBox.Text = Get(Result) + PreviousOperator;
                            if (ResultDisplayBox.Text != null)
                            {
                                error = Calculate();
                                if (error != null)
                                {
                                    ExpressionDisplayBox.Text = string.Empty;
                                    ResultDisplayBox.Text = error;
                                    Result = string.Empty;
                                    Operator = string.Empty;
                                }
                            }
                        }
                    }
                    else if (Result != string.Empty)
                    {
                        ExpressionDisplayBox.Text = Get(Result) + Operator;
                    }

                    if (ResultDisplayBox.Text != string.Empty && ResultDisplayBox.Text[ResultDisplayBox.TextLength - 1] == '=')
                    {
                        ExpressionDisplayBox.Text = Get(Result) + Operator;
                    }
                    ResultDisplayBox.Text = string.Empty;

                    PreviousOperator = Operator;
                }

                else
                {
                    if (ExpressionDisplayBox.Text != null && ExpressionDisplayBox.Text.Length > 0 && ExpressionDisplayBox.Text[ExpressionDisplayBox.Text.Length - 1] == '=')
                    {
                        Result = string.Empty;
                        Operator = string.Empty;
                        ExpressionDisplayBox.Text = string.Empty;
                        ResultDisplayBox.Text = string.Empty;
                    }
                    int length = ResultDisplayBox.TextLength;
                    if (ResultDisplayBox.Text == null || (length > 0 && ResultDisplayBox.Text[length - 1] == '!') || (length == 1 && ResultDisplayBox.Text[0] == '0'))
                    {
                        ResultDisplayBox.Text = tag;
                    }
                    else
                    {
                        ResultDisplayBox.Text += tag;
                    }
                }

                if (Operator != null && Get(Result) != null)
                {
                    ExpressionDisplayBox.Text = Get(Result) + Operator;
                }
            }
            else
            {
                if (button.Name != "MR") MemoryIndex = MemoryOperation.ViewStack().Count - 1;
                switch (button.Name[0])
                {
                    case 'M':
                        MemoryButtonClick(button.Name[1]);
                        break;

                    case 'C':   //Clear and ClearEntery logic
                        if (button.Name.Length == 2)
                        {

                        }
                        else
                        {
                            ExpressionDisplayBox.Text = string.Empty;
                        }
                        ResultDisplayBox.Text = "0";
                        Result = string.Empty;
                        Operator = string.Empty;
                        break;

                    case '<':   //Backspace logic
                        if (ResultDisplayBox.Text != null)
                        {
                            int length = ResultDisplayBox.Text.Length;
                            if (length > 0 && (char.IsDigit(ResultDisplayBox.Text[length - 1]) || ResultDisplayBox.Text[length - 1] == '.'))
                            {
                                ResultDisplayBox.Text = ResultDisplayBox.Text.Substring(0, length - 1);
                            }
                            else
                            {
                                ResultDisplayBox.Text = string.Empty;
                            }
                        }
                        break;

                    case '=':   //equals to logic
                        if (ExpressionDisplayBox.Text.Length > 0 && ExpressionDisplayBox.Text[ExpressionDisplayBox.Text.Length - 1] == '=')
                        {

                        }
                        else if (ResultDisplayBox.Text.Length > 0 && ResultDisplayBox.Text[ResultDisplayBox.Text.Length - 1] == '!')
                        {
                            ResultDisplayBox.Text = string.Empty;
                            ExpressionDisplayBox.Text = string.Empty;
                        }
                        else if (ResultDisplayBox.Text.Length > 0)
                        {
                            string error = Calculate();
                            if (error != null)
                            {
                                ResultDisplayBox.Text = error;
                            }
                        }
                        break;

                }
            }
        }

        private void MemoryButtonClick(char operation)
        {
            switch (operation)
            {
                case 'C':
                    MemoryOperation.Clear();
                    break;

                case 'R':
                    if (MemoryIndex >= 0)
                    {
                        double MemoryPop = MemoryOperation.Recall(MemoryIndex--);
                        ResultDisplayBox.Text = Get(MemoryPop.ToString());
                    }
                    break;

                case '+':
                    int length = ResultDisplayBox.Text.Length;
                    if (length > 0 && ResultDisplayBox.Text[length - 1] != '!')
                    {
                        MemoryOperation.Add(double.Parse(ResultDisplayBox.Text));
                    }
                    break;

                case '-':
                    length = ResultDisplayBox.Text.Length;
                    if (length > 0 && ResultDisplayBox.Text[length - 1] != '!')
                    {
                        MemoryOperation.Subtract(double.Parse(ResultDisplayBox.Text));
                    }
                    break;

                case 'S':
                    length = ResultDisplayBox.Text.Length;
                    if (length > 0 && ResultDisplayBox.Text[length - 1] != '!')
                    {
                        MemoryOperation.Store(Evaluator.Evaluate(ResultDisplayBox.Text));
                    }
                    break;

            }

            List<double> memoryList = MemoryOperation.ViewStack();
            MemoryListView.Items.Clear();
            for (int indexMemoryList = memoryList.Count - 1; indexMemoryList >= 0; indexMemoryList--)
            {
                MemoryListView.Items.Add(memoryList[indexMemoryList].ToString());
            }
        }

        private string Calculate()
        {
            try
            {
                ExpressionDisplayBox.Text += ResultDisplayBox.Text;
                Result = Evaluator.Evaluate(ExpressionDisplayBox.Text).ToString();
                ExpressionDisplayBox.Text += " = ";
                ResultDisplayBox.Text = Get(Result);

            }
            catch (DivideByZeroException)
            {
                return "Can't Divide By Zero!";
            }
            catch (Exception)
            {
                return "Invalid Expression!";
            }
            return null;
        }
        private string Get(string result)
        {
            if (result == null || result == "") return null;
            return result[0] == '-' ? "neg" + result.Substring(1, result.Length - 1) : result;
        }
    }
}