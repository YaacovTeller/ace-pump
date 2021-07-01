@echo off

call "\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvarsall.bat" x86

echo.
echo Running publish for `%1`...
msbuild SyncForQbPublish.msbuild  /p:configuration=%1

echo Publish complete
pause