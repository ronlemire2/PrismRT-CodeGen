using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace PrismTemplates.UILogic.Services
{
    public static class ErrorMessagesHelper
    {
        static ErrorMessagesHelper()
        {
            ResourceLoader = new ResourceLoaderAdapter(new ResourceLoader());    
        }
        public static IResourceLoader ResourceLoader { get; set; }

		public static string CreateEntityFailedError {
			get { return ResourceLoader.GetString("CreateEntityFailed"); }
		}

		public static string DeleteEntityFailedError {
			get { return ResourceLoader.GetString("DeleteEntityFailed"); }
		}

		public static string ExceptionError {
			get { return ResourceLoader.GetString("Exception"); }
		}

		public static string FldNVarCharInvalidFormatError {
			get { return ResourceLoader.GetString("FldNVarCharInvalidFormat"); }
		}

		public static string FldNVarCharInvalidLengthError {
			get { return ResourceLoader.GetString("FldNVarCharInvalidLength"); }
		}

		public static string FldNVarCharNot92024Error {
			get { return ResourceLoader.GetString("FldNVarCharNot92024"); }
		}

		public static string GetEntitiesAsyncFailedError {
			get { return ResourceLoader.GetString("GetEntitiesAsyncFailed"); }
		}

		public static string HttpRequestExceptionError {
			get { return ResourceLoader.GetString("HttpRequestException"); }
		}

		public static string InvalidFormatError {
			get { return ResourceLoader.GetString("InvalidFormatError"); }
		}

		public static string RegexError {
			get { return ResourceLoader.GetString("RegexError"); }
		}

		public static string RequiredError {
			get { return ResourceLoader.GetString("RequiredError"); }
		}

		public static string UpdateEntityFailedError {
			get { return ResourceLoader.GetString("UpdateEntityFailed"); }
		}

    }
}