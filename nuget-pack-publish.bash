﻿find . -name '*.nupkg' -print0 | xargs -print0 -0 -n1 dirname