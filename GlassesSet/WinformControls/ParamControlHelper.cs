﻿/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using ArchiTed_Grasshopper;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextBox = ArchiTed_Grasshopper.WinformControls.TextBox;

namespace InfoGlasses.WinformControls
{
    public static class ParamControlHelper
    {
        #region Param

        #region ParamIO
        public static void SetData<TGoo, T>(ITargetParam<TGoo, T> owner, T value) where TGoo : GH_Goo<T>
        {
            GH_PersistentParam<TGoo> target = owner.Target;

            GH_Document.GH_ScheduleDelegate callback = new GH_Document.GH_ScheduleDelegate(ScheduleCallback);
            target.OnPingDocument().ScheduleSolution(20, callback);

            void ScheduleCallback(GH_Document doc)
            {
                target.RecordUndoEvent(typeof(T).Name + "Changed!");
                {
                    target.PersistentData.Clear();
                    target.SetPersistentData(value);
                }
            }
        }

        public static T GetData<TGoo, T>(ITargetParam<TGoo, T> owner, out GH_ParamAccess access) where TGoo : GH_Goo<T>
        {
            access = GH_ParamAccess.item;
            if (owner.Target.Attributes.HasInputGrip)
            {
                owner.Enable = owner.Target.SourceCount == 0;
            }
            else
            {
                owner.Enable = false;
            }


            if (owner.Target.VolatileData.AllData(true).Count() > 0)
            {
                if (owner.Target.VolatileData.AllData(false).Count() > 1)
                {
                    access = owner.Target.VolatileData.PathCount == 1 ? GH_ParamAccess.list : GH_ParamAccess.tree;
                }
                GH_Goo<T> gh_color = owner.Target.VolatileData.AllData(true).ElementAt(0) as GH_Goo<T>;
                return gh_color.Value;

            }
            else
            {
                ParamControlHelper.SetData<TGoo, T>(owner, owner.Default);
                return owner.Default;
            }
        }

        //public static string GetSuffix(GH_ParamAccess access)
        //{
        //    switch (access)
        //    {
        //        case GH_ParamAccess.item:
        //            return "I";
        //        case GH_ParamAccess.list:
        //            return "L";
        //        case GH_ParamAccess.tree:
        //            return "T";
        //        default:
        //            throw new Exception("access is invalid!");
        //    }
        //}
        #endregion

        #region ParamLayout

        public static RectangleF ParamLayoutBase(IGH_Attributes targetAtt, int Width, RectangleF bound, int sideDistance = 8, bool inflate = true)
        {
            RectangleF rect;
            //check whether input
            if (targetAtt.HasInputGrip)
            {
                rect = new RectangleF(bound.Left - Width - sideDistance, targetAtt.Bounds.Top, Width, targetAtt.Bounds.Height);
            }
            else
            {
                rect = new RectangleF(bound.Right + sideDistance, targetAtt.Bounds.Top, Width, targetAtt.Bounds.Height);
            }
            if (inflate) rect.Inflate(-2, -2);
            else rect = new RectangleF(new PointF(rect.X - 2, rect.Y), rect.Size);
            return rect;
        }

        public static RectangleF UpDownSmallRect(RectangleF rect, float dis = 2)
        {
            PointF pivot = new PointF(rect.X, rect.Y + dis);
            return new RectangleF(pivot, new SizeF(rect.Width, rect.Height - 2 * dis));
        }

        #endregion

        #region ParamRender
        public static void RenderParamButtonIcon(Graphics graphics, Bitmap icon, RectangleF bound, float size = 2)
        {

            RectangleF background = bound;
            background.Inflate(size, size);
            GraphicsPath path = TextBox.GetRoundRectangle(background, size);

            graphics.FillPath(new SolidBrush(Color.FromArgb(150, Color.WhiteSmoke)), path);
            graphics.DrawPath(new Pen(Color.DimGray, 1), path);
            graphics.DrawImage(icon, bound);
        }
        #endregion
        #endregion

        #region Add Object
        #region AddObjectIcon
        public static float IconSize => 12;
        public static float IconSpacing => 6;
        public static RectangleF GetIconBound(RectangleF bound, float multy = 1)
        {
            return new RectangleF(bound.X - ParamControlHelper.IconSize - ParamControlHelper.IconSpacing * multy, bound.Y + bound.Height / 2 - ParamControlHelper.IconSize / 2,
                ParamControlHelper.IconSize, ParamControlHelper.IconSize);
        }

