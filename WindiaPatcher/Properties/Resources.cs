﻿namespace WindiaPatcher.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class Resources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    resourceMan = new System.Resources.ResourceManager("WindiaPatcher.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get => 
                resourceCulture;
            set => 
                (resourceCulture = value);
        }
    }
}

