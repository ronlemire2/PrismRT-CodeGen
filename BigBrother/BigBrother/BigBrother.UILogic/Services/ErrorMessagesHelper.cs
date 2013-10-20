using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace BigBrother.UILogic.Services
{
    public static class ErrorMessagesHelper
    {
        static ErrorMessagesHelper()
        {
            ResourceLoader = new ResourceLoaderAdapter(new ResourceLoader());    
        }
        public static IResourceLoader ResourceLoader { get; set; }

		public static string CreatePhoneCallFailedError {
			get { return ResourceLoader.GetString("CreatePhoneCallFailed"); }
		}

		public static string DeletePhoneCallFailedError {
			get { return ResourceLoader.GetString("DeletePhoneCallFailed"); }
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

		public static string GetPhoneCallsAsyncFailedError {
			get { return ResourceLoader.GetString("GetPhoneCallsAsyncFailed"); }
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

		public static string UpdatePhoneCallFailedError {
			get { return ResourceLoader.GetString("UpdatePhoneCallFailed"); }
		}

    }
}
