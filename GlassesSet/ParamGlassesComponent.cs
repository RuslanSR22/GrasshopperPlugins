﻿/*  Copyright 2020 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

//数值存储以TypeFullName进行存储！

using ArchiTed_Grasshopper;
using ArchiTed_Grasshopper.WinformControls;
using ArchiTed_Grasshopper.WPF;
using Grasshopper.Kernel;
using InfoGlasses.WPF;
using InfoGlasses.WinformControls;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using System.IO;
using System.Text;
using GH_IO.Serialization;
using System.Linq;

namespace InfoGlasses
{
    public class ParamGlassesComponent : LanguagableComponent
    {
        #region Basic Component Info
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WireGlasses;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid =>new Guid("0100df68-78e0-42bf-8055-6e352465c8b1");
        #endregion

        #region Values

        #region Wire Setting
        private const string _selectWireThickness = "selectWireThickness";
        private const double _selectWireThicknessDefault = 4;
        public double SelectWireThickness => GetValue(_selectWireThickness, _selectWireThicknessDefault);

        private const string _selectWireSolid = "selectWireSolid";
        private const int _selectWireSolidDefault = 255;
        public int SelectWireSolid => GetValue(_selectWireSolid, _selectWireSolidDefault);

        private const string _wireType = "wireType";
        private const int _wireTypeDefault = 0;
        public int WireType => GetValue(_wireType, _wireTypeDefault);

        private const string _polywireParam = "polywireParam";
        private const double _polywireParamDefault = 0.5;
        public double PolywireParam => GetValue(_polywireParam, _polywireParamDefault);

        private const string _accuracy = "accuracy";
        private const int _accuracyDefault = 1;
        public int Accuracy => GetValue(_accuracy, _accuracyDefault);
        #endregion

        #region Default Color
        private const string _defaultColor = "defaultColor";
        private readonly Color _defaultColorDefault = Color.FromArgb(150, 0, 0, 0);
        public Color DefaultColor => GetValue(_defaultColor, _defaultColorDefault);

        private const string _selectedColor = "selectedColor";
        private readonly Color _selectedColorDefault = Color.FromArgb(125, 210, 40);
        public Color SelectedColor => GetValue(_selectedColor, _selectedColorDefault);

        private const string _unselectColor = "unselectedColor";
        private readonly Color _unselectColorDefalut = Color.FromArgb(50, 0, 0, 0);
        public Color UnselectColor => GetValue(_unselectColor, _unselectColorDefalut);

        private const string _emptyColor = "emptyColor";
        private readonly Color _emptyColorDefault = Color.FromArgb(180, 255, 60, 0);
        public Color EmptyColor => GetValue(_emptyColor, _emptyColorDefault);
        #endregion

        #region Label
        private const string _showLabel = "showLabel";
        private const bool _showLabelDefault = false;
        public bool IsShowLabel => GetValue(_showLabel, _showLabelDefault);

        private const string _labelFontSize = "labelFontSize";
        private const double _labelFontSizeDefault = 5;
        public double LabelFontSize => GetValue(_labelFontSize, _labelFontSizeDefault);

        private const string _labelTextColor = "labelTextColor";
        private readonly Color _labelTextColorDefault = Color.Black;
        public Color LabelTextColor => GetValue(_labelTextColor, _labelTextColorDefault);

        private const string _labelBackgroundColor = "labelBackGroundColor";
        private readonly Color _labelDackgroundColorDefault = Color.WhiteSmoke;
        public Color LabelBackGroundColor => GetValue(_labelBackgroundColor, _labelDackgroundColorDefault);

        private const string _labelBoundaryColor = "labelBoundaryColor";
        private readonly Color _labelBoundaryColorDefault = Color.FromArgb(30, 30, 30);
        public Color LabelBoundaryColor => GetValue(_labelBoundaryColor, _labelBoundaryColorDefault);

        #endregion
        #region Tree
        private const string _showTree = "showTree";
        private const bool _showTreeDefault = false;
        public bool IsShowTree => GetValue(_showTree, _showTreeDefault);

        private const string _treeCount = "treeCount";
        private const int _treeCountDefault = 10;
        public int TreeCount => GetValue(_treeCount, _treeCountDefault);
        #endregion

        #region Legend
        private const string _showLegend = "showLegend";
        private const bool _showLegendDefault = false;
        public bool IsShowLegend => GetValue(_showLegend, _showLegendDefault);

        private const string _legendLocation = "legendLocation";
        private const int _legendLocationDefault = 2;
        public int LegendLocation => GetValue(_legendLocation, _legendLocationDefault);

        private const string _legendSize = "legendSize";
        private const double _legendSizeDefault = 20;
        public double LegendSize => GetValue(_legendSize, _legendSizeDefault);

        private const string _legendSpacing = "legendSpacing";
        private const double _legendSpacingDefault = 30;
        public double LegendSpacing => GetValue(_legendSpacing, _legendSpacingDefault);

        private const string _legendTextColor = "legendTextColor";
        private readonly Color _legendTextColorDefault = Color.Black;
        public Color LegendTextColor => GetValue(_legendTextColor, _legendTextColorDefault);

        private const string _legendBackgroundColor = "legendBackGroundColor";
        private readonly Color _legendDackgroundColorDefault = Color.WhiteSmoke;
        public Color LegendBackGroundColor => GetValue(_legendBackgroundColor, _legendDackgroundColorDefault);

        private const string _legendBoundaryColor = "labelBoundaryColor";
        private readonly Color _legendBoundaryColorDefault = Color.FromArgb(30, 30, 30);
        public Color LegendBoundaryColor => GetValue(_legendBoundaryColor, _legendBoundaryColorDefault);

        #endregion

        private const string _showControl = "showControl";
        private const bool _showControlDefault = false;
        public bool IsShowControl => GetValue(_showControl, _showControlDefault);

        private const string _showBoolControl = "showBoolControl";
        private const bool _showBoolControlDefault = true;
        public bool IsShowBoolControl => GetValue(_showBoolControl, _showBoolControlDefault);

        private const string _showColorControl = "showColorControl";
        private const bool _showColorControlDefault = true;
        public bool IsShowColorControl => GetValue(_showColorControl, _showColorControlDefault);

        private const string _showDoubleControl = "showDoubleControl";
        private const bool _showDoubleControlDefault = true;
        public bool IsShowDoubleControl => GetValue(_showDoubleControl, _showDoubleControlDefault);

        private const string _showIntControl = "showIntControl";
        private const bool _showIntControlDefault = true;
        public bool IsShowIntControl => GetValue(_showIntControl, _showIntControlDefault);

        private const string _showStringControl = "showStringControl";
        private const bool _showStringControlDefault = true;
        public bool IsShowStringControl => GetValue(_showStringControl, _showStringControlDefault);

        #endregion

        private bool _run = true;
        private bool _isFirst = true;

        private List<GooTypeProxy> _allParamProxy;
        public List<GooTypeProxy> AllParamProxy
        {
            get
            {
                if (_allParamProxy == null)
                {
                    UpdateAllParamProxy();
                }
                return _allParamProxy;
            }
            set { _allParamProxy = value; }
        }

        private List<ParamGlassesProxy> _allProxy;
        public List<ParamGlassesProxy> AllProxy
        {
            get
            {
                if (_allProxy == null)
                {
                    UpdateAllProxy();
                }
                return _allProxy;
            }
            set { _allProxy = value; }
        }

        public Dictionary<string, AddProxyParams[]> CreateProxyDict { get; set; }

        public List<GooTypeProxy> ShowProxy { get; internal set; }

        public Dictionary<string, Color> ColorDict { get; set; }

        /// <summary>
        /// Initializes a new instance of the ParamGlassesComponent class.
        /// </summary>
        public ParamGlassesComponent()
            : base(GetTransLation(new string[] { "ParamGlasses", "参数眼镜" }), GetTransLation(new string[] { "Param", "参数" }),
                 GetTransLation(new string[] { "To show the wire's and parameter's advances information.Right click or double click to have advanced options.",
                     "显示连线或参数的高级信息。右键或者双击可以获得更多选项。" }), "Params", "Showcase Tools", windowsType: typeof(WireColorsWindow))

        {
            LanguageChanged += ResponseToLanguageChanged;
            ResponseToLanguageChanged(this, new EventArgs());
            ShowProxy = new List<GooTypeProxy>();
            //For test
            CreateProxyDict = new Dictionary<string, AddProxyParams[]>() 
            {
                {"Grasshopper.Kernel.Types.GH_Point", new AddProxyParams[]{new AddProxyParams(new Guid("{3581F42A-9592-4549-BD6B-1C0FC39D067B}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Vector", new AddProxyParams[]{new AddProxyParams(new Guid("{56b92eab-d121-43f7-94d3-6cd8f0ddead8}"), 0) } },

                {"Grasshopper.Kernel.Types.GH_Circle",new AddProxyParams[]{
                    new AddProxyParams(new Guid("{807b86e3-be8d-4970-92b5-f8cdcb45b06b}"), 0),
                    new AddProxyParams(new Guid("{d114323a-e6ee-4164-946b-e4ca0ce15efa}"), 0),
                } },
                {"Grasshopper.Kernel.Types.GH_Arc", new AddProxyParams[]{new AddProxyParams(new Guid("{bb59bffc-f54c-4682-9778-f6c3fe74fce3}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Curve", new AddProxyParams[]{new AddProxyParams(new Guid("{2b2a4145-3dff-41d4-a8de-1ea9d29eef33}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Line", new AddProxyParams[]{
                    new AddProxyParams(new Guid("{4c4e56eb-2f04-43f9-95a3-cc46a14f495a}"), 0),
                    new AddProxyParams(new Guid("{4c619bc9-39fd-4717-82a6-1e07ea237bbe}"), 0),
                } },
                {"Grasshopper.Kernel.Types.GH_Plane", new AddProxyParams[]{new AddProxyParams(new Guid("{bc3e379e-7206-4e7b-b63a-ff61f4b38a3e}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Rectangle", new AddProxyParams[]{new AddProxyParams(new Guid("{d93100b6-d50b-40b2-831a-814659dc38e3}"), 0) } },

                {"Grasshopper.Kernel.Types.GH_Box", new AddProxyParams[]{
                    new AddProxyParams(new Guid("{28061aae-04fb-4cb5-ac45-16f3b66bc0a4}"), 0),
                    new AddProxyParams(new Guid("{79aa7f47-397c-4d3f-9761-aaf421bb7f5f}"), 0),
                } },
                {"Grasshopper.Kernel.Types.GH_Mesh",new AddProxyParams[]{ 
                    new AddProxyParams(new Guid("{e2c0f9db-a862-4bd9-810c-ef2610e7a56f}"), 0),
                    new AddProxyParams(new Guid("{60e7defa-8b21-4ee1-99aa-a9223d6134ff}"), 0),
                    new AddProxyParams(new Guid("{58cf422f-19f7-42f7-9619-fc198c51c657}"), 0),
                } },
                {"Grasshopper.Kernel.Types.GH_MeshFace", new AddProxyParams[]{
                    new AddProxyParams(new Guid("{1cb59c86-7f6b-4e52-9a0c-6441850e9520}"), 0),
                    new AddProxyParams(new Guid("{5a4ddedd-5af9-49e5-bace-12910a8b9366}"), 0), 
                } },
                {"Grasshopper.Kernel.Types.GH_SubD", new AddProxyParams[]{new AddProxyParams(new Guid("{855a2c73-31c0-41d2-b061-57d54229d11b}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Surface", new AddProxyParams[]{
                    new AddProxyParams(new Guid("{6e5de495-ba76-42d0-9985-a5c265e9aeca}"), 0),
                    new AddProxyParams(new Guid("{c77a8b3b-c569-4d81-9b59-1c27299a1c45}"), 0),
                    new AddProxyParams(new Guid("{36132830-e2ef-4476-8ea1-6a43922344f0}"), 0),
                    new AddProxyParams(new Guid("{71506fa8-9bf0-432d-b897-b2e0c5ac316c}"), 0),
                } },
                {"SquishyXMorphs.GH_TwistedBox",new AddProxyParams[]{ new AddProxyParams(new Guid("{124de0f5-65f8-4ae0-8f61-8fb066e2ba02}"), 0) } },

                {"Grasshopper.Kernel.Types.GH_Field",new AddProxyParams[]{ 
                    new AddProxyParams(new Guid("{d9a6fbd2-2e9f-472e-8147-33bf0233a115}"), 0),
                    new AddProxyParams(new Guid("{8cc9eb88-26a7-4baa-a896-13e5fc12416a}"), 0),
                    new AddProxyParams(new Guid("{cffdbaf3-8d33-4b38-9cad-c264af9fc3f4}"), 0),
                    new AddProxyParams(new Guid("{4b59e893-d4ee-4e31-ae24-a489611d1088}"), 0),
                    new AddProxyParams(new Guid("{d27cc1ea-9ef7-47bf-8ee2-c6662da0e3d9}"), 0),
                } },
                {"Grasshopper.Kernel.Types.GH_GeometryGroup",new AddProxyParams[]{ new AddProxyParams(new Guid("{874eebe7-835b-4f4f-9811-97e031c41597}"), 0) } },

                {"Grasshopper.Kernel.Types.GH_ComplexNumber",new AddProxyParams[]{ new AddProxyParams(new Guid("{63d12974-2915-4ccf-ac26-5d566c3bac92}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Interval", new AddProxyParams[]{new AddProxyParams(new Guid("{d1a28e95-cf96-4936-bf34-8bf142d731bf}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Interval2D", new AddProxyParams[]{
                    new AddProxyParams(new Guid("{8555a743-36c1-42b8-abcc-06d9cb94519f}"), 0),
                    new AddProxyParams(new Guid("{9083b87f-a98c-4e41-9591-077ae4220b19}"), 0),
                } },
                {"Grasshopper.Kernel.Types.GH_Matrix", new AddProxyParams[]{new AddProxyParams(new Guid("{54ac80cf-74f3-43f7-834c-0e3fe94632c6}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Time",new AddProxyParams[]{ new AddProxyParams(new Guid("{31534405-6573-4be6-8bf8-262e55847a3a}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_StructurePath",new AddProxyParams[]{ new AddProxyParams(new Guid("{946cb61e-18d2-45e3-8840-67b0efa26528}"), 0) } },
                {"Grasshopper.Kernel.Types.GH_Material",new AddProxyParams[]{ new AddProxyParams(new Guid("{76975309-75a6-446a-afed-f8653720a9f2}"), 0) } },

                {"Grasshopper.Kernel.Types.GH_MeshingParameters",new AddProxyParams[]{ new AddProxyParams(new Guid("{4a0180e5-d8f9-46e7-bd34-ced804601462}"), 0) } },
                {"SurfaceComponents.SurfaceComponents.LoftOptions",new AddProxyParams[]{ new AddProxyParams(new Guid("{45f19d16-1c9f-4b0f-a9a6-45a77f3d206c}"), 0) } },

            };

            int width = 24;

            Func<RectangleF, RectangleF> changeInput;
            var inFuncs = WinformControlHelper.GetInnerRectLeftFunc(1, 2, new SizeF(width, width), out changeInput);
            this.ChangeInputLayout = changeInput;

            Func<RectangleF, RectangleF> changeOutput;
            var outFuncs = WinformControlHelper.GetInnerRectRightFunc(1, 2, new SizeF(width, width), out changeOutput);
            this.ChangeOutputLayout = changeOutput;

            ClickButtonIcon<LangWindow> LabelButton = new ClickButtonIcon<LangWindow>(_showLabel, this, inFuncs(1), true, Properties.Resources.LabelIcon, _showLabelDefault,
               tips: new string[] { "Click to choose whether to show the wire's label.", "点击以选择是否要显示连线的名称。" },
               createMenu: () =>
               {
                   ContextMenuStrip menu = new ContextMenuStrip() { ShowImageMargin = true };
                   return menu;
               });

            ClickButtonIcon<LangWindow> treeButton = new ClickButtonIcon<LangWindow>(_showTree, this, inFuncs(0), true, Properties.Resources.ShowTreeStructure, _showTreeDefault,
                tips: new string[] { "Click to switch whether to show the wire's data structure", "点击以选择是否要显示连线的数据结构。" },
                createMenu: () =>
                {
                    ContextMenuStrip menu = new ContextMenuStrip() { ShowImageMargin = true };

                    WinFormPlus.AddNumberBoxItem(menu, this, GetTransLation(new string[] { "Data Tree Count", "树型数据显示长度" }),
                        GetTransLation(new string[] { "Set Data Tree Count", "设置树型数据显示长度" }),
                        ArchiTed_Grasshopper.Properties.Resources.SizeIcon, true, _treeCountDefault, 1, 50, _treeCount);

                    return menu;
                });


            ClickButtonIcon<LangWindow> LegendButton = new ClickButtonIcon<LangWindow>(_showLegend, this, outFuncs(0), true, Properties.Resources.LegendIcon, _showLegendDefault,
                tips: new string[] { "Click to choose whether to show the wire's legend.", "点击以选择是否要显示连线的图例。" },
                createMenu: () =>
                {
                    ContextMenuStrip menu = new ContextMenuStrip() { ShowImageMargin = true };

                    WinFormPlus.AddLoopBoexItem(menu, this, GetTransLation(new string[] { "Legend Location", "图例位置" }), true, new string[]
                    {
                        GetTransLation(new string[] { "Left Top", "左上角" }),
                        GetTransLation(new string[] { "Left Buttom", "左下角" }),
                        GetTransLation(new string[] { "Right Buttom", "右下角" }),
                        GetTransLation(new string[] { "Right Top", "右上角" }),
                    }, _legendLocationDefault, _legendLocation, new Bitmap[] 
                    { 
                        Properties.Resources.LeftTopIcon,
                        Properties.Resources.LeftBottomIcon,
                        Properties.Resources.RightBottomIcon,
                        Properties.Resources.RightTopIcon,
                    });

                    WinFormPlus.AddNumberBoxItem(menu, this, GetTransLation(new string[] { "Legend Size", "图例大小" }),
                        GetTransLation(new string[] { "Set Legend Size", "设置图例大小" }),
                        ArchiTed_Grasshopper.Properties.Resources.SizeIcon, true, _legendSizeDefault, 10, 100, _legendSize);

                    WinFormPlus.AddNumberBoxItem(menu, this, GetTransLation(new string[] { "Legend Spacing", "图例间距" }),
                        GetTransLation(new string[] { "Set Legend Spacing to Border", "设置图例到窗体边缘的距离" }),
                        ArchiTed_Grasshopper.Properties.Resources.DistanceIcon, true, _legendSpacingDefault, 0, 200, _legendSpacing);

                    WinFormPlus.ItemSet<Color>[] sets = new WinFormPlus.ItemSet<Color>[] {

                    new WinFormPlus.ItemSet<Color>( GetTransLation(new string[] { "Text Color", "文字颜色" }),GetTransLation(new string[] { "Adjust text color.", "调整文字颜色。" }),
                    null, true, _legendTextColorDefault, _legendTextColor),

                    new WinFormPlus.ItemSet<Color>( GetTransLation(new string[] { "Background Color", "背景颜色" }), GetTransLation(new string[] { "Adjust background color.", "调整背景颜色。" }),
                    null, true, _legendDackgroundColorDefault, _legendBackgroundColor),

                    new WinFormPlus.ItemSet<Color>(GetTransLation(new string[] { "Boundary Color", "边框颜色" }),
                            GetTransLation(new string[] { "Adjust boundary color.", "调整边框颜色。" }), null, true,
                            _legendBoundaryColorDefault, _legendBoundaryColor),
                    };
                    WinFormPlus.AddColorBoxItems(menu, this, GetTransLation(new string[] { "Colors", "颜色" }),
                    GetTransLation(new string[] { "Adjust color.", "调整颜色。" }), ArchiTed_Grasshopper.Properties.Resources.ColorIcon, true, sets);

                    return menu;
                });

            ClickButtonIcon<LangWindow> ControlButton = new ClickButtonIcon<LangWindow>(_showControl, this, outFuncs(1), true, Properties.Resources.InputLogo, _showControlDefault,
                tips: new string[] { "Click to choose whether to show the param's control.", "点击以选择是否要显示参数的控制项。" },
                createMenu: () =>
                {
                    ContextMenuStrip menu = new ContextMenuStrip() { ShowImageMargin = true };

                    WinFormPlus.AddCheckBoxItem(menu, LanguagableComponent.GetTransLation(new string[] { "Show Bool Control", "显示布尔控制项" }), null, null, this, _showBoolControl, _showBoolControlDefault);
                    WinFormPlus.AddCheckBoxItem(menu, LanguagableComponent.GetTransLation(new string[] { "Show Colour Control", "显示颜色控制项" }), null, null, this, _showColorControl, _showColorControlDefault);
                    WinFormPlus.AddCheckBoxItem(menu, LanguagableComponent.GetTransLation(new string[] { "Show Number Control", "显示数值控制项" }), null, null, this, _showDoubleControl, _showDoubleControlDefault);
                    WinFormPlus.AddCheckBoxItem(menu, LanguagableComponent.GetTransLation(new string[] { "Show Int Control", "显示整数控制项" }), null, null, this, _showIntControl, _showIntControlDefault);
                    WinFormPlus.AddCheckBoxItem(menu, LanguagableComponent.GetTransLation(new string[] { "Show Text Control", "显示文字控制项" }), null, null, this, _showStringControl, _showStringControlDefault);

                    return menu;
                });

            this.Controls = new IRespond[] { LabelButton, treeButton, LegendButton, ControlButton};
        }

        protected override void AppendAdditionComponentMenuItems(ToolStripDropDown menu)
        {
            ToolStripMenuItem exceptionsItem = new ToolStripMenuItem(GetTransLation(new string[] { "WireColors", "连线颜色" }), Properties.Resources.ExceptionIcon, exceptionClick);
            exceptionsItem.ToolTipText = GetTransLation(new string[] { "A window to set the wire's color.", "可以设置连线颜色的窗口。" });
            exceptionsItem.Font = GH_FontServer.StandardBold;
            exceptionsItem.ForeColor = Color.FromArgb(19, 34, 122);

            void exceptionClick(object sender, EventArgs e)
            {
                CreateWindow();
            }
            menu.Items.Add(exceptionsItem);

            GH_DocumentObject.Menu_AppendSeparator(menu);

            WinFormPlus.AddNumberBoxItem(menu, this, GetTransLation(new string[] { "Lebel Font Size", "气泡框中字体大小" }),
    GetTransLation(new string[] { "Set Lebel Font Size", "设置气泡框中字体大小" }),
    ArchiTed_Grasshopper.Properties.Resources.SizeIcon, true, _labelFontSizeDefault, 3, 20, _labelFontSize);

            WinFormPlus.ItemSet<Color>[] sets = new WinFormPlus.ItemSet<Color>[] {

                    new WinFormPlus.ItemSet<Color>( GetTransLation(new string[] { "Text Color", "文字颜色" }),GetTransLation(new string[] { "Adjust text color.", "调整文字颜色。" }),
                    null, true, _labelTextColorDefault, _labelTextColor),

                    new WinFormPlus.ItemSet<Color>( GetTransLation(new string[] { "Background Color", "背景颜色" }), GetTransLation(new string[] { "Adjust background color.", "调整背景颜色。" }),
                    null, true, _labelDackgroundColorDefault, _labelBackgroundColor),

                    new WinFormPlus.ItemSet<Color>(GetTransLation(new string[] { "Boundary Color", "边框颜色" }),
                            GetTransLation(new string[] { "Adjust boundary color.", "调整边框颜色。" }), null, true,
                            _labelBoundaryColorDefault, _labelBoundaryColor),
                    };
            WinFormPlus.AddColorBoxItems(menu, this, GetTransLation(new string[] { "Colors", "颜色" }),
            GetTransLation(new string[] { "Adjust color.", "调整颜色。" }), ArchiTed_Grasshopper.Properties.Resources.ColorIcon, true, sets);

            GH_DocumentObject.Menu_AppendSeparator(menu);

            WinFormPlus.AddNumberBoxItem(menu, this, GetTransLation(new string[] { "Selected Wire Thickness Plus", "选中时连线宽度增值" }),
                GetTransLation(new string[] { "Set Selected Wire Thickness Plus", "设置选中时连线宽度增值" }),
                ArchiTed_Grasshopper.Properties.Resources.SizeIcon, true, _selectWireThicknessDefault, 0, 20, _selectWireThickness);

            WinFormPlus.AddNumberBoxItem(menu, this, GetTransLation(new string[] { "Selected Wire Color Alpha Plus", "选中时连线颜色ALpha通道增量" }),
                GetTransLation(new string[] { "Set Selected Wire Color Alpha Plus", "选中时连线颜色ALpha通道增量" }),
                ArchiTed_Grasshopper.Properties.Resources.ColorIcon, true, _selectWireSolidDefault, -255, 255, _selectWireSolid);

            GH_DocumentObject.Menu_AppendSeparator(menu);

            WinFormPlus.AddLoopBoexItem(menu, this, GetTransLation(new string[] { "Wire Type", "连线类型" }), true, new string[]
            {
                GetTransLation(new string[]{ "Bezier Curve", "贝塞尔曲线"}),
                GetTransLation(new string[]{ "PolyLine", "多段线"}),
                GetTransLation(new string[]{ "Line", "直线"}),
            }, _wireTypeDefault, _wireType);

            if(WireType == 1)
            {
                WinFormPlus.AddNumberBoxItem(menu, this, GetTransLation(new string[] { "    PolyLine Param", "    多段线参数" }),
                    GetTransLation(new string[] { "Click to set the polyline wire param.", "点击以修改多段线的参数。" }),
                    null, true, _polywireParamDefault, 0, 0.5, _polywireParam);
            }

            WinFormPlus.AddLoopBoexItem(menu, this, GetTransLation(new string[] { "Accuracy", "数据精度" }), true, new string[]
            {
                GetTransLation(new string[]{ "Rough", "粗糙"}),
                GetTransLation(new string[]{ "Medium", "中等"}),
                GetTransLation(new string[]{ "High", "高精"}),
            }, _accuracyDefault, _accuracy);

            WinFormPlus.ItemSet<Color>[] sets2 = new WinFormPlus.ItemSet<Color>[]
            {

                new WinFormPlus.ItemSet<Color>( GetTransLation(new string[] { "Default Wire Color", "默认连线颜色" }),
                    GetTransLation(new string[] { "Adjust default wire color.", "调整默认连线颜色。" }),
                    null, true, _defaultColorDefault, _defaultColor),

                new WinFormPlus.ItemSet<Color>( GetTransLation(new string[] { "Selected Wire Color", "选中连线颜色" }),
                    GetTransLation(new string[] { "Adjust selected wire color.", "调整选中连线颜色。" }),
                    null, true, _selectedColorDefault, _selectedColor),

                new WinFormPlus.ItemSet<Color>(GetTransLation(new string[] { "Unselected Wire Color", "未选中连线颜色" }),
                    GetTransLation(new string[] { "Adjust unselected wire color.", "调整未选中连线颜色。" }),
                    null, true, _unselectColorDefalut, _unselectColor),

                new WinFormPlus.ItemSet<Color>(GetTransLation(new string[] { "Empty Wire Color", "空连线颜色" }),
                    GetTransLation(new string[] { "Adjust empty wire color.", "调整空连线颜色。" }),
                    null, true, _emptyColorDefault, _emptyColor),
            };
            WinFormPlus.AddColorBoxItems(menu, this, GetTransLation(new string[] { "Wire Colors", "连线颜色" }),
            GetTransLation(new string[] { "Adjust wire color.", "调整连线颜色。" }), ArchiTed_Grasshopper.Properties.Resources.ColorIcon, true, sets2);

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("", "", "", GH_ParamAccess.item, true);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        #region Algrithm
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.GetData(0, ref _run);
            RemoveAll();

            if (_isFirst)
            {

                this.OnPingDocument().ObjectsAdded += ObjectsAddedAction;
                this.OnPingDocument().ObjectsDeleted += ObjectsDeletedAction;
                Grasshopper.Instances.ActiveCanvas.CanvasPrePaintWires += ActiveCanvas_CanvasPrePaintWires;
                Grasshopper.Instances.ActiveCanvas.CanvasPostPaintWires += ActiveCanvas_CanvasPostPaintWires;
                Grasshopper.Instances.ActiveCanvas.DocumentChanged += ActiveCanvas_DocumentChanged;

                if(ColorDict == null)
                {
                    Readtxt();
                }

                _isFirst = false;
            }

            if (_run)
            {
                foreach (var obj in this.OnPingDocument().Objects)
                {
                    this.AddOneObject(obj);
                }
                Grasshopper.Instances.ActiveCanvas.Refresh();

                if (this.IsShowLegend)
                {
                    this.RenderObjs.Add(new ParamLegend(this));
                }
            }
        }

        private void RemoveAll()
        {
            if(this.RenderObjs != null)
            {
                foreach (var item in this.RenderObjs)
                {
                    if(item is IDisposable)
                    {
                        var dispose = item as IDisposable;
                        dispose.Dispose();
                    }
                }
            }
            this.RenderObjs = new List<IRenderable>();
            this.RenderObjsUnderComponent = new List<IRenderable>();
        }

        /// <summary>
        /// Add a new object into this component.
        /// </summary>
        /// <param name="obj"></param>
        private void AddOneObject(IGH_DocumentObject obj)
        {
            if(obj is IGH_Param)
            {
                IGH_Param param = obj as IGH_Param;
                if (param.Attributes.HasInputGrip)
                {
                    AddOneParam(param);
                }
            }
            else if(obj is IGH_Component)
            {
                IGH_Component com = obj as IGH_Component;
                bool addControl = obj != this;
                foreach (IGH_Param param in com.Params.Input)
                {
                    AddOneParam(param, addControl);
                }
            }
        }

        private void RemoveOneObject(IGH_DocumentObject obj)
        {
            if (obj is IGH_Param)
            {
                IGH_Param param = obj as IGH_Param;
                if (param.Attributes.HasInputGrip)
                {
                    RemoveOneParam(param);
                }
            }
            else if (obj is IGH_Component)
            {
                IGH_Component com = obj as IGH_Component;
                foreach (IGH_Param param in com.Params.Input)
                {
                    RemoveOneParam(param);
                }
            }
        }

        private void RemoveOneParam(IGH_Param param)
        {
            while (true)
            {
                bool findit = false;
                foreach (var item in this.RenderObjs)
                {
                    var result = item.GetType().GetProperty("Target");
                    if (result != null)
                    {
                        var prop = result.GetValue(item);
                        if (prop == param)
                        {
                            if (item is IDisposable)
                            {
                                ((IDisposable)item).Dispose();
                            }
                            this.RenderObjs.Remove(item);
                            findit = true;
                            break;
                        }
                    }
                }
                if (!findit) break;
            }

        }

        private void AddOneParam(IGH_Param param, bool addControl = true)
        {
            this.RenderObjs.Add(new WireConnectRenderItem(param, this));
            if (!this.IsShowControl || !addControl) return;
            Type type = param.Type;

            if (IsPersistentParam(param.GetType()))
            {

                if (this.IsShowBoolControl && typeof(GH_Goo<bool>).IsAssignableFrom(type))
                {
                    Type paramType = typeof(CheckBoxParam<>).MakeGenericType(type);
                    this.RenderObjs.Add((IRenderable)Activator.CreateInstance(paramType, param, this, true, null, 5000, null, true, false));
                    return;
                }    
                else if (this.IsShowColorControl && typeof(GH_Goo<Color>).IsAssignableFrom(type))
                {
                    Type paramType = typeof(ColourSwatchParam<>).MakeGenericType(type);
                    this.RenderObjs.Add((IRenderable)Activator.CreateInstance(paramType, param, this, true, null, 1000, false));
                    return;
                }
                else if (this.IsShowDoubleControl && typeof(GH_Goo<double>).IsAssignableFrom(type))
                {
                    Type paramType = typeof(InputBoxDoubleParam<>).MakeGenericType(type);
                    this.RenderObjs.Add((IRenderable)Activator.CreateInstance(paramType, param, this, true, null, 5000, false));
                    return;
                }
                else if (this.IsShowIntControl &&  typeof(GH_Goo<int>).IsAssignableFrom(type))
                {
                    Type paramType = typeof(InputBoxIntParam<>).MakeGenericType(type);
                    this.RenderObjs.Add((IRenderable)Activator.CreateInstance(paramType, param, this, true, null, 5000, false));
                    return;
                }
                else if (this.IsShowStringControl && typeof(GH_Goo<string>).IsAssignableFrom(type))
                {
                    Type paramType = typeof(InputBoxStringParam<>).MakeGenericType(type);
                    this.RenderObjs.Add((IRenderable)Activator.CreateInstance(paramType, param, this, true, null, 5000, false));
                    return;
                }
 
            }
            if (param.Attributes.HasInputGrip && true)
            {
                Type paramType = typeof(CheckBoxAddObject<>).MakeGenericType(type);
                this.RenderObjs.Add((IRenderable)Activator.CreateInstance(paramType, param, this, true, null, 5000, null, true, false));
                return;
            }
        }

        private void SetTranslateColor()
        {
            GH_Skin.wire_default = Color.Transparent;
            GH_Skin.wire_empty = Color.Transparent;
            GH_Skin.wire_selected_a = Color.Transparent;
            GH_Skin.wire_selected_b = Color.Transparent;
        }

        private void SetDefaultColor()
        {
            GH_Skin.wire_default = this.DefaultColor;
            GH_Skin.wire_empty = this.EmptyColor;
            GH_Skin.wire_selected_a = this.SelectedColor;
            GH_Skin.wire_selected_b = this.UnselectColor;
        }

        private void ActiveCanvas_DocumentChanged(GH_Canvas sender, GH_CanvasDocumentChangedEventArgs e)
        {
            Grasshopper.Instances.ActiveCanvas.CanvasPrePaintWires -= ActiveCanvas_CanvasPrePaintWires;
            Grasshopper.Instances.ActiveCanvas.CanvasPostPaintWires -= ActiveCanvas_CanvasPostPaintWires;

            if (sender.Document == this.OnPingDocument())
            {
                Grasshopper.Instances.ActiveCanvas.CanvasPrePaintWires += ActiveCanvas_CanvasPrePaintWires;
                Grasshopper.Instances.ActiveCanvas.CanvasPostPaintWires += ActiveCanvas_CanvasPostPaintWires;
            }
        }

        private void ObjectsAddedAction(object sender, GH_DocObjectEventArgs e)
        {
            foreach (GH_DocumentObject docObj in e.Objects)
            {
                AddOneObject(docObj);
            }
        }

        private void ObjectsDeletedAction(object sender, GH_DocObjectEventArgs e)
        {
            foreach (GH_DocumentObject docobj in e.Objects)
            {
                RemoveOneObject(docobj);
            }
        }

        private void ActiveCanvas_CanvasPostPaintWires(GH_Canvas sender)
        {
            SetDefaultColor();
        }

        private void ActiveCanvas_CanvasPrePaintWires(GH_Canvas sender)
        {
            SetTranslateColor();
        }

        private void UpdateAllParamProxy()
        {
            _allParamProxy = new List<GooTypeProxy>();
            foreach (IGH_ObjectProxy proxy in Grasshopper.Instances.ComponentServer.ObjectProxies)
            {
                if (!proxy.Obsolete && proxy.Kind == GH_ObjectType.CompiledObject)
                {
                    try
                    {
                        IGH_DocumentObject obj = proxy.CreateInstance();
                        if (IsPersistentParam(obj.GetType()))
                        {
                            GooTypeProxy paramProxy = new GooTypeProxy(((IGH_Param)obj).Type, this);
                            if (!_allParamProxy.Contains(paramProxy))
                            {
                                _allParamProxy.Add(paramProxy);
                            }
                        }
                    }
                    catch
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, proxy.Desc.Name +
                            GetTransLation(new string[] { " failed to create!", "创建失败！" }));
                    }

                }
            }
        }

        public void UpdateAllProxy()
        {
            _allProxy = new List<ParamGlassesProxy>();
            foreach (IGH_ObjectProxy proxy in Grasshopper.Instances.ComponentServer.ObjectProxies)
            {
                if (!proxy.Obsolete && proxy.Kind == GH_ObjectType.CompiledObject)
                {
                    _allProxy.Add(new ParamGlassesProxy(proxy));
                }
            }
        }

        private bool IsPersistentParam(Type type)
        {
            if(type == null)
            {
                return false;
            }
            else if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(GH_PersistentParam<>))
                    return true;
                else if (type.GetGenericTypeDefinition() == typeof(GH_Param<>))
                    return false;
            }

            return IsPersistentParam(type.BaseType);
        }

        #endregion

        #region After Algrithm
        protected override void ResponseToLanguageChanged(object sender, EventArgs e)
        {
            string[] input = new string[] { GetTransLation(new string[] { "Run", "启动" }), GetTransLation(new string[] { "R", "启动" }), GetTransLation(new string[] { "Run", "启动" }) };

            ChangeComponentAtt(this, new string[] {GetTransLation(new string[] { "ParamGlasses", "参数眼镜" }), GetTransLation(new string[] { "Param", "参数" }),
                GetTransLation(new string[] { "To show the wire's and parameter's advances information.Right click or double click to have advanced options.",
                     "显示连线或参数的高级信息。右键或者双击可以获得更多选项。" }) },
                new string[][] { input }, new string[][] { });

            this.ExpireSolution(true);
        }

        public override void CreateWindow()
        {
            WinformControlHelper.CreateWindow(Activator.CreateInstance(this.WindowsType, this) as LangWindow, this);
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            LanguageChanged -= ResponseToLanguageChanged;
            Grasshopper.Instances.ActiveCanvas.DocumentChanged -= ActiveCanvas_DocumentChanged;
            Grasshopper.Instances.ActiveCanvas.CanvasPrePaintWires -= ActiveCanvas_CanvasPrePaintWires;
            Grasshopper.Instances.ActiveCanvas.CanvasPostPaintWires -= ActiveCanvas_CanvasPostPaintWires;
            RemoveAll();
            SetDefaultColor();


            try
            {
                this.OnPingDocument().ObjectsAdded -= ObjectsAddedAction;
                this.OnPingDocument().ObjectsDeleted -= ObjectsDeletedAction;
            }
            catch
            {

            }
            base.RemovedFromDocument(document);
        }

        internal void SetColor(string name, Color color)
        {
            ColorDict[name] = color;
        }

        internal Color GetColor(string name)
        {
            try
            {
                return ColorDict[name];
            }
            catch
            {
                return this.DefaultColor;
            }
        }

        internal void SetCreateProxyGuid(string name, AddProxyParams[] proxies)
        {
            CreateProxyDict[name] = proxies;
        }

        internal AddProxyParams[] GetCreateProxyGuid(string name)
        {
            try
            {
                return CreateProxyDict[name];
            }
            catch
            {
                return null;
            }
        }

        internal void WriteTxt()
        {
            string name = "WireGlasses_Default";
            string path = "";
            try
            {
                path = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location) + "\\" + name + ".txt";
            }
            catch
            {
                var result = (Directory.EnumerateFiles(Grasshopper.Folders.DefaultAssemblyFolder, "*" + name + ".txt", SearchOption.TopDirectoryOnly));
                if (result.Count() > 0)
                {
                    path = result.ElementAt(0);
                }
            }

            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            string saveStr = "";

            foreach (var item in ColorDict.Keys)
            {
                saveStr += item.ToString() + ',' + ColorDict[item].ToArgb().ToString() + "\n";
            }

            sw.Write(saveStr);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private void Readtxt()
        {
            string name = "WireGlasses_Default";
            string path = "";
            try
            {
                path = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location) + "\\" + name + ".txt";
            }
            catch
            {
                var result = (Directory.EnumerateFiles(Grasshopper.Folders.DefaultAssemblyFolder, "*" + name + ".txt", SearchOption.TopDirectoryOnly));
                if(result.Count() > 0)
                {
                    path = result.ElementAt(0);
                }
            }


            ColorDict = new Dictionary<string, Color>();
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);

                try
                {
                    while (true)
                    {

                        string[] strs = sr.ReadLine().Split(',');
                        SetColor(strs[0], Color.FromArgb(int.Parse(strs[1])));


                    }

                }
                catch
                {

                }
            }
            catch
            {

            }

        }

        public override bool Write(GH_IWriter writer)
        {
            if(ColorDict.Count != 0)
            {
                writer.SetInt32("ColorCount", ColorDict.Count);
                int n = 0;
                foreach (string key in ColorDict.Keys)
                {
                    writer.SetString("name" + n.ToString(), key);
                    writer.SetDrawingColor("color" + n.ToString(), ColorDict[key]);
                    n++;
                }
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            ColorDict = new Dictionary<string, Color>();
            int count = 0;
            if( reader.TryGetInt32("ColorCount", ref count))
            {
                for (int n = 0; n < count; n++)
                {
                    ColorDict[reader.GetString("name" + n.ToString())] =
                        reader.GetDrawingColor("color" + n.ToString());
                }
                
            }

            return base.Read(reader);
        }
        #endregion

    }
}