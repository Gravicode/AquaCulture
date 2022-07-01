using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Globalization;
using System.Net.Sockets;

namespace AquaCulture.Tools
{
    /// <summary>
    /// Form Validation Class
    /// </summary>
    public class FormValidation
    {
        StringBuilder sbMessage = new StringBuilder();
        List<FormField> fields = new List<FormField>();
        static bool _invalidMail = false;
        static string _locale = "en-US";


        public FormValidation()
        {

        }
        public FormValidation(string locale)
        {
            _locale = locale;
        }

        /// <summary>
        /// Register a field
        /// </summary>
        /// <param name="name">Field name</param>
        /// <param name="title">Field label</param>
        /// <param name="value">Field value</param>
        /// <param name="validations">Comma separated value of these validation keys : Required,Email,LimitLength,Number,AlphaNumeric,Date,Time,Regex</param>
        /// <returns>FormField Object</returns>
        public FormField RegisterField(string name, string title, string value, string validations)
        {
            return RegisterField(name, title, value, validations, 0, "", "");
        }

        /// <summary>
        /// Register a field
        /// </summary>
        /// <param name="name">Field name</param>
        /// <param name="title">Field label</param>
        /// <param name="value">Field value</param>
        /// <param name="validations">Comma separated value of these validation keys : Required,Email,LimitLength,Number,AlphaNumeric,Date,Time,Regex</param>
        /// <param name="limit">Maximum string length. Required only for LimitLength validation</param>
        /// <returns>FormField Object</returns>
        public FormField RegisterField(string name, string title, string value, string validations, int limit)
        {
            return RegisterField(name, title, value, validations, limit, "", "");
        }

        /// <summary>
        /// Register a field
        /// </summary>
        /// <param name="name">Field name</param>
        /// <param name="title">Field label</param>
        /// <param name="value">Field value</param>
        /// <param name="validations">Comma separated value of these validation keys : Required,Email,LimitLength,Number,AlphaNumeric,Date,Time,Regex</param>
        /// <param name="limit">Maximum string length. Required only for LimitLength validation</param>
        /// <param name="regex">Regex string. Required only for Regex validation</param>
        /// <returns>FormField Object</returns>
        public FormField RegisterField(string name, string title, string value, string validations, int limit, string regex)
        {
            return RegisterField(name, title, value, validations, limit, regex, "");
        }

        /// <summary>
        /// Register a field
        /// </summary>
        /// <param name="name">Field name</param>
        /// <param name="title">Field label</param>
        /// <param name="value">Field value</param>
        /// <param name="validations">Comma separated value of these validation keys : Required,Email,LimitLength,Number,AlphaNumeric,Date,Time,Regex</param>
        /// <param name="regex">Regex string. Required only for Regex validation</param>
        /// <param name="limit">Maximum string length. Required only for LimitLength validation</param>
        /// <param name="fieldNameToCompare">Field name to compare</param>
        /// <returns>FormField Object</returns>
        public FormField RegisterField(string name, string title, string value, string validations, int limit, string regex, string fieldNameToCompare)
        {
            string[] vals = validations.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            List<FormField.ValidationType> valtypes = new List<FormField.ValidationType>();
            foreach (string item in vals)
            {
                try
                {
                    valtypes.Add((FormField.ValidationType)Enum.Parse(typeof(FormField.ValidationType), item));
                }
                catch { }
            }
            FormField fld = new FormField();
            fld.Name = name;
            fld.Title = title;
            fld.Value = value;
            fld.Validations = valtypes;
            fld.Regex = regex;
            fld.Length = limit;
            fld.CompareFieldName = fieldNameToCompare;
            fields.Add(fld);
            return fld;
        }

