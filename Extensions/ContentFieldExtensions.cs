﻿using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.Media.Fields;
using OrchardCore.Taxonomies.Fields;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Etch.OrchardCore.Fields.Extensions
{
    public static class ContentFieldExtensions
    {
        #region Boolean Field

        public static bool GetBooleanFieldValue(this ContentPart part, string name)
        {
            return part?.Get<BooleanField>(name)?.Value ?? false;
        }

        public static void SetBooleanField(this ContentPart part, string name, bool value)
        {
            var field = part.GetOrCreate<BooleanField>(name);
            field.Value = value;
            part.Apply(name, field);
        }

        #endregion

        #region Date Field

        public static DateTime? GetDateFieldValue(this ContentPart part, string name)
        {
            return part?.Get<DateField>(name)?.Value;
        }

        public static void SetDateField(this ContentPart part, string name, DateTime? value)
        {
            var field = part.GetOrCreate<DateField>(name);
            field.Value = value;
            part.Apply(name, field);
        }

        #endregion

        #region Date Time Field

        public static DateTime? GetDateTimeFieldValue(this ContentPart part, string name)
        {
            return part?.Get<DateTimeField>(name)?.Value;
        }

        public static void SetDateTimeField(this ContentPart part, string name, DateTime? value)
        {
            var field = part.GetOrCreate<DateTimeField>(name);
            field.Value = value;
            part.Apply(name, field);
        }

        #endregion

        #region Html Field

        public static string GetHtmlFieldValue(this ContentPart part, string name)
        {
            return part?.Get<HtmlField>(name)?.Html;
        }

        public static void SetHtmlField(this ContentPart part, string name, string value)
        {
            var field = part.GetOrCreate<HtmlField>(name);
            field.Html = value;
            part.Apply(name, field);
        }

        #endregion

        #region Link Field

        public static string GetLinkFieldValue(this ContentPart part, string name)
        {
            return part?.Get<LinkField>(name)?.Url;
        }

        public static void SetLinkField(this ContentPart part, string name, string value)
        {
            var field = part.GetOrCreate<LinkField>(name);
            field.Url = value;
            part.Apply(name, field);
        }

        #endregion

        #region Media Field

        public static string[] GetMediaFieldPaths(this ContentPart contentPart, string name)
        {
            return contentPart?.Get<MediaField>(name)?.Paths ?? new string[0];
        }

        public static void SetMediaFieldPaths(this ContentPart part, string name, string[] paths)
        {
            var field = part.GetOrCreate<MediaField>(name);
            field.Paths = paths;
            part.Apply(name, field);
        }

        #endregion

        #region Taxonomy Field

        public static string[] GetTaxonomyFieldTerms(this ContentPart part, string name)
        {
            return part?.Get<TaxonomyField>(name)?.TermContentItemIds;
        }

        public static void SetTaxonomyField(this ContentPart part, string name, string taxonomyId, string[] termIds)
        {
            var field = part.GetOrCreate<TaxonomyField>(name);
            field.TaxonomyContentItemId = taxonomyId;
            field.TermContentItemIds = termIds;
            part.Apply(name, field);
        }

        #endregion

        #region Text Field

        public static string GetTextFieldValue(this ContentPart part, string name)
        {
            return part?.Get<TextField>(name)?.Text;
        }

        public static void SetTextField(this ContentPart part, string name, string value)
        {
            var field = part.GetOrCreate<TextField>(name);
            field.Text = value;
            part.Apply(name, field);
        }

        #endregion

        #region Generic

        public static string HtmlClassify(this ContentPartFieldDefinition contentPartFieldDefinition)
        {
            var partName = contentPartFieldDefinition.PartDefinition.Name;
            var fieldName = contentPartFieldDefinition.FieldDefinition.Name;

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"{ToHyphenCase(partName.EndsWith("Part") ? partName.Substring(0, partName.Length - 4) : partName)}-");
            stringBuilder.Append($"{ToHyphenCase(fieldName.EndsWith("Field") ? fieldName.Substring(0, fieldName.Length - 5) : fieldName)}");
            stringBuilder.Append($" {stringBuilder}--{ToHyphenCase(contentPartFieldDefinition.Name)}");

            return stringBuilder.ToString();
        }

        private static string ToHyphenCase(string value)
        {
            return Regex.Replace(value, @"([a-z])([A-Z])", "$1-$2").ToLower();
        }

        #endregion
    }
}
