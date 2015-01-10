
set Foo.tests=2
echo "Testing for configuration 2"
msbuild TestingMultipleImplementations.sln
vstest.console FooTestImpl1\bin\Debug\FooTestImpl1.dll /logger:trx