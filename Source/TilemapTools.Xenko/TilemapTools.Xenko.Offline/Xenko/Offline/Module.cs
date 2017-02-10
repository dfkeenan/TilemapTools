﻿using System;
using System.Reflection;
using SiliconStudio.Core;
using SiliconStudio.Core.Reflection;

namespace TilemapTools.Xenko.Offline
{
    public static class Module
    {
        [ModuleInitializer]
        public static void InitializeModule()
        {
            AssemblyRegistry.Register(typeof(Module).GetTypeInfo().Assembly, AssemblyCommonCategories.Assets);
        }
    }
}