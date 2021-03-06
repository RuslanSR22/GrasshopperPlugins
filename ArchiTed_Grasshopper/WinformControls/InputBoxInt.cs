﻿/*  Copyright 2020 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchiTed_Grasshopper.WinformControls
{
    public class InputBoxInt: InputBoxBase<int>
    {
        public int Min { get; }
        public int Max { get; }

        public InputBoxInt(string valueName, ControllableComponent owner, Func<int, RectangleF, RectangleF, RectangleF> layoutWidth, bool enable,
            int @default, int min = int.MinValue, int max = int.MaxValue, string[] tips = null, int tipsRelay = 5000, Func<ToolStripDropDownMenu> createMenu = null,
            bool renderLittleZoom = false)
            : base(valueName, owner, layoutWidth, enable, tips, tipsRelay, createMenu, renderLittleZoom)
        {
            this.Min = min;
            this.Default = @default;
            this.Max = max;
        }

        protected override string WholeToString()
        {
            bool isNull;
            int value = this.GetValue(out isNull);
            if (isNull)
            {
                return "Null";
            }
            else
            {
                return value.ToString();
            }

        }

        protected override int StringToT(string str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                result = result >= Min ? result : Min;
                result = result <= Max ? result : Max;
            }
            else
            {
                result = Default;
            }
            return result;
        }

        public override int GetValue(out bool isNull)
        {
            isNull = false;
            return Owner.GetValuePub(ValueName, Default);
        }

        public override void SetValue(int valueIn, bool record)
        {
            Owner.SetValuePub(ValueName, valueIn, record);
        }
    }
}
