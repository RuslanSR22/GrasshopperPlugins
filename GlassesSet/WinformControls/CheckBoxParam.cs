﻿/*  Copyright 2020 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using ArchiTed_Grasshopper;
using ArchiTed_Grasshopper.WinformControls;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckBox = ArchiTed_Grasshopper.WinformControls.CheckBox;

namespace InfoGlasses.WinformControls
{
    class CheckBoxParam<TGoo>: CheckBox, ITargetParam<TGoo, bool> where TGoo: GH_Goo<bool>
    {
        public GH_PersistentParam<TGoo> Target { get; }
        GH_Param<TGoo> IParamControlBase<TGoo>.Target => this.Target;
        public GH_ParamAccess Access { get; set; }

        private AddProxyParams[] _myProxies;
        public AddProxyParams[] MyProxies
        {
            get
            {
                _myProxies = _myProxies ?? ParamControlHelper.GetAddProxyParams(this);
                return _myProxies;
            }
        }
        public new ParamGlassesComponent Owner { get; }
        public RectangleF IconButtonBound => ParamControlHelper.GetIconBound(this.Bounds);
        public int Width => 20;

        public CheckBoxParam(GH_PersistentParam<TGoo> target, ParamGlassesComponent owner, bool enable,
            string[] tips = null, int tipsRelay = 5000, Func<ToolStripDropDownMenu> createMenu = null, bool isToggle = true,
            bool renderLittleZoom = false)
            : base(null, owner, null, enable, false, tips, tipsRelay, createMenu, isToggle, renderLittleZoom)
        {
            this.Target = target;
            this.Owner = owner;
            ParamControlHelper.SetDefaultValue(this, false);
        }

        public void RespondToMouseDown(object sender, MouseEventArgs e)
        {
            ParamControlHelper.ParamMouseDown(this, this.RespondToMouseUp, sender, e, init: GetValue().ToString());
        }

        public override void Layout(RectangleF innerRect, RectangleF outerRect)
        {
            float small = -2;
            RectangleF rect = CanvasRenderEngine.MaxSquare(ParamControlHelper.ParamLayoutBase(this.Target.Attributes, Width, outerRect));
            rect.Inflate(small, small);
            this.Bounds = rect;
        }

        protected override bool IsRender(GH_Canvas canvas, Graphics graphics, bool renderLittleZoom = false)
        {
            return ParamControlHelper.IsRender(this, canvas, graphics, renderLittleZoom) && base.IsRender(canvas, graphics, renderLittleZoom);
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            ParamControlHelper.IconRender(this, canvas, graphics, channel);
            base.Render(canvas, graphics, channel);
        }

        public override bool GetValue()
        {
            GH_ParamAccess access = GH_ParamAccess.item;
            var result = ParamControlHelper.GetData<TGoo, bool>(this, out access);
            this.Access = access;
            return result;
        }

        public override void SetValue(bool valueIn, bool record = true)
        {
            if (record)
            {
                Target.RecordUndoEvent("Set the boolean");
            }
            ParamControlHelper.SetData<TGoo, bool>(this, valueIn);
        }

        public void Dispose()
        {
            Grasshopper.Instances.ActiveCanvas.MouseDown -= RespondToMouseDown;
        }
    }
}
