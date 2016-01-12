using System;
using System.Collections.Generic;
using CoreJwt;
using Xunit;

namespace JWT.test
{
    public class JwtSerializerTest
    {
        private const string SALT = "abc123";

        [Fact]
        public void HMACSHA256_1Iteration()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA256, 1, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt = serializer.Serialize(payload);

            payload = serializer.Deserialize(jwt);

            Assert.True(payload.ContainsKey("test"));
            Assert.Equal("test", payload["test"]);
        }

        [Fact]
        public void HMACSHA512_1Iteration()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 1, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt = serializer.Serialize(payload);

            payload = serializer.Deserialize(jwt);

            Assert.True(payload.ContainsKey("test"));
            Assert.Equal("test", payload["test"]);
        }

        [Fact]
        public void HMACSHA256_2Iterations()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA256, 2, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt = serializer.Serialize(payload);

            payload = serializer.Deserialize(jwt);

            Assert.True(payload.ContainsKey("test"));
            Assert.Equal("test", payload["test"]);
        }

        [Fact]
        public void HMACSHA512_2Iterations()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 2, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt = serializer.Serialize(payload);

            payload = serializer.Deserialize(jwt);

            Assert.True(payload.ContainsKey("test"));
            Assert.Equal("test", payload["test"]);
        }

        [Fact]
        public void NotExpired()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 2, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"exp", DateTime.Now.AddSeconds(10).ToString()}
            };

            var jwt = serializer.Serialize(payload);

            payload = serializer.Deserialize(jwt);
        }

        [Fact]
        public void ExpiredJwtThrowsExpiredException()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 2, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"exp", DateTime.Now.AddSeconds(-10).ToString()}
            };

            var jwt = serializer.Serialize(payload);

            var e = Assert.Throws<ExpiredException>(() => serializer.Deserialize(jwt));
            Assert.Equal("JWT has expired.", e.Message);
        }

        [Fact]
        public void InvalidExpirationThrowsArgumentException()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 2, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"exp", "Not a timestamp"}
            };

            var e = Assert.Throws<ArgumentException>(() => serializer.Serialize(payload));
            Assert.Equal("Expiration must be a valid timestamp.", e.Message);
        }

        [Fact]
        public void TamperingCausesTamperingException()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 2, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt = serializer.Serialize(payload);
            jwt = jwt + "test";

            var e = Assert.Throws<TamperingException>(() => serializer.Deserialize(jwt));
            Assert.Equal("JWT shows signs of tampering.", e.Message);
        }

        [Fact]
        public void DifferentAlgorithmsProduceDifferentTokens()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 1, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt512 = serializer.Serialize(payload);

            serializer.Algorithm = JwtHashAlgorithm.HMACSHA256;

            var jwt256 = serializer.Serialize(payload);

            Assert.NotEqual(jwt512, jwt256);
        }

        [Fact]
        public void DifferentIterationsProduceDifferentTokens()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 1, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt1 = serializer.Serialize(payload);

            serializer.Iterations = 2;

            var jwt2= serializer.Serialize(payload);

            Assert.NotEqual(jwt1, jwt2);
        }

        [Fact]
        public void DifferentSaltsProduceDifferentTokens()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 1, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt1 = serializer.Serialize(payload);

            serializer.Salt = "test";

            var jwt2 = serializer.Serialize(payload);

            Assert.NotEqual(jwt1, jwt2);
        }

        [Fact]
        public void DifferentPayloadsProduceDifferentTokens()
        {
            var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 1, SALT);
            var payload = new Dictionary<string, string>()
            {
                {"test", "test"}
            };

            var jwt1 = serializer.Serialize(payload);

            payload.Add("test2", "test2");

            var jwt2 = serializer.Serialize(payload);

            Assert.NotEqual(jwt1, jwt2);
        }
    }
}
