using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IsaksMusic.Data
{
    public class LinkExistsAttribute: ValidationAttribute
    {
        public string Url { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            Url = value.ToString();
            bool exists = false;

            try
            {
                HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    exists = true;
                }
                response.Close();

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                exists = false;
            }

            if (exists)
            {
                return null;
            }

            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
