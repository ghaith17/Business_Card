using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ProgressSoft.Models
{
    public class BusinessCard //: IValidatableObject
    {
        int customerId;
        string name;
        string gender;
        DateTime date_of_birth;
        string email;
        string phone;
        string photo ="";
        string address;
        [Key]
        public int CustomerId { get => customerId; set => customerId = value; }

        [Required(ErrorMessage = "Required")]
        public string Name { get => name; set => name = value; }

        public string Gender { get => gender; set => gender = value; }

        [Required(ErrorMessage = "The end date is required")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DOB { get => date_of_birth; set => date_of_birth = value; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get => email; set => email = value; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([\+]?(?:00)?[0-9]{1,3}[\s.-]?[0-9]{1,12})([\s.-]?[0-9]{1,4}?)$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get => phone; set => phone = value; }

        public string Photo {
            get {
                if(photo != null)
                    return photo;
                return "";

            }
            set
            {
                this.photo = value;
            }
        }

        [Required(ErrorMessage = "Required")]
        public string Address { get => address; set => address = value; }

    }

    /// <summary>
    /// Custom DateTime JSON serializer/deserializer
    /// </summary>
    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        /// <summary>
        /// DateTime format
        /// </summary>
        private const string Format = "{0:d}";

        /// <summary>
        /// Writes value to JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Value to be written</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(Format));
        }

        /// <summary>
        /// Reads value from JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Target type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="serializer">JSON serialized</param>
        /// <returns>Deserialized DateTime</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            var s = reader.Value.ToString();
            DateTime result;
            if (DateTime.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            return DateTime.Now;
        }

    }

}