Hashing classes for byte array, string, SecureString<br/>
Hash method returns hashedItem and salt as out params, salts should be unique for each iteration<br/>
HashesMatch checks if hash maches provided value and salt<br/>
You can set HashingAlgorithm by changin hashing algorithm in calsses by default its set to SHA-512</br>

For .Net Core <= 2.0.0 is used (HashAlgorithm)CryptoConfig.CreateFromName(HashingAlgorithm) because HashAlgorithm.Create(HashingAlgorithm) is bugged. https://github.com/dotnet/corefx/issues/22626
