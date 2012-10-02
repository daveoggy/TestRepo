@echo off
exec %1 add %2
exec %1 commit -m "Version update"
exec %1 push origin master