        /// <summary>
        /// Initiate field validations. Must be called after registering fields. 
        /// </summary>
        /*
        public void ValidateFields()
        {
            StringBuilder errorMsg = new StringBuilder();
            foreach (FormField item in fields)
            {
                foreach (FormField.ValidationType type in item.Validations)
                {
                    switch (type)
                    {
                        case FormField.ValidationType.Required:
                            if (string.IsNullOrEmpty(item.Message))
                            {
                                if (!IsValidRequired(item)) item.Message = "Field " + item.Title + " is required.";
                            }
                            break;
                        case FormField.ValidationType.Email:
                            if (string.IsNullOrEmpty(item.Message))
                            {
                                if (!IsValidEmail(item)) item.Message = "Field " + item.Title + " is invalid. Please enter a valid email.";
                            }
                            break;
                        case FormField.ValidationType.LimitLength:
                            if (string.IsNullOrEmpty(item.Message) && !string.IsNullOrEmpty(item.Value))
                            {
                                if (!ValidateLength(item)) item.Message = "Field " + item.Title + " is limited to " + item.Length.ToString() + " characters only.";
                            }
                            break;
                        case FormField.ValidationType.Number:
                            if (string.IsNullOrEmpty(item.Message) && !string.IsNullOrEmpty(item.Value))
                            {
                                if (!isValidNumeric(item)) item.Message = "Field " + item.Title + " is invalid. Please enter numeric characters.";
                            }
                            break;
                        case FormField.ValidationType.AlphaNumeric:
                            if (string.IsNullOrEmpty(item.Message) && !string.IsNullOrEmpty(item.Value))
                            {
                                if (!isValidAlphaNumeric(item)) item.Message = "Field " + item.Title + " is invalid. Please enter alphanumeric characters.";
                            }
                            break;
                        case FormField.ValidationType.Date:
                            if (string.IsNullOrEmpty(item.Message) && !string.IsNullOrEmpty(item.Value))
                            {
                                if (!IsValidDate(item)) item.Message = "Field " + item.Title + " is invalid. Please enter a date using '"  + "' format.";
                            }
                            break;
                        case FormField.ValidationType.Time:
                            if (string.IsNullOrEmpty(item.Message) && !string.IsNullOrEmpty(item.Value))
                            {
                                if (!IsValidTime(item)) item.Message = "Field " + item.Title + " is invalid. Please enter a time using 'hh:mm:ss' format.";
                            }
                            break;
                        case FormField.ValidationType.Regex:
                            if (string.IsNullOrEmpty(item.Message) && !string.IsNullOrEmpty(item.Value))
                            {
                                if (!IsValidRegex(item)) item.Message = "Field " + item.Title + " is invalid.";
                            }
                            break;
                        default:
                            break;
                    }
                }

            }
            RebuildMessage();
        }
        */
        /// <summary>
        /// Check form validation for errors. Must be called after ValidateFields method.
        /// </summary>
        /// <returns>True if no error found.</returns>
        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(sbMessage.ToString());
            }
        }

        /// <summary>
        /// Append custom error message if current field name has no error message.
        /// </summary>
        /// <param name="message">Custom error message</param>
        /// <param name="fieldname">Field Name</param>
        public void AddCustomError(string fieldname, string message)
        {
            FormField mf = fields.Find(i => i.Name == fieldname);
            if ((mf != null) && (string.IsNullOrEmpty(mf.Message)))
            {
                mf.Message = message;
            }
            RebuildMessage();
        }

        /// <summary>
        /// Rebuild error message list
        /// </summary>
        void RebuildMessage()
        {
            sbMessage = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            fields.ForEach(delegate (FormField f)
            {
                if (!string.IsNullOrEmpty(f.Message))
                {
                    sb.Append("<li>" + f.Message + "</li>");
                }
            });
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                sbMessage.Append("<ul class=\"errorList\">" + sb.ToString() + "</ul>");
            }
        }

        /// <summary>
        /// Get comma separated field names which raise errors
        /// </summary>
        /// <param name="fieldNames">Additional field names. Comma separated.</param>
        /// <returns>Comma separated field names</returns>
        public string GetErrorFields(string fieldNames)
        {
            StringBuilder sb = new StringBuilder();
            foreach (FormField item in fields)
            {
                if (!string.IsNullOrEmpty(item.Message))
                {
                    if (!string.IsNullOrEmpty(sb.ToString())) sb.Append(",");
                    sb.Append(item.Name);
                }
            }

            if (!string.IsNullOrEmpty(sb.ToString())) sb.Append(",");
            sb.Append(fieldNames);
            return sb.ToString();
        }

        /// <summary>
        /// Get registered field
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <returns>Field object</returns>
        public FormField GetField(string fieldName)
        {
            return fields.FirstOrDefault(i => i.Name == fieldName);
        }

        /// <summary>
        /// Get error message list
        /// </summary>
        /// <param name="useJsonFormat">Set to true to get JSON formatted error list</param>
        /// <returns>Error message list</returns>
        public string GetErrorMessages(bool useJsonFormat)
        {
            if (useJsonFormat)
            {
                StringBuilder sbJson = new StringBuilder();
                fields.ForEach(delegate (FormField f)
                {
                    if (!string.IsNullOrEmpty(f.Message))
                    {
                        if (!string.IsNullOrEmpty(sbJson.ToString())) sbJson.Append(",");
                        sbJson.Append("{\"f\":\"" + f.Name + "\",\"m\":\"" + f.Message + "\"}");
                    }
                });
                return "[" + sbJson.ToString() + "]";
            }
            else return sbMessage.ToString();
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                _invalidMail = true;
            }
            return match.Groups[1].Value + domainName;
        }
        public static bool IsValidEmail(string Value)
        {
            string strIn = Value;
            _invalidMail = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (_invalidMail) return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);
        }
        public bool IsValidRequired(FormField field)
        {
            try
            {
                return (field != null) && !string.IsNullOrEmpty(field.Value.Trim());
            }
            catch { return false; }
        }
        public static bool isValidNumeric(FormField field)
        {
            bool ret = false;
            try
            {
                //int tmp = 0;
                //int.TryParse(field.Value, out tmp);
                //ret = tmp.ToString() == field.Value;

                decimal tmp = 0;
                CultureInfo culture = new CultureInfo(_locale);
                ret = decimal.TryParse(field.Value, NumberStyles.Number, culture, out tmp);
            }
            catch { ret = false; }
            return ret;
        }
        public bool isValidAlphaNumeric(FormField field)
        {
            Regex rg = new Regex("[^a-zA-Z0-9]");
            //if has non AlpahNumeric char, return false, else return true.
            return !rg.IsMatch(field.Value);
        }
        public bool IsValidDate(FormField field)
        {
            string format = "dd/MM/yyyy";
            DateTime Test;
            CultureInfo culture = new CultureInfo(_locale);
            return (DateTime.TryParseExact(field.Value, format, culture, DateTimeStyles.None, out Test) == true);
        }
        public bool IsValidTime(FormField field)
        {
            bool ret = false;
            try
            {
                string[] tm = field.Value.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                int hour = tm.Length > 0 ? int.Parse(tm[0]) : 0;
                int minute = tm.Length > 1 ? int.Parse(tm[1]) : 0;
                int second = tm.Length > 2 ? int.Parse(tm[2]) : 0;
                ret = ((hour >= 0) && (hour < 24) && (minute >= 0) && (minute < 60) && (second >= 0) && (second < 60));
            }
            catch { ret = false; }
            return ret;
        }
        public bool IsValidRegex(FormField field)
        {
            Regex rg = new Regex(field.Regex);
            return rg.IsMatch(field.Value);
        }
        public bool ValidateLength(FormField field)
        {
            return field.Value.Length <= field.Length;
        }

        public bool IsDateLowerThan(FormField field, DateTime dateToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                return value1 < dateToCompare;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateLowerThan(FormField field, FormField fieldToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                DateTime value2 = DateTime.ParseExact(fieldToCompare.Value, format, culture, DateTimeStyles.None);
                return value1 < value2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateLowerOrEqualThan(FormField field, DateTime dateToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                return value1 <= dateToCompare;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateLowerOrEqualThan(FormField field, FormField fieldToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                DateTime value2 = DateTime.ParseExact(fieldToCompare.Value, format, culture, DateTimeStyles.None);
                return value1 <= value2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateGreaterThan(FormField field, DateTime dateToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                return value1 > dateToCompare;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateGreaterThan(FormField field, FormField fieldToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                DateTime value2 = DateTime.ParseExact(fieldToCompare.Value, format, culture, DateTimeStyles.None);
                return value1 > value2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateGreaterOrEqualThan(FormField field, DateTime dateToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                return value1 >= dateToCompare;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateGreaterOrEqualThan(FormField field, FormField fieldToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                DateTime value2 = DateTime.ParseExact(fieldToCompare.Value, format, culture, DateTimeStyles.None);
                return value1 >= value2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateEqual(FormField field, DateTime dateToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                return value1 == dateToCompare;
            }
            catch
            {
                return false;
            }
        }
        public bool IsDateEqual(FormField field, FormField fieldToCompare)
        {
            try
            {
                CultureInfo culture = new CultureInfo(_locale);
                string format = "dd/MM/yyyy";
                DateTime value1 = DateTime.ParseExact(field.Value, format, culture, DateTimeStyles.None);
                DateTime value2 = DateTime.ParseExact(fieldToCompare.Value, format, culture, DateTimeStyles.None);
                return value1 == value2;
            }
            catch
            {
                return false;
            }
        }

        public bool IsNumberLowerThan(FormField field, FormField fieldToCompare)
        {
            try
            {
                Decimal value1 = Decimal.Parse(field.Value);
                Decimal value2 = Decimal.Parse(fieldToCompare.Value);
                return value1 < value2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsNumberLowerOrGreaterThan(FormField field, FormField fieldToCompare)
        {
            try
            {
                Decimal value1 = Decimal.Parse(field.Value);
                Decimal value2 = Decimal.Parse(fieldToCompare.Value);
                return value1 <= value2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsNumberGreaterThan(FormField field, FormField fieldToCompare)
        {
            try
            {
                Decimal value1 = Decimal.Parse(field.Value);
                Decimal value2 = Decimal.Parse(fieldToCompare.Value);
                return value1 > value2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsNumberGreaterOrEqualThan(FormField field, FormField fieldToCompare)
        {
            try
            {
                Decimal value1 = Decimal.Parse(field.Value);
                Decimal value2 = Decimal.Parse(fieldToCompare.Value);
                return value1 >= value2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsNumberEqual(FormField field, FormField fieldToCompare)
        {
            try
            {
                Decimal value1 = Decimal.Parse(field.Value);
                Decimal value2 = Decimal.Parse(fieldToCompare.Value);
                return value1 == value2;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsIpInRange(IPAddress ipAddr, IPAddress lowerIp, IPAddress upperIp)
        {
            AddressFamily addressFamily = lowerIp.AddressFamily;
            byte[] lowerBytes = lowerIp.GetAddressBytes();
            byte[] upperBytes = upperIp.GetAddressBytes();
            if (ipAddr.AddressFamily != addressFamily)
            {
                return false;
            }

            byte[] addressBytes = ipAddr.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < lowerBytes.Length &&
                (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                upperBoundary &= (addressBytes[i] == upperBytes[i]);
            }

            return true;
        }

    }

    /// <summary>
    /// Form Field class for validation
    /// </summary>
    public class FormField
    {
        public enum ValidationType
        {
            Required = 0,
            Email,
            LimitLength,
            Number,
            AlphaNumeric,
            Date,
            Time,
            Regex
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Regex { get; set; }
        public int Length { get; set; }
        public string CompareFieldName { get; set; }
        public List<ValidationType> Validations { get; set; }
        public string Message { get; set; }
        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(Message);
            }
        }
    }
}
