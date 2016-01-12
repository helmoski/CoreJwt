using Microsoft.AspNet.Cryptography.KeyDerivation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreJwt
{
    public class JwtSerializer : IJwtSerializer
    {
        private string _salt;
        private JwtHashAlgorithm _algorithm;
        private KeyDerivationPrf _prf;

        public int Iterations { get; set; }

        public JwtHashAlgorithm Algorithm {
            get
            {
                return _algorithm;
            }
            set
            {
                _algorithm = value;
                if(value == JwtHashAlgorithm.HMACSHA256)
                {
                    _prf = KeyDerivationPrf.HMACSHA256;
                }
                else
                {
                    _prf = KeyDerivationPrf.HMACSHA512;
                }
            }
        }

        public string Salt {
            set {
                _salt = value;
            }
        }

        public JwtSerializer(JwtHashAlgorithm algorithm, int iterations, string salt)
        {
            Algorithm = algorithm;
            Iterations = iterations;
            Salt = salt;
        }

        public string Serialize(Dictionary<string, string> payload)
        {
            var header = new Dictionary<string, string>()
            {
                {"typ", "JWT"},
                {"alg", _algorithm == JwtHashAlgorithm.HMACSHA256 ? "HMACSHA256" : "HMACSHA512"}
            };
            var headerJson = JsonConvert.SerializeObject(header);
            var encodedHeader = Encode(headerJson);

            var payloadJson = JsonConvert.SerializeObject(payload);
            var encodedPayload = Encode(payloadJson);

            var signatureBytes = KeyDerivation.Pbkdf2(encodedPayload, Encoding.UTF8.GetBytes(_salt), _prf, Iterations, 256);
            var signature = Encoding.UTF8.GetString(signatureBytes);
            var encodedSignature = Encode(signature);

            var jwt = String.Format("{0}.{1}.{2}", encodedHeader, encodedPayload, encodedSignature);

            return jwt;
        }

        public Dictionary<string, string> Deserialize(string jwt)
        {
            var parts = jwt.Split('.');

            if(parts.Length != 3)
            {
                throw new TamperingException();
            }

            var signature = Decode(parts[2]);
            var encodedPayload = parts[1];
            
            var checkSignatureBytes = KeyDerivation.Pbkdf2(encodedPayload, Encoding.UTF8.GetBytes(_salt), _prf, Iterations, 256);
            var checkSignature = Encoding.UTF8.GetString(checkSignatureBytes);

            if (signature != checkSignature)
            {
                throw new TamperingException();
            }

            var payloadJson = Decode(encodedPayload);
            var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(payloadJson);

            return payload;
        }

        private string Encode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var output = Convert.ToBase64String(bytes);
            return output;
        }

        private string Decode(string input)
        {
            try
            {
                var bytes = Convert.FromBase64String(input);
                var output = Encoding.UTF8.GetString(bytes);
                return output;
            }
            catch (FormatException)
            {
                throw new TamperingException();
            }
        }
    }
}
