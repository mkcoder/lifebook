#!/bin/bash
find . -name 'lifebook.*.nupkg' -exec cp {} nupkg \; && nuget init ./nupkg ./lifebook-nuget-feed