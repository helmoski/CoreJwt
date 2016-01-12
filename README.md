# CoreJWT

> [JWT](https://jwt.io/) implementation for .NET core

[![Build status](https://ci.appveyor.com/api/projects/status/539lkkin09waiugb/branch/master?svg=true)](https://ci.appveyor.com/project/rydahhh/corejwt/branch/master)

### Using Statement

```{c#}
using CoreJwt;
```

### Serialize Example

```{c#}
// Create payload
var payload = new Dictionary<string, string>()
{
    {"key", "value"}
};

// Instantiate a JwtSerializer that will use the HMACSHA512
// algorithm with five iterations and "MY_SALT" as the salt
var serializer = new JwtSerializer(JwtHashAlgorithm.HMACSHA512, 5, "MY_SALT");

// Create the JWT
var jwt = serializer.Serialize(payload);

// Value of jwt: eyJ0eXAiOiJKV1QiLCJhbGciOiJITUFDU0hBNTEyIn0=.eyJrZXkiOiJ2YWx1ZSJ9.77+977+977+9Me+/vRnvv71FTg7vv73vv73vv70f77+9Lu+/ve+/ve+/ve+/vWpyE++/ve+/vTfvv70GAzTvv716XV0R77+977+977+977+9zY7vv707DVrvv73vv73vv73vv70r77+977+977+9Ee+/vRzvv71RH++/vXxa77+9YAwuRRzvv70+Sy3vv73vv71j77+9fSx477+977+9azRM77+9eRhj77+9O++/ve+/ve+/ve+/vWfvv71kIu+/vWfvv73vv70fcu+/vVbvv70577+977+977+9bHnvv73FsE1cPiXvv73XvO+/vVUM77+9G++/ve+/ve+/vSVg77+9XEl277+9FDvvv71m6Lq877+9LEjvv73vv73vv71OPwVp77+9Nhgh77+9aM6677+9JERIBu+/vQXvv73vv70m77+977+9bu+/ve+/ve+/ve+/ve+/ve+/vUheEAUA77+9YDbvv71zLmgKVS7vv73vv71o77+9HQoc77+977+9Ou+/vQfvv73vv71IB3Ip340V77+9Pu+/ve+/vUnvv73vv71577+9VyRI77+977+9K3Lvv73vv71J77+977+977+9EA3vv73vv73vv73vv70a77+9De+/vVk=
```

### Deserialize Example

```{c#}
// This example builds off the previous example

// Deserialize the JWT
payload = serializer.Deserialize(jwt);

// The payload will contain {"key","value"}
```