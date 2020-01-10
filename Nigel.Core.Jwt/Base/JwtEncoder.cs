using System;
using System.Collections.Generic;
using Nigel.Core.Jwt.Algorithms;

using static Nigel.Core.Jwt.Internal.EncodingHelper;

namespace Nigel.Core.Jwt
{
    /// <summary>
    /// Encodes Jwt.
    /// </summary>
    public sealed class JwtEncoder : IJwtEncoder
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IBase64UrlEncoder _urlEncoder;

        /// <summary>
        /// Creates an instance of <see cref="JwtEncoder" />
        /// </summary>
        /// <param name="jsonSerializer">The Json Serializer</param>
        /// <param name="algorithm">The Jwt Algorithm</param>
        /// <param name="urlEncoder">The Base64 URL Encoder</param>
        public JwtEncoder(IJwtAlgorithm algorithm, IJsonSerializer jsonSerializer, IBase64UrlEncoder urlEncoder)
        {
            _algorithm = algorithm;
            _jsonSerializer = jsonSerializer;
            _urlEncoder = urlEncoder;
        }

        /// <inheritdoc />
        public string Encode(object payload, string key) =>
            Encode(null, payload, key is object ? GetBytes(key) : null);

        /// <inheritdoc />
        public string Encode(object payload, byte[] key) =>
            Encode(null, payload, key);

        /// <inheritdoc />
        public string Encode(IDictionary<string, object> extraHeaders, object payload, string key) =>
            Encode(extraHeaders, payload, GetBytes(key));

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException" />
        public string Encode(IDictionary<string, object> extraHeaders, object payload, byte[] key)
        {
            if (payload is null)
                throw new ArgumentNullException(nameof(payload));

            var segments = new List<string>(3);

            var header = extraHeaders is null ? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) : new Dictionary<string, object>(extraHeaders, StringComparer.OrdinalIgnoreCase);
            header.Add("typ", "JWT");
            header.Add("alg", _algorithm.Name);

            var headerBytes = GetBytes(_jsonSerializer.Serialize(header));
            var payloadBytes = GetBytes(_jsonSerializer.Serialize(payload));

            segments.Add(_urlEncoder.Encode(headerBytes));
            segments.Add(_urlEncoder.Encode(payloadBytes));

            var stringToSign = String.Join(".", segments.ToArray());
            var bytesToSign = GetBytes(stringToSign);

            var signature = _algorithm.Sign(key, bytesToSign);
            segments.Add(_urlEncoder.Encode(signature));

            return String.Join(".", segments.ToArray());
        }
    }
}
