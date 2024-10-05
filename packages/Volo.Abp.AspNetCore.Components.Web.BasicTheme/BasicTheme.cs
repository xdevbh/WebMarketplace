using System;
using Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic.Layouts;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;
using Volo.Abp.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic.Layouts;

namespace Volo.Abp.AspNetCore.Components.Web.BasicTheme;

[ThemeName(Name)]
public class BasicTheme : ITheme, ITransientDependency
{
    public const string Name = "Basic";

    public virtual Type GetLayout(string name, bool fallbackToDefault = true)
    {
        switch (name)
        {
            case StandardLayouts.Application:
            case StandardLayouts.Account:
                return typeof(MainLayout);
            case StandardLayouts.Empty:
                return typeof(NullLayout);
            default:
                return fallbackToDefault ? typeof(MainLayout) : typeof(NullLayout);
        }
    }
}