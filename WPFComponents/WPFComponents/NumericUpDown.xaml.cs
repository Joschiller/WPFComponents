using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFComponents
{
    /// <summary>
    /// An input control for integer values.
    /// <br/>
    /// Optionally a <see cref="Min"/> and <see cref="Max"/> can be configured as well as a <see cref="MaxLength"/> for the string length of the current <see cref="Value"/>.
    /// <br/>
    /// The value can be changed by either inserting a string into the control, by clicking the up or down buttons or by scrolling above the input field, which will change the <see cref="Value"/> by <see cref="StepPerScroll"/>.
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public delegate void ValueChangeHandler(int newVal);
        /// <summary>
        /// Event that is triggered, whenever the <see cref="Value"/> of the <see cref="NumericUpDown"/> changes.
        /// </summary>
        public event ValueChangeHandler ValueChanged;

        /// <summary>
        /// Value of the <see cref="NumericUpDown"/>.
        /// </summary>
        public int Value { get; private set; } = 0;
        /// <summary>
        /// Minimum <see cref="Value"/> of the <see cref="NumericUpDown"/>.
        /// </summary>
        public int? Min { get; private set; } = null;
        /// <summary>
        /// Maximum <see cref="Value"/> of the <see cref="NumericUpDown"/>.
        /// </summary>
        public int? Max { get; private set; } = null;
        /// <summary>
        /// Maximum character length of the <see cref="Value"/>. To set no limit set the <see cref="MaxLength"/> to <c>null</c>.
        /// <br/>
        /// Default: <c>null</c>
        /// </summary>
        public uint? MaxLength { get; set; } = null;
        /// <summary>
        /// Value that will be added or substracted from the current <see cref="Value"/> per scoll-event when using the mouse wheel over the input field.
        /// <br/>
        /// Default: <c>1</c>
        /// </summary>
        public uint StepPerScroll { get; set; } = 1;

        private readonly Regex NUMBER_REGEX = new Regex(@"^[-]?\d+$");
        public NumericUpDown()
        {
            InitializeComponent();
            DataContext = this;
            updateGUI(() => { });
        }
        private void updateGUI(Action callback)
        {
            number.Text = Value.ToString();
            if (Min.HasValue && Value < Min.Value) SetValue(Min.Value);
            if (Max.HasValue && Value > Max.Value) SetValue(Max.Value);
            down.IsEnabled = Value > int.MinValue && (!Min.HasValue || Value > Min.Value);
            up.IsEnabled = Value < int.MaxValue && (!Max.HasValue || Value < Max.Value) && (!MaxLength.HasValue || (Value + 1).ToString().Length <= MaxLength.Value);
            number.IsEnabled = down.IsEnabled || up.IsEnabled;
            callback?.Invoke();
        }

        #region Setter
        /// <summary>
        /// Sets the <see cref="Value"/> of the control.
        /// </summary>
        /// <param name="val">New <see cref="Value"/></param>
        public void SetValue(int val)
        {
            Value = val;
            updateGUI(() => { });
        }
        /// <summary>
        /// Sets the <see cref="Min"/> of the control.
        /// </summary>
        /// <param name="val">New <see cref="Min"/></param>
        public void SetMin(int? val)
        {
            Min = val;
            updateGUI(() => ValueChanged?.Invoke(Value));
        }
        /// <summary>
        /// Sets the <see cref="Max"/> of the control.
        /// </summary>
        /// <param name="val">New <see cref="Max"/></param>
        public void SetMax(int? val)
        {
            Max = val;
            updateGUI(() => ValueChanged?.Invoke(Value));
        }
        #endregion

        #region Handler
        private void up_Click(object sender, RoutedEventArgs e)
        {
            Value++;
            updateGUI(() => ValueChanged?.Invoke(Value));
        }
        private void down_Click(object sender, RoutedEventArgs e)
        {
            Value--;
            updateGUI(() => ValueChanged?.Invoke(Value));
        }
        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true; // prevent scolling outside the control
            var newValue = Value;
            if (e.Delta > 0) newValue += (int)StepPerScroll;
            if (e.Delta < 0) newValue -= (int)StepPerScroll;
            if (MaxLength.HasValue && newValue.ToString().Length > MaxLength.Value)
            {
                if (newValue.ToString().StartsWith("-"))
                {
                    int.TryParse("-" + new string('9', (int)MaxLength.Value - 1), out newValue);
                }
                else
                {
                    int.TryParse(new string('9', (int)MaxLength.Value), out newValue);
                }
            }
            Value = newValue;
            updateGUI(() => ValueChanged?.Invoke(Value));
        }
        private void number_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NUMBER_REGEX.IsMatch(number.Text))
            {
                var parsedValue = Value;
                if (int.TryParse(number.Text, out parsedValue))
                {
                    Value = parsedValue;
                    updateGUI(() => ValueChanged?.Invoke(Value));
                }
                else
                {
                    // ignore new number, because it cannot be parsed => set min/max number
                    SetValue(number.Text.StartsWith("-") ? int.MinValue : int.MaxValue);
                }
            }
            else
            {
                if (number.Text == "")
                {
                    Value = 0;
                    updateGUI(() => ValueChanged?.Invoke(Value));
                    number.CaretIndex = 1;
                }
                var carPos = number.CaretIndex;
                number.Text = Value.ToString();
                number.CaretIndex = Math.Min(carPos, number.Text.Length);
            }
        }
        #endregion
    }
}
