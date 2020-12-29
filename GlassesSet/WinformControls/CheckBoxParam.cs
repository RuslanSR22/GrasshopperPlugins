﻿/*  Copyright 2020 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using ArchiTed_Grasshopper.WPF;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchiTed_Grasshopper.WinformControls
{
    class CheckBoxParam: CheckBox, ITargetParam<bool>
    {
        public GH_PersistentParam<GH_Goo<bool>> Target { get; }

        public GH_ParamAccess Access { get; set; }

        //public string Suffix => WinformControlHelper.GetSuffix(this.Access);

        public int Width => 20;

        public CheckBoxParam(string valueName, ControllableComponent owner, Func<RectangleF, RectangleF, RectangleF> layout, bool enable,
            bool @default, string[] tips = null, int tipsRelay = 1000, Func<ToolStripDropDownMenu> createMenu = null, bool isToggle = true,
            bool renderLittleZoom = false)
            : base(valueName, owner, layout, enable, @default, tips, tipsRelay, createMenu, isToggle, renderLittleZoom)
        {

        }

        public override void Layout(RectangleF innerRect, RectangleF outerRect)
        {
            this.Bounds = CanvasRenderEngine.MaxSquare(WinformControlHelper.ParamLayoutBase(this.Target.Attributes, Width, innerRect, outerRect));
            this.Bounds.Inflate(-2, -2);
        }



        protected override bool GetValue()
        {
            GH_ParamAccess access = GH_ParamAccess.item;
            var result = WinformControlHelper.GetData<bool>(this, out access);
            this.Access = access;
            return result;
        }

        protected override void SetValue(bool valueIn)
        {
            WinformControlHelper.SetData<bool>(this, valueIn);
        }
    }
}