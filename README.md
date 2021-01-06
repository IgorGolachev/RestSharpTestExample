# RestSharpTestExample
RestSharp (NTLM) + NUnit + NLog

This repo is simple example of API Integration testing (API + DB queries) using RestSharp lib with NTLM auth.
There is a abstract Feature class which serves as base class for other features to be tested.

framework features:
1. Custom call methods
2. Comparisons using FluentAssertions lib
3. Logging to a file using NLog
4. Parallel start (test data still needs to be handled while to be used in parallel, ie locks or concurrently safe data types, etc.)
5. Simple profiling