        public static void IconRender<TGoo>(IAddObjectParam<TGoo> paramcontrol, GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel) where TGoo : class, IGH_Goo
        {
            if (channel == GH_CanvasChannel.Objects)
            {
                if (paramcontrol.MyProxies.Length != 1)
                {
                    ParamControlHelper.RenderParamButtonIcon(graphics, paramcontrol.Target.Icon_24x24, paramcontrol.IconButtonBound);
                }
                else if (paramcontrol.MyProxies.Length == 1)
                {
                    ParamControlHelper.RenderParamButtonIcon(graphics, paramcontrol.MyProxies[0].Icon.GetIcon(!paramcontrol.Target.Locked, true), paramcontrol.IconButtonBound);
                }
            }
        }
        #endregion

        #region Respond
        public static void AddObjectMouseDown<TGoo>(IAddObjectParam<TGoo> paramcontrol, object sender, MouseEventArgs e) where TGoo : class, IGH_Goo
        {
            GH_Viewport vp = Grasshopper.Instances.ActiveCanvas.Viewport;
            if (vp.Zoom >= 0.5f && paramcontrol.IconButtonBound.Contains(vp.UnprojectPoint(e.Location)))
            {
                if (paramcontrol.MyProxies.Length == 1)
                {
                    CreateNewObject(paramcontrol, 0);
                }
                else if (paramcontrol.MyProxies.Length > 1)
                {
                    ContextMenuStrip menu = new ContextMenuStrip() { ShowImageMargin = true };
                    for (int i = 0; i < paramcontrol.MyProxies.Length; i++)
                    {
                        void Item_Click(object sender1, EventArgs e1, int index)
                        {
                            CreateNewObject(paramcontrol, index);
                        }
                        WinFormPlus.AddClickItem(menu, paramcontrol.MyProxies[i].Name, null, paramcontrol.MyProxies[i].Icon.GetIcon(true, true), i, Item_Click, false);
                    }
                    menu.Show(Grasshopper.Instances.ActiveCanvas, e.Location);
                }
            }
        }

        #endregion


        #region Create Object
        public static void CreateNewObject<TGoo>(IAddObjectParam<TGoo> paramcontrol, int index = 0) where TGoo : class, IGH_Goo
        {
            CreateNewObject(paramcontrol.MyProxies, paramcontrol.Target, index);
        }


        public static void CreateNewObject(AddProxyParams[] proxies, IGH_Param target, int index = 0)
        {
            if (proxies.Length < index) return;

            IGH_DocumentObject obj = Grasshopper.Instances.ComponentServer.EmitObject(proxies[index].Guid);
            if (obj == null)
            {
                return;
            }

            CreateNewObject(obj, target, proxies[index].OutIndex);
        }

        public static void CreateNewObject(IGH_DocumentObject obj, IGH_Param target, int outIndex = 0, float leftMove = 100, string init = null)
        {

            if (obj == null)
            {
                return;
            }

            PointF comRightCenter = new PointF(target.Attributes.Bounds.Left - leftMove,
                target.Attributes.Bounds.Top + target.Attributes.Bounds.Height / 2);
            if (obj is GH_Component)
            {
                GH_Component com = obj as GH_Component;

                AddAObjectToCanvas(com, comRightCenter, false, init);

                target.AddSource(com.Params.Output[outIndex]);
                com.Params.Output[outIndex].Recipients.Add(target);

                target.OnPingDocument().NewSolution(false);
            }
            else if (obj is IGH_Param)
            {
                IGH_Param param = obj as IGH_Param;

                AddAObjectToCanvas(param, comRightCenter, false, init);

                target.AddSource(param);
                param.Recipients.Add(target);

                target.OnPingDocument().NewSolution(false);
            }
            else
            {
                target.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, LanguagableComponent.GetTransLation(new string[]
                {
                    "The added object is not a Component or Parameters!", "添加的对象不是一个运算器或参数！",
                }));
            }
        }


        public static void AddAObjectToCanvas(IGH_DocumentObject obj, PointF pivot, bool update, string init = null)
        {
            var functions = typeof(GH_Canvas).GetRuntimeMethods().Where(m => m.Name.Contains("InstantiateNewObject") && !m.IsPublic).ToArray();
            if (functions.Length > 0)
            {
                functions[0].Invoke(Grasshopper.Instances.ActiveCanvas, new object[] { obj, init, pivot, update });
            }
        }
        #endregion
        #endregion
    }
}
