﻿using PrismTemplates.UILogic.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismTemplates.UILogic.Services
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException(ModelValidationResult validationResult) {
            ValidationResult = validationResult;
        }

        public ModelValidationException(string message, Exception innerException)
            : base(message, innerException) {
        }

        public ModelValidationException(string message)
            : base(message) {
        }

        public ModelValidationException() {
        }

        public ModelValidationResult ValidationResult { get; set; }

        public override string Message {
            get {
                string result = string.Empty;
                bool firstItem = true;

                foreach (var key in ValidationResult.ModelState.Keys) {
                    if (!firstItem) result += "\n";

                    var errors = string.Join(", ", ValidationResult.ModelState[key].ToArray());
                    result += string.Format(CultureInfo.CurrentCulture, "{0} : {1}", key, errors);
                    firstItem = false;
                }

                return result;
            }
        }
    }
}
