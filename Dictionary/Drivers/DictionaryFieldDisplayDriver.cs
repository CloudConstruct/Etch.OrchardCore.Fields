﻿using Microsoft.Extensions.Localization;
using Moov2.OrchardCore.Fields.Dictionary.Fields;
using Moov2.OrchardCore.Fields.Dictionary.Models;
using Moov2.OrchardCore.Fields.Dictionary.Settings;
using Moov2.OrchardCore.Fields.Dictionary.ViewModels;
using Newtonsoft.Json;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moov2.OrchardCore.Fields.Dictionary.Drivers
{
    public class DictionaryFieldDisplayDriver : ContentFieldDisplayDriver<DictionaryField>
    {
        #region PublicVariables

        public IStringLocalizer T { get; set; }

        #endregion PublicVariables

        #region Constructor

        public DictionaryFieldDisplayDriver(IStringLocalizer<DictionaryFieldDisplayDriver> localizer)
        {
            T = localizer;
        }

        #endregion Constructor

        #region ContentFieldDisplayDriver<DictionaryField>

        #region Display

        public override IDisplayResult Display(DictionaryField field, BuildFieldDisplayContext context)
        {
            return Initialize<DisplayDictionaryFieldViewModel>(GetDisplayShapeType(context), model =>
            {
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
                model.Data = field.Data;
            })
            .Location("Content")
            .Location("SummaryAdmin", "");
        }

        #endregion Display

        #region Edit

        public override IDisplayResult Edit(DictionaryField field, BuildFieldEditorContext context)
        {
            var settings = GetSettings(context);
            var isNew = field.ContentItem.Id == 0;
            return Initialize<EditDictionaryFieldViewModel>(GetEditorShapeType(context), model =>
            {
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;

                model.Data = JsonConvert.SerializeObject(isNew ? GetDefaults(context) : field.Data);

                model.MaxEntries = settings?.MaxEntries;
                model.MinEntries = settings?.MinEntries;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(DictionaryField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var model = new EditDictionaryFieldViewModel();

            await updater.TryUpdateModelAsync(model, Prefix, m => m.Data);

            var settings = GetSettings(context);

            field.Data = JsonConvert.DeserializeObject<List<DictionaryItem>>(model.Data);

            if (settings?.MinEntries > 0 && (field.Data == null || field.Data.Count < settings.MinEntries))
            {
                updater.ModelState.AddModelError($"{Prefix}.{nameof(model.Data)}", T["You must specify at least {0} items.", settings.MinEntries]);
            }

            if (settings?.MaxEntries > 0 && field.Data?.Count > settings.MaxEntries)
            {
                updater.ModelState.AddModelError($"{Prefix}.{nameof(model.Data)}", T["You can specify at most {0} items.", settings.MaxEntries]);
            }

            return Edit(field, context);
        }

        #endregion Edit

        #endregion ContentFieldDisplayDriver<DictionaryField>

        #region Helpers

        private DictionaryFieldSettings GetSettings(BuildFieldEditorContext context)
        {
            return context?.PartFieldDefinition?.Settings?.ToObject<DictionaryFieldSettings>();
        }

        private IList<DictionaryItem> GetDefaults(BuildFieldEditorContext context)
        {
            var settingsValue = GetSettings(context)?.DefaultData;
            if (settingsValue != null)
            {
                return JsonConvert.DeserializeObject<IList<DictionaryItem>>(settingsValue);
            }
            return null;
        }

        #endregion Helpers
    }
}
