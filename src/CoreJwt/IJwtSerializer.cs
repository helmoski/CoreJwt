using System;
using System.Collections.Generic;

namespace CoreJwt
{
    public interface IJwtSerializer
    {
        JwtHashAlgorithm Algorithm { get; set; }
        int Iterations { get; set; }
        string Salt { set; }

        /// <summary>
        /// Serializes a JSON web token
        /// </summary>
        /// <param name="payload">Payload for the JWT</param>
        /// <returns>JWT</returns>
        string Serialize(Dictionary<string, string> payload);

        /// <summary>
        /// Deserializes a JSON web token
        /// </summary>
        /// <param name="jwt">JSON web token</param>
        /// <returns>Payload</returns>
        /// <exception cref="TamperingException">JWT shows signs of tampering</exception>
        Dictionary<string, string> Deserialize(string jwt);
    }
}
