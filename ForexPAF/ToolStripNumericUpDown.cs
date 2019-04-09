using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ForexPAF
{
    /// <summary>
    /// ToolStripNumericUpDown
    /// </summary>
    [System.Windows.Forms.Design.ToolStripItemDesignerAvailability(
     System.Windows.Forms.Design.ToolStripItemDesignerAvailability.ToolStrip |
     System.Windows.Forms.Design.ToolStripItemDesignerAvailability.StatusStrip)]
    [ToolboxBitmap(typeof(NumericUpDown), "NumericUpDown")]
    public sealed class ToolStripNumericUpDown : ToolStripControlHost
    {

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ToolStripNumericUpDown()
            : base(new NumericUpDown()
            {
                TextAlign = HorizontalAlignment.Right
            ,
                Maximum = 128,
                Minimum = 2
            })
        { }

        public ToolStripNumericUpDown(string name)
            : base(new NumericUpDown()
            {
                TextAlign = HorizontalAlignment.Right
            ,
                Maximum = 128,
                Minimum = 2
            }, name)
        { }

        #endregion

        #region プロパティ

        /// <summary>
        /// ホストしているNumericUpDownコントロール
        /// </summary>
        public NumericUpDown NumericUpDown
        {
            get
            {
                return (NumericUpDown)Control;
            }
        }

        /// <summary>
        /// ホストしているコントロールのValueの値を取得または設定します。
        /// </summary>
        public decimal Value
        {
            get
            {
                return NumericUpDown.Value;
            }
            set
            {
                NumericUpDown.Value = value;
            }
        }

        /// <summary>
        /// ホストしているコントロールのTextの値を取得または設定します。
        /// </summary>
        public string NumericUpDownText
        {
            get
            {
                return NumericUpDown.Text;
            }
            set
            {
                NumericUpDown.Text = value;
            }
        }

        #endregion

        #region イベント

        /// <summary>
        /// Valueの値が変化した
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// ValueChangedイベント発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDown_OnValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }

        #endregion

        #region オーバーライド

        /// <summary>
        /// ホストしているNumericUpDownのイベントをサブスクライブします。
        /// </summary>
        /// <param name="control"></param>
        protected override void OnSubscribeControlEvents(System.Windows.Forms.Control control)
        {
            base.OnSubscribeControlEvents(control);
            NumericUpDown chkControl = (NumericUpDown)control;
            chkControl.ValueChanged += new EventHandler(NumericUpDown_OnValueChanged);
        }

        /// <summary>
        /// ホストしているNumericUpDownのイベントをアンサブスクライブします。
        /// </summary>
        /// <param name="control"></param>
        protected override void OnUnsubscribeControlEvents(System.Windows.Forms.Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            NumericUpDown chkControl = (NumericUpDown)control;
            chkControl.ValueChanged -= new EventHandler(NumericUpDown_OnValueChanged);
        }

        #endregion
    }
}
