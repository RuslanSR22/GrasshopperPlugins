﻿/*  Copyright 2020 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using ArchiTed_Grasshopper.WPF;
using ArchiTed_Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoGlasses.WPF
{
    public class GooTypeProxy : ISearchItem
    {
        public ParamGlassesComponent Owner { get; }
        public Color ShowColor => Owner.GetColor(this.TypeFullName);

        public System.Windows.Media.Brush ShowBrush => new System.Windows.Media.SolidColorBrush(ColorExtension.ConvertToMediaColor(ShowColor));

        public string TypeFullName { get; }
        public string TypeName{ get; }
        public bool IsPlugin { get; } = true;
        public string AssemName { get; } = string.Empty;

        public Guid AssemId { get; } = Guid.Empty;

        public GH_AssemblyInfo Assembly { get; } = null;

        public Type DataType { get; }

        public string FindDesc => TypeName + TypeFullName;

        public GooTypeProxy(Type type, ParamGlassesComponent owner)
        {
            this.Owner = owner;
            this.TypeFullName = type.FullName;
            this.TypeName = type.Name;
            this.DataType = type;
            foreach (var item in Grasshopper.Instances.ComponentServer.Libraries)
            {
                if (item.Assembly == type.Assembly)
                {
                    this.Assembly = item;
                    this.IsPlugin = !item.IsCoreLibrary;
                    this.AssemName = item.Name;
                    this.AssemId = item.Id;
                    break;
                }
            }
        }
    }
}
