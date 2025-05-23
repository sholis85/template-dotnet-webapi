﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Infrastructure.Localization;
internal static class Startup
{
    internal static IServiceCollection AddPOLocalization(this IServiceCollection services, IConfiguration config)
    {
        var localizationSettings = config.GetSection(nameof(LocalizationSettings)).Get<LocalizationSettings>();

        if (localizationSettings?.EnableLocalization is true
            && localizationSettings.ResourcesPath is not null)
        {
            services.AddPortableObjectLocalization(options => options.ResourcesPath = localizationSettings.ResourcesPath);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                if (localizationSettings.SupportedCultures != null)
                {
                    var supportedCultures = localizationSettings.SupportedCultures.Select(x => new CultureInfo(x)).ToList();

                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                }

                options.DefaultRequestCulture = new RequestCulture(localizationSettings.DefaultRequestCulture ?? "en-US");
                options.FallBackToParentCultures = localizationSettings.FallbackToParent ?? true;
                options.FallBackToParentUICultures = localizationSettings.FallbackToParent ?? true;
            });

            services.AddSingleton<ILocalizationFileLocationProvider, PoFileLocationProvider>();
        }

        return services;
    }
}
