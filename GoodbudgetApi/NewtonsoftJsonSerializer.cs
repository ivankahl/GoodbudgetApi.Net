using System;
using RestSharp;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace GoodbudgetApi
{
    /// <summary>
    /// Default JSON serializer for request bodies
    /// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
    /// </summary>
    internal class NewtonsoftJsonSerializer : ISerializer, IDeserializer
    {
        #region Fields & Properties

        /// <summary>
        /// Gets the default NewtonsoftJsonSerializer instance.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static NewtonsoftJsonSerializer Default => _default.Value;

        /// <summary>
        /// The default instance holder.
        /// </summary>
        private static readonly Lazy<NewtonsoftJsonSerializer> _default = 
            new Lazy<NewtonsoftJsonSerializer>(LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string RootElement { get; set; }
        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Content type for serialized content
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The serializer implementation.
        /// </summary>
        private readonly global::Newtonsoft.Json.JsonSerializer _serializer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonSerializer"/> class.
        /// </summary>
        public NewtonsoftJsonSerializer()
        {
            ContentType = "application/json";
            _serializer = new global::Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonSerializer"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        public NewtonsoftJsonSerializer(global::Newtonsoft.Json.JsonSerializer serializer)
        {
            ContentType = "application/json";
            _serializer = serializer;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Serialize the object as JSON
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON as String</returns>
        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    _serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }

        /// <summary>
        /// Deserializes the specified response.
        /// </summary>
        /// <typeparam name="T">The response type.</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>The strongly-typed deserialized response.</returns>
        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        #endregion
    }
}