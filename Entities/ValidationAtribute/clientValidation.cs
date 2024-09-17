using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Entities.ValidationAtribute
{
    /// <summary>
    /// Kiểm tra thông tin hợp lệ khi user cập nhật lại địa chỉ
    /// </summary>   
    public class EnforceTrueAttribute : ValidationAttribute, IClientModelValidator
    {


        public EnforceTrueAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        private bool _checkedForLocalizer;
        private IStringLocalizer _stringLocalizer;

        /// <summary>
        /// other property (should be a bool in a hidden field) which determines if this validation should be applied
        /// for example I want to use this attribute to require a user to check the box
        /// agreeing to the registration agreement, but only if the site registration agreement has been specified
        /// ie some sites may not even have an agreement defined
        /// </summary>
        public string OtherProperty { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           // var otherPropertyInfo = null;// validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
           // if (otherPropertyInfo == null)
           // {
                //return ValidationResult.Success;
            //}

           // object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            //if (otherPropertyValue == null) return ValidationResult.Success;
            //if (((bool)otherPropertyValue) == false) return ValidationResult.Success;

            if (value == null) return new ValidationResult("value cannot be null");
            if (value.GetType() != typeof(bool)) throw new InvalidOperationException("can only be used on boolean properties.");
            if ((bool)value == true)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return name; //string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

           // CheckForLocalizer(context);
            var errorMessage = GetErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-enforcetrue", errorMessage);
            MergeAttribute(context.Attributes, "data-val-other", "#" + OtherProperty);
        }

        private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }

        //private void CheckForLocalizer(ClientModelValidationContext context)
        //{
        //    if (!_checkedForLocalizer)
        //    {
        //        _checkedForLocalizer = true;

        //        var services = context.ActionContext.HttpContext.RequestServices;
        //        var options = services.GetRequiredService<IOptions<MvcDataAnnotationsLocalizationOptions>>();
        //        var factory = services.GetService<IStringLocalizerFactory>();

        //        var provider = options.Value.DataAnnotationLocalizerProvider;
        //        if (factory != null && provider != null)
        //        {
        //            _stringLocalizer = provider(
        //                context.ModelMetadata.ContainerType ?? context.ModelMetadata.ModelType,
        //                factory);
        //        }
        //    }
        //}

        private string GetErrorMessage(string displayName)
        {
            if (_stringLocalizer != null &&
                !string.IsNullOrEmpty(ErrorMessage) &&
                string.IsNullOrEmpty(ErrorMessageResourceName) &&
                ErrorMessageResourceType == null)
            {
                return _stringLocalizer[ErrorMessage, displayName];
            }

            return FormatErrorMessage(displayName);
        }

        //public VerifyAgeAttribute(int age)
        //{
        //    Age = age;
        //}

        //public int Age { get; private set; }

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    if ((DateTime?)value <= DateTime.Now.AddYears(-Age))
        //    {
        //        return ValidationResult.Success;
        //    }
        //    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        //}

        //void IClientModelValidator.AddValidation(ClientModelValidationContext context)
        //{
        //    context.Attributes.Add("data-val-custom-verifyage", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        //    context.Attributes.Add(
        //        "data-val-custom-verifyage-validationcallback",
        //        $@"function(options) {{
        //            var now = new Date();
        //            return options.value && options.value <= now.setFullYear(now.getFullYear() - {Age});
        //        }}");
        //}

        //public override string FormatErrorMessage(string name)
        //{
        //    return string.Format(ErrorMessageString, name, Age);
        //}

        //public CheckExitsUpdateClientAttribute()
        //    : base("The value of the {0} field is not valid")
        //{
        //}
        //public void AddValidation(ClientModelValidationContext context)
        //{
        //    MergeAttribute(context.Attributes, "data-val-custom-checkexitsupdateclient",
        //            FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        //    MergeAttribute(context.Attributes, "data-val-custom-checkexitsupdateclient-validationcallback", "checkexitsupdateclient");
        //}
        //bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        //{
        //    if (attributes.ContainsKey(key))
        //    {
        //        return false;
        //    }
        //    attributes.Add(key, value);
        //    return true;
        //}
    }

    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public sealed class CheckExitsUpdateClient : ValidationAttribute, IClientValidatable
    //{

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        if (value != null)
    //        {
    //            string _company = value.ToString();
    //            if (string.IsNullOrEmpty(_company))
    //            {
    //                return new ValidationResult("Mã số thuế không đúng định dạng");
    //            }
    //        }

    //        return ValidationResult.Success;
    //    }
    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(System.Web.Mvc.ModelMetadata metadata, System.Web.Mvc.ControllerContext context)
    //    {
    //        ModelClientValidationRule mvr = new ModelClientValidationRule();
    //        mvr.ErrorMessage = ErrorMessage;
    //        mvr.ValidationType = "checkexitsupdateclient";

    //        return new[] { mvr };
    //    }

    // }
}
