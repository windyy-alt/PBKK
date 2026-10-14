using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        private Label lblHistory;
        private Label lblDisplay;

        private string currentInput = "0";
        private double firstValue = 0;
        private string currentOperator = "";
        private bool isNewInput = true;
        private bool justCalculated = false;

        // COLOR PALETTE
        private readonly Color clrDusk = ColorTranslator.FromHtml("#683173");     // background
        private readonly Color clrPlum = ColorTranslator.FromHtml("#601F3D");     // display panel
        private readonly Color clrRose = ColorTranslator.FromHtml("#BB444A");     // clear / delete
        private readonly Color clrPeach = ColorTranslator.FromHtml("#FD9268");    // number buttons
        private readonly Color clrOrange = ColorTranslator.FromHtml("#FD8951");   // operator buttons
        private readonly Color clrTextDark = ColorTranslator.FromHtml("#4A1E3A");
        private readonly Color clrWhite = Color.White;

        public Form1()
        {
            InitializeComponent();
            BuildCalculatorUI();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void BuildCalculatorUI()
        {
            this.Text = "Kalkulator";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(420, 630);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = clrDusk;

            // PANEL DISPLAY
            Panel displayPanel = new Panel();
            displayPanel.Size = new Size(370, 110);
            displayPanel.Location = new Point(25, 20);
            displayPanel.BackColor = clrPlum;
            this.Controls.Add(displayPanel);

            lblHistory = new Label();
            lblHistory.Text = "";
            lblHistory.ForeColor = Color.FromArgb(235, 210, 220);
            lblHistory.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblHistory.TextAlign = ContentAlignment.MiddleRight;
            lblHistory.Size = new Size(330, 24);
            lblHistory.Location = new Point(20, 15);
            displayPanel.Controls.Add(lblHistory);

            lblDisplay = new Label();
            lblDisplay.Text = "0";
            lblDisplay.ForeColor = clrWhite;
            lblDisplay.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            lblDisplay.TextAlign = ContentAlignment.MiddleRight;
            lblDisplay.Size = new Size(330, 50);
            lblDisplay.Location = new Point(20, 45);
            displayPanel.Controls.Add(lblDisplay);

            int startX = 34;
            int startY = 150;
            int btnW = 82;
            int btnH = 68;
            int gap = 8;

            string[,] buttonTexts =
            {
                { "%", "√", "x²", "⌫" },
                { "C", "÷", "×", "-" },
                { "7", "8", "9", "+" },
                { "4", "5", "6", "=" },
                { "1", "2", "3", "=" },
                { "+/-", "0", ".", "=" }
            };

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    string text = buttonTexts[row, col];

                    // Tombol "=" dibuat memanjang 3 baris
                    if (text == "=" && row == 3)
                    {
                        Button btnEqual = CreateButton(
                            "=",
                            startX + (btnW + gap) * col,
                            startY + (btnH + gap) * row,
                            btnW,
                            btnH * 3 + gap * 2,
                            clrOrange,
                            clrWhite
                        );
                        btnEqual.Click += Button_Click;
                        this.Controls.Add(btnEqual);
                    }

                    // skip tombol "=" row 4 dan 5 karena sudah dibuat panjang
                    if (text == "=" && row > 3)
                        continue;

                    if (!(text == "=" && row == 3))
                    {
                        Color backColor;
                        Color foreColor;

                        if (text == "C" || text == "⌫")
                        {
                            backColor = clrRose;
                            foreColor = clrWhite;
                        }
                        else if (text == "÷" || text == "×" || text == "-" || text == "+" || text == "=")
                        {
                            backColor = clrOrange;
                            foreColor = clrWhite;
                        }
                        else if (text == "%" || text == "√" || text == "x²" || text == "+/-")
                        {
                            backColor = clrPlum;
                            foreColor = clrWhite;
                        }
                        else
                        {
                            backColor = clrPeach;
                            foreColor = clrTextDark;
                        }

                        Button btn = CreateButton(
                            text,
                            startX + (btnW + gap) * col,
                            startY + (btnH + gap) * row,
                            btnW,
                            btnH,
                            backColor,
                            foreColor
                        );

                        btn.Click += Button_Click;
                        this.Controls.Add(btn);
                    }
                }
            }
        }

        private Button CreateButton(string text, int x, int y, int width, int height, Color backColor, Color foreColor)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(width, height);
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.TabStop = false;
            return btn;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string value = ((Button)sender).Text;

            if (char.IsDigit(value, 0))
            {
                InputNumber(value);
                return;
            }

            switch (value)
            {
                case ".":
                    InputDecimal();
                    break;
                case "C":
                    ClearAll();
                    break;
                case "⌫":
                    Backspace();
                    break;
                case "+/-":
                    ToggleSign();
                    break;
                case "%":
                    ApplyPercent();
                    break;
                case "√":
                    ApplySqrt();
                    break;
                case "x²":
                    ApplySquare();
                    break;
                case "+":
                case "-":
                case "×":
                case "÷":
                    SetOperator(value);
                    break;
                case "=":
                    CalculateResult();
                    break;
            }
        }

        private void InputNumber(string number)
        {
            if (currentInput == "0" || isNewInput || justCalculated)
            {
                currentInput = number;
                isNewInput = false;
                justCalculated = false;
            }
            else
            {
                currentInput += number;
            }

            UpdateDisplay();
        }

        private void InputDecimal()
        {
            if (isNewInput || justCalculated)
            {
                currentInput = "0.";
                isNewInput = false;
                justCalculated = false;
            }
            else if (!currentInput.Contains("."))
            {
                currentInput += ".";
            }

            UpdateDisplay();
        }

        private void SetOperator(string op)
        {
            try
            {
                if (!isNewInput && currentOperator != "")
                {
                    CalculateResult();
                }

                firstValue = ParseDisplayValue();
                currentOperator = op;
                lblHistory.Text = $"{FormatNumber(firstValue)} {op}";
                isNewInput = true;
                justCalculated = false;
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan saat memilih operator.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateResult()
        {
            try
            {
                if (string.IsNullOrEmpty(currentOperator))
                    return;

                double secondValue = ParseDisplayValue();
                double result = 0;

                switch (currentOperator)
                {
                    case "+":
                        result = firstValue + secondValue;
                        break;
                    case "-":
                        result = firstValue - secondValue;
                        break;
                    case "×":
                        result = firstValue * secondValue;
                        break;
                    case "÷":
                        if (secondValue == 0)
                            throw new DivideByZeroException();
                        result = firstValue / secondValue;
                        break;
                }

                lblHistory.Text = $"{FormatNumber(firstValue)} {currentOperator} {FormatNumber(secondValue)} =";
                currentInput = result.ToString(CultureInfo.InvariantCulture);
                currentOperator = "";
                firstValue = result;
                isNewInput = true;
                justCalculated = true;

                UpdateDisplay();
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Tidak bisa membagi dengan nol.", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearAll();
            }
            catch
            {
                MessageBox.Show("Terjadi kesalahan saat menghitung.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearAll();
            }
        }

        private void ApplyPercent()
        {
            try
            {
                double value = ParseDisplayValue();
                value /= 100.0;
                currentInput = value.ToString(CultureInfo.InvariantCulture);
                UpdateDisplay();
            }
            catch
            {
                MessageBox.Show("Operasi persen gagal.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplySquare()
        {
            try
            {
                double value = ParseDisplayValue();
                double result = Math.Pow(value, 2);
                lblHistory.Text = $"sqr({FormatNumber(value)})";
                currentInput = result.ToString(CultureInfo.InvariantCulture);
                isNewInput = true;
                justCalculated = true;
                UpdateDisplay();
            }
            catch
            {
                MessageBox.Show("Operasi kuadrat gagal.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplySqrt()
        {
            try
            {
                double value = ParseDisplayValue();

                if (value < 0)
                {
                    MessageBox.Show("Tidak bisa mencari akar dari bilangan negatif.", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                double result = Math.Sqrt(value);
                lblHistory.Text = $"√({FormatNumber(value)})";
                currentInput = result.ToString(CultureInfo.InvariantCulture);
                isNewInput = true;
                justCalculated = true;
                UpdateDisplay();
            }
            catch
            {
                MessageBox.Show("Operasi akar gagal.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleSign()
        {
            if (currentInput != "0")
            {
                if (currentInput.StartsWith("-"))
                    currentInput = currentInput.Substring(1);
                else
                    currentInput = "-" + currentInput;

                UpdateDisplay();
            }
        }

        private void Backspace()
        {
            if (!isNewInput && currentInput.Length > 1)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else
            {
                currentInput = "0";
            }

            UpdateDisplay();
        }

        private void ClearAll()
        {
            currentInput = "0";
            firstValue = 0;
            currentOperator = "";
            isNewInput = true;
            justCalculated = false;
            lblHistory.Text = "";
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            lblDisplay.Text = FormatNumber(ParseStringValue(currentInput));
        }

        private double ParseDisplayValue()
        {
            return ParseStringValue(currentInput);
        }

        private double ParseStringValue(string value)
        {
            double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result);
            return result;
        }

        private string FormatNumber(double value)
        {
            if (value % 1 == 0)
                return value.ToString("0", CultureInfo.InvariantCulture);

            return value.ToString("0.##########", CultureInfo.InvariantCulture);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                InputNumber((e.KeyCode - Keys.D0).ToString());
            }
            else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            {
                InputNumber((e.KeyCode - Keys.NumPad0).ToString());
            }
            else if (e.KeyCode == Keys.Add || (e.KeyCode == Keys.Oemplus && e.Shift))
            {
                SetOperator("+");
            }
            else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
            {
                SetOperator("-");
            }
            else if (e.KeyCode == Keys.Multiply)
            {
                SetOperator("×");
            }
            else if (e.KeyCode == Keys.Divide || e.KeyCode == Keys.OemQuestion)
            {
                SetOperator("÷");
            }
            else if (e.KeyCode == Keys.Enter)
            {
                CalculateResult();
            }
            else if (e.KeyCode == Keys.Back)
            {
                Backspace();
            }
            else if (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod)
            {
                InputDecimal();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                ClearAll();
            }
        }
    }
}