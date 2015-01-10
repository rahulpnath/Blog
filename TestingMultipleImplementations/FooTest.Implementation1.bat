
set Foo.tests=1
echo "Testing for configuration 1"
msbuild TestingMultipleImplementations.sln
vstest.console FooTestImpl1\bin\Debug\FooTestImpl1.dll  /logger:trx